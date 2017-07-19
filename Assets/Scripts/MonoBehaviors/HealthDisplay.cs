﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HealthDisplay : PartyUIElement
{
    private Text healthDisplay;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        healthDisplay = GetComponent<Text>();        
    }

    void OnEnable()
    {
        if (player != null)
        {
            healthDisplay.text = player.Health + "/" + player.Stats.maxHealth;
        }
        else
        {
            healthDisplay.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.HealthChanged)
        {
            healthDisplay.text = player.Health + "/" + player.Stats.maxHealth;
        }
    }    
}
