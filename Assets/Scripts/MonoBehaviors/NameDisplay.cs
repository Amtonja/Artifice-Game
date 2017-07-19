using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NameDisplay : PartyUIElement
{
    private Text nameDisplay;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        nameDisplay = GetComponent<Text>();
        nameDisplay.text = player.Stats.characterName;
    }    

    // Update is called once per frame
    void Update()
    {

    }
}
