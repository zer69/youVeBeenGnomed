using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]

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
        if (loadButton)
        {
            this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        } else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data to load");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
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
