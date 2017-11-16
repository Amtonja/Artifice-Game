using System;
using System.Collections;
using System.Collections.Generic;
using Artifice.Characters;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class Inventory : MonoBehaviour
{
    private Dictionary<Consumable, int> consumables;
    private List<Item> equipables;
    private List<Item> keyItems;
    private static int StackSize = 99;

    private bool tempFlag = false;

    // Use this for initialization
    void Start()
    {
        Consumables = new Dictionary<Consumable, int>();
        equipables = new List<Item>();
        keyItems = new List<Item>();

        // TESTING!
        HealthPotion potion = Instantiate(Resources.Load("ScriptableObjects/Items/Potion")) as HealthPotion;
        ReceiveConsumable(potion, 5);

        HealthPotion potion2 = Instantiate(Resources.Load("ScriptableObjects/Items/Aux Potion")) as HealthPotion;        
        ReceiveConsumable(potion2, 4); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!tempFlag)
        {
            GameObject.Find("Hurley").GetComponent<Player>().TakeDamage(300);
            tempFlag = true;
        }   
    }

    public void GiveConsumable(Inventory recipient, Consumable item, int quantity)
    {
        if (Consumables.Keys.Any(c => c.itemName == item.itemName))
        {
            Consumables[item]--;
            if (Consumables[item] == 0)
            {
                Consumables.Remove(item);
            }
        }
        else
        {
            Debug.LogError("Inventory of " + gameObject.name + " does not have any of " + item.itemName);
        }
        recipient.ReceiveConsumable(item, quantity);
    }

    public void ReceiveConsumable(Consumable item, int quantity)
    {
        if (Consumables.Keys.Any(c => c.itemName == item.itemName))
        {
            if (Consumables[item] < StackSize)
            {
                Consumables[item] += quantity;
            }
            else
            {
                Debug.LogError("Inventory of " + gameObject.name + " cannot hold any more of " + item.itemName);
            }
        }
        else
        {
            Consumables.Add(item, quantity);
        }
    }

    public void UseConsumable(Consumable item, CombatEntity target)
    {
        if (Consumables.Keys.Any(c => c.itemName == item.itemName))
        {
            Consumables[item]--;
            if (Consumables[item] == 0)
            {
                Consumables.Remove(item);
            }
            item.OnUse(target);
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

    #region C# Properties
    public Dictionary<Consumable, int> Consumables
    {
        get
        {
            return consumables;
        }

        set
        {
            consumables = value;
        }
    }
    #endregion
}
