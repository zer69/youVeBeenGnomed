using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ingot : MonoBehaviour
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare, 
        Epic, 
        Legendary
    };

    public enum OreType
    {
        Iron,
        Copper,
        Silver
    };

    public enum Enchantment
    {
        None,
        Fire,
        Water,
        Earth,
        Air,
        Light,
        Dark,
        Poison,
        Lightning
    };

    public enum CompletionStatus
    {
        Raw, // status for furnace
        Melted, // status after furnace, anvil next

        Forged, // done on anvil, new base status, next is heated/cooled
        
        Heated, // heat on furnace after forging
        Cooled, // cooled after heating

        Sharpened, // status after whetstone
        Completed // status after workbench

    };

    public enum WeaponType
    {
        None,
        Axe,
        Sword,
        Dagger,
        Spear
    }

    public enum AnvilState
    {
        Raw,
        Rare,
        MediumRare,
        WellDone,
        Weapon
    }

    public enum OreQuality
    {
        Poor,
        Medium,
        Normal,
        Good,
        Exellent,
        Legendary
    };

    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] public CompletionStatus status;
    [SerializeField] public AnvilState anvilState;
    [SerializeField] public WeaponType weaponType;


    [SerializeField] public Rarity rarity;
    [SerializeField] public OreType oreType;
    


    [Header("Air Temperature")]
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] public float airTemperature = 24;
    [SerializeField] public float airCoolingRate = 0.09f;

    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [Header("Ignot Properties")]
    [Header("Quality")]
    [SerializeField] private float quality;

    [Header("Temperature")]
    [SerializeField] private float MeltingPoint;

    [SerializeField] public float coolingRate;
    [SerializeField] public float currentTemperature;
    [SerializeField] private Vector3 targetGlow;
    private Color targetGlowColor;
    private float glowRate;

    //minTemperatureValue = airTemperature
    //[SerializeField] private float maxTemperatureValue = 1200;
    [Header("Structure")]
    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] public float strength;
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float minStrengthValue = 0;
    [SerializeField] private float maxStrengthValue = 100;
    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] public float fragility;
    [BackgroundColor(0f, 1.5f, 0f, 1f)]
    [SerializeField] private float minFragilityValue = 0;
    [SerializeField] private float maxFragilityValue = 100;

    [Header("Sharpness")]
    [SerializeField] public float sharpness;

    [BackgroundColor(1.5f, 1.5f, 0f, 1f)]
    [SerializeField] bool isEnchanted;
    [SerializeField] private Enchantment enchantment;
    [SerializeField] private int enchantmentQuality;
    public int EnchantmentQuality
    {
        get => enchantmentQuality;
        set
        {
            if (enchantmentQuality != 0)
            {
                enchantmentQuality = (value + enchantmentQuality) / 2;
            }
            else
            {
                enchantmentQuality = value;
            }
        }
    }

    [Header("No Edit")]
    [BackgroundColor(1.5f, 0f, 0f, 1f)]
    [SerializeField] private GameObject state1;
    [SerializeField] private GameObject state2;

    [SerializeField] private Material GlowMaterial;


    [Header("Events")]
    [BackgroundColor(0.75f, 0f, 1.5f, 1f)]

    [SerializeField] private go_GameEvent sendIngot;
    [SerializeField] private s_GameEvent hint;
    [SerializeField] private col_GameEvent weaponLanded;
    [SerializeField] private col_GameEvent weaponPicked;

    [SerializeField] private TextMeshPro temperatureText;
    [SerializeField] private TextMeshPro sharpnessText;
    [SerializeField] private TextMeshPro fragilityText;
    [SerializeField] private TextMeshPro strengthText;
    [SerializeField] private TextMeshPro enchantmentText;


    private Transform axePos;
    private Transform bladePos;
    private Transform spearPos;
    private Transform daggerPos;

    bool readyRaised = true;
    bool weaponMaterialSet = false;

    private float price; // calculated based on rarity, type, quality, strength, fragility, sharpness and enchantment
    private void Start()
       
    {
        GlowMaterial = GetComponent<MeshRenderer>().material;
        

        targetGlowColor = new Color(targetGlow.x, targetGlow.y, targetGlow.z);

        //ingotMaterial = this.gameObject.GetComponent<Material>();
        coolingRate = airCoolingRate;
        //currentTemperature = airTemperature;

        isEnchanted = false;
        enchantment = Enchantment.None;
        EnchantmentQuality = 0;

        axePos = transform.Find("AxePos");
        bladePos = transform.Find("BladePos");
        spearPos = transform.Find("SpearPos");
        daggerPos = transform.Find("DaggerPos");
    }
    private void Update()
    {
        UpdateMat();
        Cooling();
        if (anvilState != AnvilState.Weapon)
            UpdateGraphics();
        
        InfoUpdate();
       
    }

    private void UpdateMat()
    {
        glowRate = Mathf.Clamp(currentTemperature-airTemperature - MeltingPoint/3f, 0f, MeltingPoint * 1.5f) / MeltingPoint;

        GlowMaterial.SetColor("_EmissionColor", targetGlowColor * glowRate);
    }

    public void UpdateGraphics()
    {
        switch (anvilState)
        {
            case AnvilState.Rare:
                GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(true);
                ResetMaterial(transform.GetChild(0));
                break;
            case AnvilState.MediumRare:
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                ResetMaterial(transform.GetChild(1));
                break;
            case AnvilState.WellDone:
                anvilState = AnvilState.Weapon;
                transform.GetChild(1).gameObject.SetActive(false);
                UpdateWeaponGraphics();
                break;
            
        }
    }

    

    public void UpdateWeaponGraphics()
    {
        //Debug.Log("SENTWEAPON");
        sendIngot.Raise(this.gameObject);
        
        


    }

    public void SetWeaponMaterial()
    {
        //Debug.Log(transform.gameObject.name);
        switch (weaponType)
        {
            case WeaponType.Axe:
                if (axePos.GetChild(axePos.childCount-1).gameObject.tag == "axe1")
                {
                    GetRootMaterial(axePos, axePos.childCount - 1);
                    SetRootMaterial(axePos.GetChild(axePos.childCount - 1));
                }
                else
                    ResetMaterial(axePos.GetChild(axePos.childCount - 1));


                break;
            case WeaponType.Sword:
                ResetMaterial(bladePos.GetChild(bladePos.childCount - 1));
                break;
            case WeaponType.Spear:
                GetRootMaterial(spearPos, spearPos.childCount - 1);
                SetRootMaterial(spearPos.GetChild(spearPos.childCount - 1));
                break;
            case WeaponType.Dagger:
                ResetMaterial(daggerPos.GetChild(daggerPos.childCount - 1));
                break;
        }
    }

    private void ResetMaterial(Transform obj)
    {
        GlowMaterial = obj.GetComponent<MeshRenderer>().material;
        GlowMaterial.EnableKeyword("_EMISSION");
    }

    private void GetRootMaterial(Transform obj, int child)
    {
        GlowMaterial = obj.GetChild(child).GetChild(0).GetComponent<MeshRenderer>().material;
        GlowMaterial.EnableKeyword("_EMISSION");

    }

    private void SetRootMaterial(Transform obj)
    {
        foreach (Transform child in obj)
        {
            child.GetComponent<MeshRenderer>().material = GlowMaterial;
        }
    }

    //method for ingot temperature reduction, rate - how much does the temperature change
    public bool Cooling()
    {        
        if (currentTemperature - coolingRate >= airTemperature)
        {
            currentTemperature -= coolingRate;
            //temperatureText.text = "<sprite=2> " + currentTemperature.ToString("F2") + " *C";
            //Debug.Log("currentTemperature: " + currentTemperature);

            return true;
        }

        
        if (status == CompletionStatus.Heated)
        {
            status = CompletionStatus.Cooled;
        }

        return false;
    }

    public bool Heating(float furnaceTemperature, float smeltingSpeed)
    {
        if (currentTemperature < furnaceTemperature)
        {
            currentTemperature += smeltingSpeed * Time.deltaTime;
            //temperatureText.text = "<sprite=2> " + currentTemperature.ToString("F2") + " *C";
            Melting();
            //Debug.Log("Current temperature of ingot is " + ingotTemperature + "*C");

            if (status >= CompletionStatus.Forged)
            {
                status = CompletionStatus.Heated;
            }
            return true;
        }

        return false;
    }

    public void StrengthModification(float rate)
    {
        //to decrease the value, a negative value must be passed
        if (strength + rate > minStrengthValue && strength + rate < maxStrengthValue)
        {
            strength += rate;
        }
    }

   
    public void FragilityModification(float rate)
    {
        //to decrease the value, a negative value must be passed
        if (fragility + rate > minFragilityValue && strength + rate < maxFragilityValue)
        {
            fragility += rate;
        }
    }

    public void setNormalCoolingRate()
    {
        coolingRate = airCoolingRate;
        Debug.Log("coolingRate: " + airCoolingRate);
    }

    public void setZeroCoolingRate()
    {
        coolingRate = 0;
        Debug.Log("coolingRate: ZeroCoolingRate");
    }

    public void setSpecificCoolingRate(float rate)
    {
        coolingRate = rate;
        Debug.Log("coolingRate: " + rate);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TeleportPlatform")
            weaponLanded.Raise(this.GetComponent<BoxCollider>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TeleportPlatform")
            weaponPicked.Raise(this.GetComponent<BoxCollider>());
    }

    public void setEnchantment(int enchantmentId, int quality)
    {        
        isEnchanted = true;
        enchantment = (Enchantment)Enum.GetValues(typeof(Enchantment)).GetValue(enchantmentId); ;
        EnchantmentQuality = quality;
    }

    public void setEnchantment(Enchantment enchantment, int quality)
    {
        isEnchanted = true;
        this.enchantment = enchantment;
        EnchantmentQuality = quality;
    }

    public Enchantment getEnchantment()
    {
        return enchantment;
    }

    public void Melting()
    {
        if (anvilState == AnvilState.Weapon)
            return;
        if (currentTemperature >= MeltingPoint)
        {
            status = CompletionStatus.Melted;
            if (readyRaised)
            {
                hint.Raise("Ingot ready for forging!");
                readyRaised = false;
            }
        }
        else
        {
            status = CompletionStatus.Raw;
        }
    }

    public ValueTuple<CompletionStatus, WeaponType, Rarity, OreType> getData()
    {
        return (status, weaponType, rarity, oreType);
    }

    public void setData(ValueTuple<CompletionStatus, WeaponType, Rarity, OreType> data)
    {
        (status, weaponType, rarity, oreType) = data;
    }

    void InfoUpdate()
    {
        /*temperatureText.text = "<sprite=2> " + currentTemperature.ToString("F2");
        sharpnessText.text = "<sprite=0> " + sharpness.ToString("F2");
        fragilityText.text = "<sprite=1> " + fragility.ToString("F2");
        strengthText.text = "<sprite=4> " + strength.ToString("F2");
        enchantmentText.text = "<sprite=2> " + enchantment.ToString("F2");*/
    }

    public void SwitchGlassesLayer(int layer)
    {
        switch (layer)
        {
            case 1:
                temperatureText.gameObject.SetActive(true);
                sharpnessText.gameObject.SetActive(false);
                enchantmentText.gameObject.SetActive(false);
                fragilityText.gameObject.SetActive(false);
                strengthText.gameObject.SetActive(false);
                break;

            case 2:
                fragilityText.gameObject.SetActive(true);
                sharpnessText.gameObject.SetActive(false);
                temperatureText.gameObject.SetActive(false);
                temperatureText.gameObject.SetActive(false);                
                strengthText.gameObject.SetActive(false);
                break;

            case 3:
                strengthText.gameObject.SetActive(true);
                sharpnessText.gameObject.SetActive(false);
                temperatureText.gameObject.SetActive(false);
                temperatureText.gameObject.SetActive(false);
                fragilityText.gameObject.SetActive(false);                
                break;

            case 4:
                sharpnessText.gameObject.SetActive(true);
                temperatureText.gameObject.SetActive(false);
                temperatureText.gameObject.SetActive(false);
                fragilityText.gameObject.SetActive(false);
                strengthText.gameObject.SetActive(false);
                break;

            case 5:
                enchantmentText.gameObject.SetActive(true);
                sharpnessText.gameObject.SetActive(false);
                temperatureText.gameObject.SetActive(false);                
                fragilityText.gameObject.SetActive(false);
                strengthText.gameObject.SetActive(false);
                break;
        }
    }

    public void setComponentsActive(bool activity)
    {
        transform.Find("AxePos").Find("ShaftPos1").gameObject.SetActive(activity);
        transform.Find("AxePos").Find("ShaftPos2").gameObject.SetActive(activity);
        transform.Find("BladePos").Find("GardaPos").gameObject.SetActive(activity);
        transform.Find("BladePos").Find("HiltPos1").gameObject.SetActive(activity);
        transform.Find("BladePos").Find("HiltPos2").gameObject.SetActive(activity);
        transform.Find("DaggerPos").Find("GardaPos").gameObject.SetActive(activity);
        transform.Find("DaggerPos").Find("HiltPos").gameObject.SetActive(activity);
        transform.Find("SpearPos").Find("ShaftPos").gameObject.SetActive(activity);
    }
}
