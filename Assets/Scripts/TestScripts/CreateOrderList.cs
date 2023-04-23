using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateOrderList
{
    [MenuItem("Assets/Create/Order List")]
    public static OrderList Create()
    {
        OrderList asset = ScriptableObject.CreateInstance<OrderList>();

        AssetDatabase.CreateAsset(asset, "Assets/OrderList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}