using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NameDisplay : PartyUIElement
{
    private Text nameDisplay;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();        
        nameDisplay = GetComponent<Text>();        
    }    

    void OnEnable()
    {
        if (player != null)
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
