using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class OrderListEditor : EditorWindow
{

    public OrderList orderList;
    private int viewIndex = 1;

    [MenuItem("Window/Order List Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(OrderListEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            orderList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(OrderList)) as OrderList;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Order List Editor", EditorStyles.boldLabel);
        if (orderList != null)
        {
            if (GUILayout.Button("Show Order List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = orderList;
            }
        }
        if (GUILayout.Button("Open Order List"))
        {
            OpenItemList();
        }
        if (GUILayout.Button("New Order List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = orderList;
        }
        GUILayout.EndHorizontal();

        if (orderList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Order List", GUILayout.ExpandWidth(false)))
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Order List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (orderList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < orderList.orderList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Order", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Order", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (orderList.orderList == null)
                Debug.Log("List is empty");
            if (orderList.orderList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Order", viewIndex, GUILayout.ExpandWidth(false)), 1, orderList.orderList.Count);
                //Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
                EditorGUILayout.LabelField("of   " + orderList.orderList.Count.ToString() + "  orders", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                //orderList.orderList[viewIndex - 1].itemName = EditorGUILayout.TextField("Item Name", orderList.orderList[viewIndex - 1].itemName as string);
                //orderList.orderList[viewIndex - 1].id = EditorGUILayout.IntField("Order ID", orderList.orderList[viewIndex - 1].id);

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                orderList.orderList[viewIndex - 1].weaponType = (Ingot.WeaponType)EditorGUILayout.EnumPopup("Weapon type", orderList.orderList[viewIndex - 1].weaponType);
                orderList.orderList[viewIndex - 1].oreType = (Ingot.OreType)EditorGUILayout.EnumPopup("Ore type", orderList.orderList[viewIndex - 1].oreType);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                orderList.orderList[viewIndex - 1].oreQuality = (Ingot.OreQuality)EditorGUILayout.EnumPopup("Required quality", orderList.orderList[viewIndex - 1].oreQuality);
                orderList.orderList[viewIndex - 1].requiredSharpness = EditorGUILayout.IntSlider("Required sharpness", orderList.orderList[viewIndex - 1].requiredSharpness, 0, 100);
                orderList.orderList[viewIndex - 1].requiredFragility = EditorGUILayout.IntSlider("Required fragility", orderList.orderList[viewIndex - 1].requiredFragility, 0, 100);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                //GUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("Hits per section");
                //orderList.orderList[viewIndex - 1].hitsPerSection[0] = EditorGUILayout.IntField("", orderList.orderList[viewIndex - 1].hitsPerSection[0]);
                //orderList.orderList[viewIndex - 1].hitsPerSection[1] = EditorGUILayout.IntField("", orderList.orderList[viewIndex - 1].hitsPerSection[1]);
                //orderList.orderList[viewIndex - 1].hitsPerSection[2] = EditorGUILayout.IntField("", orderList.orderList[viewIndex - 1].hitsPerSection[2]);
                //GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                orderList.orderList[viewIndex - 1].price = EditorGUILayout.IntField("Price", orderList.orderList[viewIndex - 1].price);
                orderList.orderList[viewIndex - 1].reputation = EditorGUILayout.IntField("Reputation", orderList.orderList[viewIndex - 1].reputation);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                orderList.orderList[viewIndex - 1].orderComplexity = EditorGUILayout.IntField("Reputation level required", orderList.orderList[viewIndex - 1].orderComplexity);
                orderList.orderList[viewIndex - 1].enchantmentLevel = EditorGUILayout.IntField("Enchantment level", orderList.orderList[viewIndex - 1].enchantmentLevel);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                //orderList.orderList[viewIndex - 1].description = EditorGUILayout.TextField("Order description", orderList.orderList[viewIndex - 1].description);
                orderList.orderList[viewIndex - 1].enchantment = (Ingot.Enchantment)EditorGUILayout.EnumPopup("Enchantment", orderList.orderList[viewIndex - 1].enchantment);
                GUILayout.EndHorizontal();


                GUILayout.Space(10);

            }
            else
            {
                GUILayout.Label("This Order List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(orderList);
        }
    }

    void CreateNewItemList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        orderList = CreateOrderList.Create();
        if (orderList)
        {
            orderList.orderList = new List<Order>();
            string relPath = AssetDatabase.GetAssetPath(orderList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenItemList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Order List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            orderList = AssetDatabase.LoadAssetAtPath(relPath, typeof(OrderList)) as OrderList;
            if (orderList.orderList == null)
                orderList.orderList = new List<Order>();
            if (orderList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        Order newItem = new Order();
        //newItem.itemName = "New Order";
        orderList.orderList.Add(newItem);
        viewIndex = orderList.orderList.Count;
    }

    void DeleteItem(int index)
    {
        orderList.orderList.RemoveAt(index);
    }
}