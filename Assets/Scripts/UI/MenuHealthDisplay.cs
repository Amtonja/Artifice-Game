using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MenuHealthDisplay : MonoBehaviour
{
    private Player character;
    private Text healthDisplay;

    void Init()
    {
        healthDisplay = GetComponent<Text>();
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();
        if (cd == null)
        {
            CharacterSelectButton csb = GetComponentInParent<CharacterSelectButton>();
            Character = csb.Character;
            if (csb != null && csb.Character != null)
            {
                healthDisplay.text = Character.Health + " / " + Character.Stats.maxHealth;
            }
        }
        if (cd != null && cd.Character != null)
        {
            Character = cd.Character;
            healthDisplay.text = Character.Health + " / " + Character.Stats.maxHealth;
        }
    }

    void OnEnable()
    {
        Init();
    }

    void CharacterDisplayChange()
    {
        Init();
    }

    void Update()
    {
        if (Character != null && healthDisplay != null && Character.HealthChanged)
        {
            healthDisplay.text = Character.Health + " / " + Character.Stats.maxHealth;
        }
    }

    public Player Character
    {
        get
        {
            return character;
        }

        set
        {
            character = value;
        }
    }
}
