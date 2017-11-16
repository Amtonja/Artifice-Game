using Artifice.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoDisplay : MonoBehaviour
{
    private Item currentItem = null;
    
    public Item CurrentItem
    {
        get
        {
            return currentItem;
        }
        set
        {
            if (value != currentItem)
            {
                currentItem = value;
                GetComponent<Text>().text = currentItem.description;
            }
        }
    }
}
