using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderReciever : MonoBehaviour
{
    private Order activeOrder;

    private Image orderImage;
    [SerializeField] private List<Sprite> weaponSpriteList;

    private TMP_Text orderName;

    private TMP_Text money;
    private float moneyStat;

    private TMP_Text reputation;
    private float reputationStat;

    private Transform quality;
    private Image qualityImage;
    private TMP_Text materialName;
    private TMP_Text qualityValue;
    [SerializeField] private List<Sprite> materialSpriteList;
    private Ingot.OreType materialTypeStat;
    private Ingot.OreQuality materialQualityStat;


    private Transform fragility;
    private TMP_Text fragilityValue;
    private Transform fragilityThreshhold;
    private float fragilityStat;
    
    private Transform sharpness;
    private TMP_Text sharpnessValue;
    private Transform sharpnessThreshhold;
    private float sharpnessStat;


    private Transform sturdiness;
    private TMP_Text sturdinessValue;
    private Transform sturdinessThreshhold;
    private float sturdinessStat;


    private Transform enchantment;
    private TMP_Text enchantmentName;
    private Ingot.Enchantment enchantmentStat;

    private void Start()
    {
        orderImage = transform.Find("Image").GetComponent<Image>();
        orderName = transform.Find("Name").GetComponent<TMP_Text>();
        money = transform.Find("Price").GetComponent<TMP_Text>();
        reputation = transform.Find("Reputation").GetComponent<TMP_Text>();

        quality = transform.Find("Stats").Find("Quality");
        qualityImage = quality.Find("Image").GetComponent<Image>();
        materialName = quality.Find("Name").GetComponent<TMP_Text>();
        qualityValue = quality.Find("QualityValue").GetComponent<TMP_Text>();

        fragility = transform.Find("Stats").Find("Fragility");
        fragilityValue = fragility.Find("Value").GetComponent<TMP_Text>();
        fragilityThreshhold = fragility.Find("ProgressBarFrame").Find("Threshhold");


        sharpness = transform.Find("Stats").Find("Sharpness");
        sharpnessValue = sharpness.Find("Value").GetComponent<TMP_Text>();
        sharpnessThreshhold = sharpness.Find("ProgressBarFrame").Find("Threshhold");

        sturdiness = transform.Find("Stats").Find("Sturdiness");
        sturdinessValue = sturdiness.Find("Value").GetComponent<TMP_Text>();
        sturdinessThreshhold = sturdiness.Find("ProgressBarFrame").Find("Threshhold");

        enchantment = transform.Find("Stats").Find("Enchantment");
        enchantmentName = enchantment.Find("Name").GetComponent<TMP_Text>();

    }

    public void ReadOrderStats(Order order)
    {
        activeOrder = order;
        
        SetImage();
        SetName();

        SetPayBack("money", activeOrder.price);
        SetPayBack("reputation", activeOrder.reputation);



        SetMaterial();

        SetStatProgressBar("fragility", activeOrder.requiredFragility);
        SetStatProgressBar("sharpness", activeOrder.requiredSharpness);
        SetStatProgressBar("sturdiness", activeOrder.requiredStrength);
        
        SetEnchantment();

    }

    private void SetImage()//probably value here
    {
        orderImage.sprite = weaponSpriteList[(int)activeOrder.weaponType - 1];
    }

    private void SetName()//probably value here
    {
        orderName.text = activeOrder.oreType.ToString() + " " + activeOrder.weaponType.ToString();
    }

    private void SetPayBack(string stat, float value)
    {
        switch (stat)
        {
            case "money":
                moneyStat = value;
                money.text = value.ToString();
                break;
            case "reputation":
                reputationStat = value;
                reputation.text = value.ToString();
                break;
        }
    }

    private void SetMaterial()//value here
    {
        qualityImage.sprite = materialSpriteList[(int)activeOrder.oreType];
        qualityValue.text = activeOrder.oreQuality.ToString();
        materialName.text = activeOrder.oreType.ToString();
        materialTypeStat = activeOrder.oreType;
        materialQualityStat = activeOrder.oreQuality;

    }

    private void SetStatProgressBar(string stat, float value)
    {
        TMP_Text tmpValue = null;
        Transform tmpThreshHold = null;
        switch (stat)
        {
            case "fragility":
                tmpValue = fragilityValue;
                tmpThreshHold = fragilityThreshhold;
                fragilityStat = value;
                break;
            case "sharpness":
                tmpValue = sharpnessValue;
                tmpThreshHold = sharpnessThreshhold;
                sharpnessStat = value;
                break;
            case "sturdiness":
                tmpValue = sturdinessValue;
                tmpThreshHold = sturdinessThreshhold;
                sturdinessStat = value;
                break;
        }
        tmpValue.GetComponent<TMP_Text>().text = value.ToString();
        
        tmpThreshHold.localPosition = new Vector3(value -50f, 2f, 0f);
    }

    private void SetEnchantment()//value here
    {
        enchantmentName.text = activeOrder.enchantment.ToString();
        enchantmentStat = activeOrder.enchantment;
    }

}
