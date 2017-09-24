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
    protected ActionIcon _actionIcon;
    protected CombatPlayerUI parentUI;
    protected TextMeshProUGUI textmesh;        
    protected GameObject descriptionDisplay;

    private CombatAction action;
        
    protected void Awake()
    {
        descriptionDisplay = PlayManager.instance.groupCombatUI.transform.Find("Canvas/Panel/ActionInfo/ActionDescription").gameObject;
    }

    // Use this for initialization
    protected void Start()
    {
        _actionIcon = GetComponentInParent<ActionIcon>();
        parentUI = GetComponentInParent<CombatPlayerUI>();
        //descriptionDisplay = PlayManager.instance.groupCombatUI.transform.Find("Canvas/Panel/ActionInfo/ActionDescription").gameObject;
    }

    public void OnCancel(BaseEventData eventData)
    {
        _actionIcon.CloseSubmenu();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (Action.methodName != null)
        {
            parentUI.Invoke(Action.methodName, 0);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected button: " + Action.actionName);

        if (Action.description != null && descriptionDisplay != null)
        {
            descriptionDisplay.GetComponent<Text>().text = Action.description;
        }

        if (descriptionDisplay == null)
        {
            Debug.LogError("Couldn't find object to dislay action description");
        }
    }

    public CombatAction Action
    {
        get
        {
            return action;
        }

        set
        {
            action = value;
        }
    }

}
