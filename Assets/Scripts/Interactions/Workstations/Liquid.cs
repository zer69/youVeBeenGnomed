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
        Debug.Log(collider.gameObject.transform.gameObject.name);
        if (!CoolingWeapon(collider.gameObject.transform.gameObject))
        {
            hint.Raise("Oh my, it's cooled! Almost as cold as ice");
        }
        
        //if(collider.tag == "Tool")
        //{            
        //    //Debug.Log("Tool Stay");
        //    CoolingWeapon(collider.gameObject.transform.GetChild(1).gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enter liquid");
        Ingot ignot = other.gameObject.transform.GetChild(1).GetComponent<Ingot>();
        ignot.setSpecificCoolingRate(CoolingRate);
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exit liquid");
        Ingot ignot = other.gameObject.transform.GetChild(1).GetComponent<Ingot>();
        ignot.setNormalCoolingRate();
    }

}
