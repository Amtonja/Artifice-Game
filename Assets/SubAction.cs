using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubAction : MonoBehaviour, ICancelHandler
{
    private ActionHex _actionHex;

    // Use this for initialization
    void Start()
    {
        _actionHex = GetComponentInParent<ActionHex>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCancel(BaseEventData eventData)
    {
        _actionHex.CloseSubmenu();
    }
}
