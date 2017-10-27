using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuAgilityBar : MonoBehaviour
{
    void Init()
    {
        Image bar = GetComponent<Image>();
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();
        if (cd != null && cd.Character != null)
        {
            bar.fillAmount = cd.Character.AgilityBarValue / cd.Character.AgilityBarTarget;
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
