using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HealthDisplay : PartyUIElement
{
    private TextMeshProUGUI healthDisplay;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        healthDisplay = GetComponent<TextMeshProUGUI>();        
    }

    void OnEnable()
    {
        if (player != null && player.enabled)
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
        if (player != null && player.HealthChanged && player.enabled)
        {
            healthDisplay.text = player.Health + "/" + player.Stats.maxHealth;
        }
    }    
}
