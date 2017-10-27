using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuMagicBar : MonoBehaviour
{
    void Init()
    {
        Image bar = GetComponent<Image>();
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();
        if (cd != null && cd.Character != null)
        {
            bar.fillAmount = cd.Character.MagicBarValue / cd.Character.MagicBarTarget;
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
