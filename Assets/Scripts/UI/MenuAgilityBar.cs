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
        if (cd == null)
        {
            CharacterSelectButton csb = GetComponentInParent<CharacterSelectButton>();
            if (csb != null && csb.Character != null)
            {
                bar.fillAmount = csb.Character.AgilityBarValue / csb.Character.AgilityBarTarget;
            }
        }
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
