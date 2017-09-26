using System.Collections;
using System.Collections.Generic;
using Artifice.Characters;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<Item, int> consumables;
    private List<Item> equipables;
    private List<Item> keyItems;
    private static int StackSize = 99;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GiveConsumable(Inventory recipient, Item item, int quantity)
    {
        if (consumables.ContainsKey(item))
        {
            consumables[item]--;
            if (consumables[item] == 0)
            {
                consumables.Remove(item);
            }
        }
        else
        {
            Debug.LogError("Inventory of " + gameObject.name + " does not have any of " + item.itemName);
        }
        recipient.ReceiveConsumable(item, quantity);
    }

    public void ReceiveConsumable(Item item, int quantity)
    {
        if (consumables.ContainsKey(item))
        {
            if (consumables[item] < StackSize)
            {
                consumables[item]++;
            }
            else
            {
                Debug.LogError("Inventory of " + gameObject.name + " cannot hold any more of " + item.itemName);
            }
        }
        else
        {
            consumables.Add(item, 1);
        }
    }
}
