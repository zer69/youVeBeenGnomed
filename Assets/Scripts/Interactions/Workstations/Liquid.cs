using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{

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

    private void CoolingWeapon(GameObject ingot)
    {
        //check that the change of fragility and strength values occurs only when the ingot is cooling 
        if (ingot.gameObject.GetComponent<Ingot>().Cooling(CoolingRate))
        {
            ingot.gameObject.GetComponent<Ingot>().Cooling(CoolingRate);
            ingot.gameObject.GetComponent<Ingot>().FragilityModification(FragilityRate);
            ingot.gameObject.GetComponent<Ingot>().StrengthModification(StrengthRate);
        }

    }


    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Ingot")
        {
            Debug.Log("Stay");
            CoolingWeapon(collider.gameObject);
        }
    }
}
