using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField] private s_GameEvent hint;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float CoolingRate;
    [SerializeField] private float StrengthRate;
    [SerializeField] private float FragilityRate;

    [SerializeField] private Barrel barrel;
    [Header("Sound Events")]
    public AK.Wwise.Event CoolingSoundEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool CoolingWeapon(GameObject ingot)
    {
        //check that the change of fragility and strength values occurs only when the ingot is cooling 
        if (ingot.gameObject.GetComponent<Ingot>().Cooling())
        {
            ingot.gameObject.GetComponent<Ingot>().FragilityModification(FragilityRate);
            ingot.gameObject.GetComponent<Ingot>().StrengthModification(StrengthRate);
            return true;
        }

        return false;
    }


    void OnTriggerStay(Collider collider)
    {
        if (barrel.canControlTongs && collider.tag == "Ingot")
        {
            //Debug.Log(collider.gameObject.transform.gameObject.name);
            if (!CoolingWeapon(collider.gameObject.transform.gameObject))
            {
                hint.Raise("Oh my, it's cooled! Almost as cold as ice");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter liquid");
        if (barrel.canControlTongs && other.tag == "Ingot")
        {
            Ingot ignot = other.gameObject.transform.GetComponent<Ingot>();
            ignot.setSpecificCoolingRate(CoolingRate);
            CoolingSoundEvent.Post(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (barrel.canControlTongs && other.tag == "Ingot")
        {            
            Ingot ignot = other.gameObject.transform.GetComponent<Ingot>();
            ignot.setNormalCoolingRate();
        }
    }

}
