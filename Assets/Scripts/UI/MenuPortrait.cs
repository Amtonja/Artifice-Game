using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(Image))]
public class MenuPortrait : MonoBehaviour
{    
    void Init()
    {
        Image portrait = GetComponent<Image>();
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();
        if (cd == null)
        {
            CharacterSelectButton csb = GetComponentInParent<CharacterSelectButton>();
            if (csb != null && csb.Character != null)
            {
                portrait.sprite = csb.Character.Stats.menuPortrait;
            }
        }
        if (cd != null && cd.Character != null)
        {
            portrait.sprite = cd.Character.Stats.menuPortrait;
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
