using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class NameDisplay : PartyUIElement
{
    private TextMeshProUGUI nameDisplay;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();        
        nameDisplay = GetComponent<TextMeshProUGUI>();        
    }    

    void OnEnable()
    {
        if (player != null && player.enabled)
        {
            nameDisplay.text = player.Stats.characterName;
        }
        else
        {
            nameDisplay.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
