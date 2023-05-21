using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StorageAmounts : MonoBehaviour
{
    [SerializeField] private List<Transform> storages;
    [SerializeField] private List<int> amounts;

    private void Update()
    {
        ReadAmounts();
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

    }

    private void SetStorageAmounts()
    {
        
        foreach (Transform child in transform)
        {
            child.GetChild(2).GetComponent<TMP_Text>().text = amounts[child.GetSiblingIndex()].ToString();
        }
    }
    

}