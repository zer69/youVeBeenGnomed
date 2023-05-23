using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StorageAmounts : MonoBehaviour, IDataPersistence
{
    [SerializeField] private List<Transform> storages;
    [SerializeField] public List<int> amounts;

    private void Update()
    {
        ReadAmounts();
        if (transform.gameObject.tag != "Finish")
            SetStorageAmounts();
    }

    private void ReadAmounts()
    {
        int i = 0;
        for (; i < 3; i++)
        {
            amounts[i] = storages[i].GetComponent<PileOfIngots>().ingotsInPile;
        }

        for (;i < 4; i++)
        {
            amounts[i] = storages[i].GetComponent<CoalBox>().coalInPile;
        }

        amounts[4] = storages[4].GetComponent<Workbench>().swordGuard;
        amounts[5] = storages[5].GetComponent<Workbench>().swordHilt;
        amounts[6] = storages[6].GetComponent<Workbench>().axeHandle;
        amounts[7] = storages[7].GetComponent<Workbench>().spearHandle;

        amounts[8] = storages[8].GetComponent<PileOfBatteries>().batteriesInPile;
        amounts[9] = storages[9].GetComponent<PileOfBatteries>().batteriesInPile;
        amounts[10] = storages[10].GetComponent<PileOfBatteries>().batteriesInPile;

    }

    private void SetStorageAmounts()
    {
        
        foreach (Transform child in transform)
        {
            child.GetChild(2).GetComponent<TMP_Text>().text = amounts[child.GetSiblingIndex()].ToString();
        }
    }
    
    public void LoadData(GameData data)
    {
        this.amounts = data.amounts;
    }

    public void SaveData(ref GameData data)
    {
        data.amounts = this.amounts;
    }

}
