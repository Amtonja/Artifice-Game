using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;
using System.Xml;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SubActionButton : MonoBehaviour, ICancelHandler, ISubmitHandler, ISelectHandler
{
    private ActionIcon _actionIcon;
    private CombatPlayerUI parentUI;
    private TextMeshProUGUI textmesh;        
    private GameObject descriptionDisplay;

    public CombatAction action;

    // Use this for initialization
    void Start()
    {
        _actionIcon = GetComponentInParent<ActionIcon>();
        parentUI = GetComponentInParent<CombatPlayerUI>();
        descriptionDisplay = PlayManager.instance.groupCombatUI.transform.Find("Canvas/Panel/ActionInfo/ActionDescription").gameObject;
    }

    public void OnCancel(BaseEventData eventData)
    {
        _actionIcon.CloseSubmenu();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (action.methodName != null)
        {
            parentUI.Invoke(action.methodName, 0);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected button: " + action.actionName);

        if (action.description != null && descriptionDisplay != null)
        {
            descriptionDisplay.GetComponent<Text>().text = action.description;
        }

        if (descriptionDisplay == null)
        {
            Debug.LogError("Couldn't find object to dislay action description");
        }
    }
}
