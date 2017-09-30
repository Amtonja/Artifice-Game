using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;
using System.Xml;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AttackActionButton : SubActionButton, ICancelHandler, ISubmitHandler, ISelectHandler
{
    public Artifice.Characters.Weapon weapon;

    public bool usePrimaryWeapon;

    public new void OnSubmit(BaseEventData eventData)
    {
        if (usePrimaryWeapon)
        {
            parentUI.OnPrimaryAttack();
        }
        else
        {
            parentUI.OnSecondaryAttack();
        }
    }

    public new void OnSelect(BaseEventData eventData)
    {
        //Debug.Log("Selected button: " + weapon.itemName);
        //Debug.Log("Selected weapon attack value: " + weapon.attackValue);

        if (weapon.description != null && descriptionDisplay != null)
        {
            descriptionDisplay.GetComponent<Text>().text = weapon.description;
        }

        if (descriptionDisplay == null)
        {
            Debug.LogError("Couldn't find object to dislay attack description");
        }
    }
}
