using System.Collections;
using System.Collections.Generic;
using Artifice.Characters;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<Item, int> items;
    private static int StackSize = 99;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GiveItem(Inventory recipient, Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] == 0)
            {
                items.Remove(item);
            }
        }
        else
        {
            Debug.LogError("Inventory of " + gameObject.name + " does not have any of " + item.itemName);
        }
        recipient.ReceiveItem(item, quantity);
    }

    public void ReceiveItem(Item item, int quantity)
    {
        if (items.ContainsKey(item))
        {
            if (items[item] < StackSize)
            {
                items[item]++;
            }
            else
            {
                Debug.LogError("Inventory of " + gameObject.name + " cannot hold any more of " + item.itemName);
            }
        }
        else
        {
            items.Add(item, 1);
        }
    }
}
