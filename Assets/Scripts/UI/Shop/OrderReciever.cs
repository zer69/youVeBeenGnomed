using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderReciever : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private Transform orderCover;

    [SerializeField] private Transform orderContent;

    private Order activeOrder;
    private Order orderInTeleport; // send with ingot to manager

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
    private Ingot.WeaponType weaponTypeStat;
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

    [SerializeField]private List<RectTransform> progress = new List<RectTransform>();
    [SerializeField]private List<float> progressStat;

    [SerializeField] private Button sendOrderButton;

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
        progress[0] = (fragility.Find("ProgressBarFrame").Find("Progress").GetComponent<RectTransform>());



        sharpness = transform.Find("Stats").Find("Sharpness");
        sharpnessValue = sharpness.Find("Value").GetComponent<TMP_Text>();
        sharpnessThreshhold = sharpness.Find("ProgressBarFrame").Find("Threshhold");
        progress[1] = (sharpness.Find("ProgressBarFrame").Find("Progress").GetComponent<RectTransform>());

        sturdiness = transform.Find("Stats").Find("Sturdiness");
        sturdinessValue = sturdiness.Find("Value").GetComponent<TMP_Text>();
        sturdinessThreshhold = sturdiness.Find("ProgressBarFrame").Find("Threshhold");
        progress[2] = (sturdiness.Find("ProgressBarFrame").Find("Progress").GetComponent<RectTransform>());

        enchantment = transform.Find("Stats").Find("Enchantment");
        enchantmentName = enchantment.Find("Name").GetComponent<TMP_Text>();


        
    }

    private void Update()
    {
        if (orderInTeleport != null)
            CompareOrders();
    }

    private void CompareOrders()
    {

        if (weaponTypeStat != activeOrder.weaponType)
            transform.GetComponent<Image>().color = Color.red;
        else
            transform.GetComponent<Image>().color = Color.white;

        if ((materialTypeStat != activeOrder.oreType) || (materialQualityStat < activeOrder.oreQuality))
            quality.GetComponent<Image>().enabled = true;
        else
            quality.GetComponent<Image>().enabled = false;
    }

    public void ReadOrderStats(Order order)
    {
        orderCover.gameObject.SetActive(false);
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

    public void RecieveOrder(Ingot order)
    {
        if ((order == null) || (order.status < Ingot.CompletionStatus.Completed))
        {
            progressStat[0] = 0;
            progressStat[1] = 0;
            progressStat[2] = 0;
            sendOrderButton.interactable = false;
            orderInTeleport = null;
        }
        else
        {
            progressStat[0] = order.fragility;
            progressStat[1] = order.sharpness;
            progressStat[2] = order.strength;
            sendOrderButton.interactable = true;

            weaponTypeStat = order.weaponType;
            materialTypeStat = order.oreType;
            materialQualityStat = order.quality;

            orderInTeleport = new Order(order);

        }

        if (progress[0] != null)
            for (int i = 0; i < 3; i++)
                progress[i].offsetMax = new Vector2(-(100 - progressStat[i]), progress[i].offsetMax.y);
    }

    private void OnEnable()
    {
        if (transform.childCount > 0)
        {
            
            ReadOrderStats(orderContent.GetChild(0).GetComponent<OrderMono>().order);
        }
            
        else
            orderCover.gameObject.SetActive(true);
    }

    public void SendOrder()
    {
        gameStateManager.DoneOrderCalculations(orderInTeleport, activeOrder);
    }

}
