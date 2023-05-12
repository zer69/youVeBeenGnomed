using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private List<GameObject> ironWeapons;
    [SerializeField] private List<GameObject> copperWeapons;
    [SerializeField] private List<GameObject> silverWeapons;

    [SerializeField] private List<GameObject> ironWeaponComponents;
    [SerializeField] private List<GameObject> copperWeaponComponents;
    [SerializeField] private List<GameObject> silverWeaponComponents;



    public void UpdateWeaponMesh(GameObject ingot)
    {
        switch (ingot.GetComponent<Ingot>().oreType)
        {
            case Ingot.OreType.Iron:
                SetWeaponMesh(ingot, ironWeapons, ironWeaponComponents);
                ingot.GetComponent<Ingot>().setComponentsActive(false);
                break;
            case Ingot.OreType.Copper:
                SetWeaponMesh(ingot, copperWeapons, copperWeaponComponents);
                ingot.GetComponent<Ingot>().setComponentsActive(false);
                break;
            case Ingot.OreType.Silver:
                SetWeaponMesh(ingot, silverWeapons, silverWeaponComponents);
                ingot.GetComponent<Ingot>().setComponentsActive(false);
                break;
        }


    }

    private void SetWeaponMesh(GameObject ingot, List<GameObject> weaponList, List<GameObject> weaponCoponents)
    {
        switch (ingot.GetComponent<Ingot>().weaponType)
        {
            case Ingot.WeaponType.Axe:
                int rnd;
                rnd = Random.Range(0, 2);
                GameObject Axe = Instantiate(weaponList[0], ingot.transform.Find("AxePos"));
                Axe.transform.localPosition = Vector3.zero;
                //Axe.transform.rotation = ingot.transform.rotation;

                SetWeaponComponents(ingot, rnd, weaponCoponents);
                break;
            case Ingot.WeaponType.Sword:
                rnd = Random.Range(2, 6);
                GameObject Sword = Instantiate(weaponList[rnd], ingot.transform.Find("BladePos"));
                Sword.transform.localPosition = Vector3.zero;
                //Sword.transform.rotation = ingot.transform.rotation;

                SetWeaponComponents(ingot, rnd, weaponCoponents);
                break;
            case Ingot.WeaponType.Dagger:
                rnd = 6;
                GameObject Dagger = Instantiate(weaponList[rnd], ingot.transform.Find("DaggerPos"));
                Dagger.transform.localPosition = Vector3.zero;
                //Dagger.transform.rotation = ingot.transform.rotation;

                SetWeaponComponents(ingot, rnd, weaponCoponents);
                break;
            case Ingot.WeaponType.Spear:
                rnd = Random.Range(7, 10);
                GameObject Spear = Instantiate(weaponList[rnd], ingot.transform.Find("SpearPos"));
                //Spear.transform.localPosition = Vector3.zero;
                //Spear.transform.rotation = ingot.transform.rotation;

                SetWeaponComponents(ingot, rnd, weaponCoponents);
                break;
        }
    }


    private void SetWeaponComponents(GameObject ingot, int weaponBlade, List<GameObject> weaponCoponents)
    {
        switch (weaponBlade)
        {
            case 0://done
                GameObject Shaft_01 = Instantiate(weaponCoponents[0], ingot.transform.Find("AxePos").Find("ShaftPos1"));

                Shaft_01.transform.localPosition = Vector3.zero;
                Shaft_01.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Debug.Log("case 0");
                
                break;

            case 1://done
                GameObject Shaft_02 = Instantiate(weaponCoponents[1], ingot.transform.Find("AxePos").Find("ShaftPos2"));
                Shaft_02.transform.localPosition = Vector3.zero;
                Shaft_02.transform.localRotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("case 1");
                break;
            case 2://done
                GameObject Garda_01 = Instantiate(weaponCoponents[2], ingot.transform.Find("BladePos").Find("GardaPos"));
                GameObject Hilt_01 = Instantiate(weaponCoponents[3], ingot.transform.Find("BladePos").Find("HiltPos1"));

                Hilt_01.transform.localPosition = Vector3.zero;
                Garda_01.transform.localPosition = Vector3.zero;
                Garda_01.transform.localRotation = Quaternion.Euler(0, 0, 0);
                                
                Debug.Log("case 2");

                break;
            case 3://done
            case 4://done
                GameObject Garda_02 = Instantiate(weaponCoponents[4], ingot.transform.Find("BladePos").Find("GardaPos"));
                GameObject Hilt_02 = Instantiate(weaponCoponents[5], ingot.transform.Find("BladePos").Find("HiltPos2"));

                Garda_02.transform.localPosition = Vector3.zero;
                Garda_02.transform.localRotation = Quaternion.Euler(0, 0, 0); 

                Hilt_02.transform.localPosition = Vector3.zero;

                Debug.Log("case 3-4");
                break;
            case 5://done
                GameObject Garda_03 = Instantiate(weaponCoponents[6], ingot.transform.Find("BladePos").Find("GardaPos"));
                GameObject Hilt_03 = Instantiate(weaponCoponents[7], ingot.transform.Find("BladePos").Find("HiltPos1"));

                Garda_03.transform.localPosition = Vector3.zero;
                Garda_03.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Hilt_03.transform.localPosition = Vector3.zero;
                Debug.Log("case 5");
                break;
            case 6://done
                GameObject Garda_04 = Instantiate(weaponCoponents[2], ingot.transform.Find("DaggerPos").Find("GardaPos"));
                GameObject Hilt_04 = Instantiate(weaponCoponents[3], ingot.transform.Find("DaggerPos").Find("HiltPos"));

                Garda_04.transform.localPosition = Vector3.zero;
                Garda_04.transform.localRotation = Quaternion.Euler(0, 0, 0);

                Hilt_04.transform.localPosition = Vector3.zero;
                Debug.Log("case 6");
                break;
            case 7://done
            case 8://done 
            case 9://done
                GameObject Shaft_03 = Instantiate(weaponCoponents[8], ingot.transform.Find("SpearPos").Find("ShaftPos"));
                Shaft_03.transform.localPosition = Vector3.zero;
                Shaft_03.transform.localRotation = Quaternion.Euler(0, 0, 0);
                Shaft_03.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Debug.Log("case 7-9");
                break;
        }
    }


}
