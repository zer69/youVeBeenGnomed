using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderReciever : MonoBehaviour
{
    private Transform orderImage;

    private Transform orderName;

    private Transform money;
    private float moneyStat;

    private Transform reputation;
    private float reputationStat;

    private Transform quality;
    private Transform qualityImage;
    private Transform materialName;
    private Transform qualityValue;
    [SerializeField] private List<GameObject> materialSpriteList;


    private Transform fragility;
    private Transform fragilityValue;
    private Transform fragilityThreshhold;
    private int fragilityStat;
    
    private Transform sharpness;
    private Transform sharpnessValue;
    private Transform sharpnessThreshhold;
    private int sharpnessStat;


    private Transform sturdiness;
    private Transform sturdinessValue;
    private Transform sturdinessThreshhold;
    private int sturdinessStat;


    private Transform enchantment;
    private Transform enchantmentName;
    private string enchantmentStat;

    private void Start()
    {
        orderImage = transform.Find("Image");
        orderName = transform.Find("Name");
        money = transform.Find("Price");
        reputation = transform.Find("Reputation");

        quality = transform.Find("Stats").Find("Quality");
        qualityImage = quality.Find("Image");
        materialName = quality.Find("Name");
        qualityValue = quality.Find("QualityValue");

        fragility = transform.Find("Stats").Find("Fragility");
        fragilityValue = fragility.Find("Value");
        fragilityThreshhold = fragility.Find("ProgressBarFrame").Find("Threshhold");


        sharpness = transform.Find("Stats").Find("Sharpness");
        sharpnessValue = sharpness.Find("Value");
        sharpnessThreshhold = sharpness.Find("ProgressBarFrame").Find("Threshhold");

        sturdiness = transform.Find("Stats").Find("Sturdiness");
        sturdinessValue = sturdiness.Find("Value");
        sturdinessThreshhold = sturdiness.Find("ProgressBarFrame").Find("Threshhold");

        enchantment = transform.Find("Stats").Find("Enchantment");
        enchantmentName = enchantment.Find("Name");

    }

    public void ReadOrderStats(Transform order)
    {
        int fragility = 30;
        int sharpness = 50;
        int sturdiness = 45;
        int mone = 0;
        SetImage();
        SetName();
        SetPayBack("money", mone);
        SetPayBack("reputation", mone);



        SetMaterial();
        SetStatProgressBar("fragility", fragility);
        SetStatProgressBar("sharpness", sharpness);
        SetStatProgressBar("sturdiness", sturdiness);
        SetEnchantment();

    }

    private void SetImage()//probably value here
    {

    }

    private void SetName()//probably value here
    {

    }

    private void SetPayBack(string stat, float value)
    {
        Transform tmpPayBack = null;
        switch (stat)
        {
            case "money":
                tmpPayBack = money;
                moneyStat = value;
                break;
            case "reputation":
                tmpPayBack = reputation;
                reputationStat = value;
                break;
        }
        tmpPayBack.GetComponent<TMP_Text>().text = value.ToString();
    }

    private void SetMaterial()//value here
    {
        throw new NotImplementedException();
    }

    private void SetStatProgressBar(string stat, int value)
    {
        Transform tmpValue = null;
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
        tmpThreshHold.localPosition = new Vector3(value, 0f, 0f);
    }

    private void SetEnchantment()//value here
    {

    }

}
