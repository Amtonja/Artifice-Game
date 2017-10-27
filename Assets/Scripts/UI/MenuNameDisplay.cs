using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MenuNameDisplay : MonoBehaviour
{
    void Init()
    {
        Text nameDisplay = GetComponent<Text>();
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();
        if (cd != null && cd.Character != null)
        {
            nameDisplay.text = cd.Character.Stats.characterName;
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
