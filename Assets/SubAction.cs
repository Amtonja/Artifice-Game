using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SubAction : MonoBehaviour, ICancelHandler, ISubmitHandler
{
    private ActionIcon _actionIcon;
    private CombatPlayerUI parentUI;
    private TextMeshProUGUI textmesh;    

    public string methodName;

    // Use this for initialization
    void Start()
    {
        _actionIcon = GetComponentInParent<ActionIcon>();
        parentUI = GetComponentInParent<CombatPlayerUI>();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCancel(BaseEventData eventData)
    {
        _actionIcon.CloseSubmenu();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        parentUI.Invoke(methodName, 0);
    }
}
