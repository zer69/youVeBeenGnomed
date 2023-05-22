using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]

    [SerializeField] private b_GameEvent newGame;

    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance {get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple DP manager in the scene");
        }
        instance = this;
    }

    //private void Start()
    //{
        //this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        //this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        //LoadGame();
    //}

    public void OnLoadButtonClick(bool loadButton)
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        if (loadButton)
        {   
            LoadGame();
        } else
        {
            Debug.Log("New Game Pressed");
            this.gameData = dataHandler.Load();
            NewGame();
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
            newGame.Raise(true);
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        
    }

    public void LoadGame()
    {
        bool isNew = false;
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data to load");
            NewGame();
            isNew = true;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        
        if (isNew)
        {
            newGame.Raise(true);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    //private void OnApplicationQuit()
    //{
        //SaveGame();
    //}

    public void OnSaveButtonClick(bool saveButton)
    {
        if (saveButton)
        {
            SaveGame();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
