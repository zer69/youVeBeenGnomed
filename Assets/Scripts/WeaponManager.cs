using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> ironWeapons;
    [SerializeField] private List<GameObject> copperWeapons;
    [SerializeField] private List<GameObject> silverWeapons;





    public void UpdateWeaponMesh(GameObject ingot)
    {
        switch (ingot.GetComponent<Ingot>().oreType)
        {
            case Ingot.OreType.Iron:
                SetWeaponMesh(ingot, ironWeapons);
                break;
            case Ingot.OreType.Copper:
                SetWeaponMesh(ingot, copperWeapons);
                break;
            case Ingot.OreType.Silver:
                SetWeaponMesh(ingot, silverWeapons);
                break;
        }


    }

    private void SetWeaponMesh(GameObject ingot, List<GameObject> weaponList)
    {
        switch (ingot.GetComponent<Ingot>().weaponType)
        {
            case Ingot.WeaponType.Axe:
                GameObject Axe = Instantiate(weaponList[Random.Range(0, 2)], ingot.transform.Find("AxePos"));
                Axe.transform.localPosition = Vector3.zero;
                //Axe.transform.rotation = ingot.transform.rotation;
                break;
            case Ingot.WeaponType.Sword:
                GameObject Sword = Instantiate(weaponList[Random.Range(2, 6)], ingot.transform.Find("BladePos"));
                Sword.transform.localPosition = Vector3.zero;
                //Sword.transform.rotation = ingot.transform.rotation;
                break;
            case Ingot.WeaponType.Dagger:
                GameObject Dagger = Instantiate(weaponList[6], ingot.transform.Find("DaggerPos"));
                Dagger.transform.localPosition = Vector3.zero;
                //Dagger.transform.rotation = ingot.transform.rotation;
                break;
            case Ingot.WeaponType.Spear:
                GameObject Spear = Instantiate(weaponList[Random.Range(7, 10)], ingot.transform.Find("SpearPos"));
                Spear.transform.localPosition = Vector3.zero;
                //Spear.transform.rotation = ingot.transform.rotation;
                break;
        }
    }




    
}
