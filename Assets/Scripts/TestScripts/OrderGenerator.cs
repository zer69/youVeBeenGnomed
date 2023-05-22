using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class OrderGenerator
{
    // Random rnd = new Random();

    public OrderGenerator()
    {
    }

    public Order buildOrder(int playerLvl)
    {
        Order order = new Order();

        order.orderComplexity = genComplexity(playerLvl);

        order.weaponType = genWeaponType(playerLvl);
        order.oreType = genOreType(playerLvl);
        order.oreQuality = genOreQuality(playerLvl, order.oreType);

        order.reputation = genReputation(playerLvl);
        order.price = genPrice(playerLvl);

        int[] states = { 1, 2, 3, 4 };
        for (int i = states.Length - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = states[j];
            states[j] = states[i];
            states[i] = tmp;
        }

        for (int i = 0; i < order.orderComplexity - 1; i++)
        {
            generateStates(order, playerLvl, states[i]);
        }
        //order.RequiredFragility = genFragility(playerLvl);
        //order.RequiredSharpness = genSharpness(playerLvl);
        //order.RequiredStrength = genStrength(playerLvl);
        //order.Enchantment = genEnchantmentRune(playerLvl);
        //order.EnchantmentLevel = genEnchantmentLevel(playerLvl);

        return order;
    }

    public Order generateStates(Order order, int playerLvl, int state)
    {
        switch (state)
        {
            case 1:
                order.requiredFragility = genFragility(playerLvl);
                break;
            case 2:
                order.requiredSharpness = genSharpness(playerLvl);
                break;
            case 3:
                order.requiredStrength = genStrength(playerLvl);
                break;
            case 4:
                order.enchantment = genEnchantmentRune(playerLvl);
                order.enchantmentLevel = genEnchantmentLevel(playerLvl);
                break;
        }
        return order;
    }

    public List<Order> generateOrdersToOneDay(int playerLvl, int days)
    {
        List<Order> orders = new List<Order>();
        for (int i = 0; i < days; i++)
        {
            orders.Add(buildOrder(playerLvl));
        }
        return orders;
    }
    public Ingot.WeaponType genWeaponType(int playerLvl)
    {
        Ingot.WeaponType weaponType = Ingot.WeaponType.None;

        switch (playerLvl)
        {
            case 1:
                weaponType = (Ingot.WeaponType)Random.Range(1, 3);
                break;
            case 2:
            case 3:
                weaponType = (Ingot.WeaponType)Random.Range(1, 4);
                break;
            case 4:
            case 5:
            case 6:
            case 7:
                weaponType = (Ingot.WeaponType)Random.Range(1, 5);
                break;

        }
        return weaponType;
    }

    public int genComplexity(int playerLvl)
    {
        int[] p2 = { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2 };
        int[] p3 = { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3 };
        int[] p4 = { 2, 2, 2, 2, 2, 2, 3, 3, 3, 3 };
        int[] p5 = { 2, 2, 2, 3, 3, 3, 3, 4, 4, 4 };
        int[] p6 = { 2, 2, 3, 3, 3, 3, 4, 4, 4, 4 };
        int[] p7 = { 2, 3, 3, 3, 4, 4, 4, 5, 5, 5 };

        int complexity = 0;
        switch (playerLvl)
        {
            case 1:
                complexity = 1;
                break;
            case 2:
                complexity = p2[Random.Range(0, 10)];
                break;
            case 3:
                complexity = p3[Random.Range(0, 10)];
                break;
            case 4:
                complexity = p4[Random.Range(0, 10)];
                break;
            case 5:
                complexity = p5[Random.Range(0, 10)];
                break;
            case 6:
                complexity = p6[Random.Range(0, 10)];
                break;
            case 7:
                complexity = p7[Random.Range(0, 10)];
                break;

        }
        return complexity;
    }

    public Ingot.OreType genOreType(int playerLvl)
    {
        int oreType = 0;
        int[] p1 = { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 };
        int[] p2 = { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 };
        int[] p3 = { 0, 0, 0, 1, 1, 1, 1, 1, 1, 2 };
        int[] p4 = { 0, 0, 0, 1, 1, 1, 1, 1, 2, 2 };
        int[] p5 = { 0, 0, 1, 1, 1, 1, 1, 2, 2, 2 };
        int[] p6 = { 0, 0, 1, 1, 1, 1, 2, 2, 2, 2 };
        int[] p7 = { 0, 0, 1, 1, 1, 1, 2, 2, 2, 2 };
        switch (playerLvl)
        {
            case 1:
                oreType = p1[Random.Range(0, 10)]; ;
                break;
            case 2:
                oreType = p2[Random.Range(0, 10)];
                break;
            case 3:
                oreType = p3[Random.Range(0, 10)];
                break;
            case 4:
                oreType = p4[Random.Range(0, 10)];
                break;
            case 5:
                oreType = p5[Random.Range(0, 10)];
                break;
            case 6:
                oreType = p6[Random.Range(0, 10)];
                break;
            case 7:
                oreType = p7[Random.Range(0, 10)];
                break;

        }
        return (Ingot.OreType)oreType;
    }

    public int genSharpness(int playerLvl)
    {
        int[] sharpnessP = { 50, 55, 60, 65, 70, 80, 90 };
        int deviation = 6;
        int sharpness = generateWithDeviation(sharpnessP[playerLvl - 1], deviation);

        return sharpness;
    }

    public int genStrength(int playerLvl)
    {
        int[] strengthP = { 30, 40, 50, 60, 65, 70, 80 };
        int deviation = 6;
        int strength = generateWithDeviation(strengthP[playerLvl - 1], deviation);

        return strength;
    }

    public int genFragility(int playerLvl)
    {
        int[] fragilityP = { 90, 80, 70, 75, 60, 65, 50 };
        int deviation = 6;
        int fragility = generateWithDeviation(fragilityP[playerLvl - 1], deviation);

        return fragility;
    }

    public int generateWithDeviation(int start, int dev)
    {
        return Random.Range(start - dev, start + dev);
    }

    public int genEnchantmentLevel(int playerLvl)
    {
        int lvl = 0;
        int[] p3 = { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2 };
        int[] p4 = { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 };
        int[] p5 = { 1, 1, 1, 1, 2, 2, 2, 2, 2, 2 };
        int[] p6 = { 1, 1, 1, 2, 2, 2, 2, 3, 3, 3 };
        int[] p7 = { 1, 1, 2, 2, 2, 2, 3, 3, 3, 3 };

        switch (playerLvl)
        {
            case 1:
            case 2:
                lvl = 1;
                break;
            case 3:
                lvl = p3[Random.Range(0, 10)];
                break;
            case 4:
                lvl = p4[Random.Range(0, 10)];
                break;
            case 5:
                lvl = p5[Random.Range(0, 10)];
                break;
            case 6:
                lvl = p6[Random.Range(0, 10)];
                break;
            case 7:
                lvl = p7[Random.Range(0, 10)];
                break;

        }

        return lvl;
    }

    public Ingot.Enchantment genEnchantmentRune(int playerLvl)
    {
        int rune = 0;
        switch (playerLvl)
        {
            case 1:
                rune = 2;
                break;
            case 2:
                rune = Random.Range(2, 4);
                break;
            case 3:
                rune = Random.Range(2, 5);
                break;
            case 4:
                rune = Random.Range(1, 5);
                break;
            case 5:
            case 6:
                rune = Random.Range(1, 7);
                break;
            case 7:
                rune = Random.Range(1, 9);
                break;

        }
        return (Ingot.Enchantment)rune;
    }

    public Ingot.OreQuality genOreQuality(int playerLvl, Ingot.OreType oreType)
    {
        int oreQuality = 0;
        //metal type 1
        int[] p1_1 = { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1 };
        int[] p1_2 = { 0, 0, 1, 1, 1, 1, 1, 1, 2, 2 };
        int[] p1_3 = { 1, 1, 1, 2, 2, 2, 2, 2, 2, 3 };
        int[] p1_4 = { 0, 0, 1, 1, 1, 1, 1, 1, 2, 2 };
        //metal type 2
        int[] p2_1 = { 0, 1, 1, 1, 1, 1, 1, 2, 2, 2 };
        int[] p2_2 = { 1, 1, 2, 2, 2, 2, 2, 2, 3, 3 };
        int[] p2_3 = { 2, 2, 2, 3, 3, 3, 3, 3, 3, 4 };
        int[] p2_4 = { 2, 2, 3, 3, 3, 3, 3, 3, 4, 4 };
        //metal type 3
        int[] p3_2 = { 2, 2, 2, 2, 2, 3, 3, 3, 4, 4 };
        int[] p3_3 = { 2, 2, 2, 3, 3, 3, 3, 3, 4, 4 };
        int[] p3_4 = { 3, 3, 3, 3, 3, 4, 4, 4, 4, 5 };
        switch (oreType)
        {
            case Ingot.OreType.Copper:
                switch (playerLvl)
                {
                    case 1:
                    case 2:
                        oreQuality = p1_1[Random.Range(0, 10)];
                        break;
                    case 3:
                    case 4:
                        oreQuality = p1_2[Random.Range(0, 10)];
                        break;
                    case 5:
                    case 6:
                        oreQuality = p1_3[Random.Range(0, 10)];
                        break;
                    case 7:
                        oreQuality = p1_4[Random.Range(0, 10)];
                        break;
                }
                break;

            case Ingot.OreType.Iron:
                switch (playerLvl)
                {
                    case 1:
                    case 2:
                        oreQuality = p2_1[Random.Range(0, 10)];
                        break;
                    case 3:
                    case 4:
                        oreQuality = p2_2[Random.Range(0, 10)];
                        break;
                    case 5:
                    case 6:
                        oreQuality = p2_3[Random.Range(0, 10)];
                        break;
                    case 7:
                        oreQuality = p2_4[Random.Range(0, 10)];
                        break;
                }
                break;

            case Ingot.OreType.Silver:
                switch (playerLvl)
                {
                    case 3:
                    case 4:
                        oreQuality = p3_2[Random.Range(0, 10)];
                        break;
                    case 5:
                    case 6:
                        oreQuality = p3_3[Random.Range(0, 10)];
                        break;
                    case 7:
                        oreQuality = p3_4[Random.Range(0, 10)];
                        break;
                }
                break;
        }
        return (Ingot.OreQuality)oreQuality;
    }

    public int genReputation(int playerLvl)
    {
        int baseReputation = 10;
        float[] multipliersList = new float[7] { 1, 1.2f, 1.5f, 1.9f, 2.4f, 3, 3.7f };
        int range = 4;

        float reputation = Random.Range(baseReputation * multipliersList[playerLvl - 1] - range, baseReputation * multipliersList[playerLvl - 1] + range);
        return Mathf.RoundToInt(reputation);
    }

    public int genPrice(int playerLvl)
    {
        int basePrice = 100;
        float[] multipliersList = new float[7] { 1, 1.2f, 1.5f, 1.9f, 2.4f, 3, 3.7f };
        int range = 40;

        float price = Random.Range(basePrice * multipliersList[playerLvl - 1] - range, basePrice * multipliersList[playerLvl - 1] + range);
        return Mathf.RoundToInt(price);
    }
}


