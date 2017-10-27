using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TopMenuButton : MonoBehaviour, ISelectHandler
{ 
    public GameObject subMenu;

    public void OnSelect(BaseEventData eventData)
    {
        foreach (TopMenuButton button in transform.parent.GetComponentsInChildren<TopMenuButton>())
        {
            button.subMenu.SetActive(false);
        }
        subMenu.SetActive(true);
    }

}
