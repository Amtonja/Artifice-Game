using Artifice.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectButton : MonoBehaviour, ICancelHandler
{
    private Player character;
    private Consumable item;
    private ItemListing itemListing; // the item listing that was clicked to bring us here

    public void OnClick()
    {
        if (Character != null && item != null && ItemListing != null)
        {
            PlayManager.instance.Inventory.UseConsumable(Item, Character);
            ItemListing.quantityChanged.Invoke();
        }   
    }

    void OnEnable()
    {
        if (PlayManager.instance.Party.Length >= transform.GetSiblingIndex() + 1)
        {
            Character = PlayManager.instance.Party[transform.GetSiblingIndex()];
        }
    }
    
    public void OnCancel(BaseEventData eventData)
    {
        transform.parent.gameObject.SetActive(false);
    }

    public Player Character
    {
        get
        {
            return character;
        }

        set
        {            
            if (character == value) return;
            character = value;
            BroadcastMessage("CharacterDisplayChange");
        }
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

    public ItemListing ItemListing
    {
        get
        {
            return itemListing;
        }

        set
        {
            itemListing = value;
        }
    }
}
