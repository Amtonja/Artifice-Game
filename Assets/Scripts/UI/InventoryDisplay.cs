using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Artifice.Characters;
using UnityEngine.EventSystems;

public class InventoryDisplay : MonoBehaviour
{
    public GameObject itemListingPrefab;

    public ItemInfoDisplay itemInfoDisplay;

    public GameObject characterSelectPanel;

    void OnEnable()
    {
        Initialize(PlayManager.instance.Inventory);
    }

    void Initialize(Inventory inventory)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Consumable, int> c in inventory.Consumables)
        {
            Debug.Log("Inventory contains: " + c.Value + " " + c.Key.itemName + "s");
            GameObject item = Instantiate(itemListingPrefab, transform, false) as GameObject;
            item.GetComponent<Text>().text = c.Key.itemName + " " + c.Value.ToString();
            item.GetComponent<ItemListing>().Item = c.Key;
        }        
    }
}
