using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Inventory MyInventory;
    public GameObject slotGrid;
    /*public Slot slotPrefab;*/
    public GameObject emptySlot;
    public Text item_description;

    string item_name;
    bool item_equitable;
    int item_amount;

    int discardIndex;

    public List<GameObject> slots = new List<GameObject>(); // used to store the slots

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

        RefreshItem();
        instance.item_description.text = "";
    }

    private void OnEnable()
    {
        RefreshItem();
    }

    public static void UpdateItemInfo(string itemDescription, string itemName, bool itemEquitable, int itemAmount)
    {
        instance.item_description.text = itemDescription;

        instance.item_name = itemName;
        instance.item_equitable = itemEquitable;

        instance.item_amount = itemAmount;
    }

    // update the amount of the item
    public void UpdateItemAmount(string itemName, int newAmount)
    {
        item foundItem = MyInventory.items.Find(i => i != null && i.itemName == itemName);
        if (foundItem != null)
        {
            foundItem.item_amount = newAmount;
        }
    }

    // use the item
    public void useItem()
    {
        return;
    }

    // remove the item from the inventory
    public void discardItem()
    {
        return;
    }

    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        instance.slots.Clear();

        for (int i = 0; i < instance.MyInventory.items.Count; i++)
        {
            instance.slots.Add(Instantiate(instance.emptySlot));

            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotID = i;

            if (instance.MyInventory.items[i] != null)
            {
                instance.slots[i].GetComponent<Slot>().SetUpSlot(instance.MyInventory.items[i]);
            }
            else
            {
                instance.slots[i].GetComponent<Slot>().ClearSlot(); // for empty slots, clear the slot
            }
        }
    }

}
