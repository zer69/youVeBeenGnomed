using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ReputationTmp : MonoBehaviour
{
    public float reputation = 0;
    public float maxRep = 200;

    public TMP_Text rep;

    private void Update()
    {
        rep.text = reputation.ToString();
    }
}
