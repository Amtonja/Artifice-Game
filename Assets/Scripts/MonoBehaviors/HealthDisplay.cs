using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HealthDisplay : PartyUIElement
{
    private Text healthDisplay;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        healthDisplay = GetComponent<Text>();
        healthDisplay.text = player.Health + "/" + player.Stats.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.HealthChanged)
        {
            healthDisplay.text = player.Health + "/" + player.Stats.MaxHealth;
        }
    }    
}
