using System;
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

    public void GiveConsumable(Inventory recipient, Item item, int quantity)
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

    public void UseConsumable(Item item)
    {
        if (consumables.ContainsKey(item))
        {
            consumables[item]--;
            if (consumables[item] == 0)
            {
                consumables.Remove(item);
            }
            // call item's use method
        }
        else
        {
            Debug.LogError("Inventory of " + gameObject.name + " does not have any of " + item.itemName);
        }

    }

    public void GiveWeapon(Inventory recipient, Weapon weapon)
    {
        if (equipables.Contains(weapon))
        {
            equipables.Remove(weapon);
            recipient.ReceiveWeapon(weapon);
        }
        else
        {
            Debug.LogError("Inventory of " + gameObject.name + " does not have any of " + weapon.itemName);
        }
    }

    public void ReceiveWeapon(Weapon weapon)
    {
        equipables.Add(weapon);
    }

    public void EquipWeapon(Weapon weapon, bool isPrimary)
    {
        Gear gear = GetComponent<Gear>();
        if (gear != null)
        {
            if (isPrimary)
            {
                equipables.Remove(weapon);
                gear.primaryWeapon = weapon;
            }
            else
            {
                equipables.Remove(weapon);
                gear.secondaryWeapon = weapon;
            }
        }
    }

    public void UnequipWeapon(bool isPrimary)
    {
        Gear gear = GetComponent<Gear>();
        if (gear != null)
        {
            if (isPrimary)
            {
                equipables.Add(gear.primaryWeapon);
                gear.primaryWeapon = null;
            }
            else
            {
                equipables.Add(gear.secondaryWeapon);
                gear.secondaryWeapon = null;
            }
        }
    }

    public void GiveKeyItem(Inventory recipient, Item item)
    {
        if (keyItems.Contains(item))
        {
            keyItems.Remove(item);
            recipient.ReceiveKeyItem(item);            
        }
        else
        {
            Debug.LogError("Inventory of " + gameObject.name + " does not have any of " + item.itemName);
        }
    }

    public void ReceiveKeyItem(Item item)
    {
        if (keyItems.Contains(item))
        {
            Debug.LogError(gameObject.name + " already has the item " + item.itemName);
        }
        else
        {
            keyItems.Add(item);
        }
    }
}
