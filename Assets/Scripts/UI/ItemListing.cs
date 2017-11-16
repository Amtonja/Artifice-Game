using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Characters;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemListing : MonoBehaviour, ISelectHandler
{
    private Consumable item;

    public UnityEvent quantityChanged;

    public int GetQuantity()
    {
        if (PlayManager.instance.Inventory.Consumables.ContainsKey(Item))
        {
            return PlayManager.instance.Inventory.Consumables[Item];
        }
        else
        {            
            Destroy(gameObject);
            return 0;
        }
    }

    public void UpdateDisplay()
    {
        GetComponent<Text>().text = Item.itemName + " " + GetQuantity();
    }

    public void OnClick()
    {
        //PlayManager.instance.Inventory.UseConsumable(Item, GameObject.Find("Hurley").GetComponent<Player>());
        CharacterSelectPanel csp = GetComponentInParent<InventoryDisplay>().characterSelectPanel.GetComponent<CharacterSelectPanel>();
        csp.gameObject.SetActive(true);
        foreach (CharacterSelectButton button in csp.GetComponentsInChildren<CharacterSelectButton>())
        {
            button.Item = Item;
            button.ItemListing = this;
        }
        EventSystem.current.SetSelectedGameObject(csp.transform.GetChild(0).gameObject);
        //quantityChanged.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GetComponentInParent<InventoryDisplay>().itemInfoDisplay.CurrentItem = Item;
    }

    public Consumable Item
    {
        get
        {
            return item;
        }

        set
        {
            item = value;
        }
    }
}
