using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private Material shiverMaterial;
    [SerializeField] private Vector3 baseEmissionValue;
    [Header("Shiver Stats")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float minEmissionValue;
    [SerializeField] private float maxEmissionValue;
    [SerializeField] private float shiverRateSeconds;

    private Color baseEmission;
    private Color currentEmission;
    private Color targetEmission;
    private bool canChangeEmission = true;

    private void Start()
    {
        shiverMaterial.EnableKeyword("_EMISSION");
        baseEmission = new Color(baseEmissionValue.x, baseEmissionValue.y, baseEmissionValue.z, 1f);
        currentEmission = shiverMaterial.GetColor("_EmissionColor");

        //Debug.Log(baseEmission);
        //Debug.Log(currentEmission);
    }

    private void Update()
    {
        if (canChangeEmission)
            ShiverCalc();
        Shiver();

    }

    private void Shiver()
    {
        //Debug.Log(currentEmission);
        currentEmission = Color.Lerp(currentEmission, targetEmission, shiverRateSeconds);
        shiverMaterial.SetColor("_EmissionColor", currentEmission);
    }

    private void ShiverCalc()
    {
        targetEmission = baseEmission * Random.Range(minEmissionValue, maxEmissionValue);
        StartCoroutine(ShiverCooldown());
    }

    IEnumerator ShiverCooldown()
    {
        canChangeEmission = false;
        yield return new WaitForSeconds(1f);
        canChangeEmission = true;
    }
    
}
