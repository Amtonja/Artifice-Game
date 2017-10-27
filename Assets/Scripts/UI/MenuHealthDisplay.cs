using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MenuHealthDisplay : MonoBehaviour
{
    void Init()
    {
        Text healthDisplay = GetComponent<Text>();
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();
        if (cd != null && cd.Character != null)
        {
            healthDisplay.text = cd.Character.Health + " / " + cd.Character.Stats.maxHealth;
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
}
