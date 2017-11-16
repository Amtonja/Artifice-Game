using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuSpecialBar : MonoBehaviour {

    void Init()
    {
        Image bar = GetComponent<Image>();
        CharacterDisplay cd = GetComponentInParent<CharacterDisplay>();
        if (cd == null)
        {
            CharacterSelectButton csb = GetComponentInParent<CharacterSelectButton>();
            if (csb != null && csb.Character != null)
            {
                bar.fillAmount = csb.Character.SpecialBarValue / csb.Character.SpecialBarTarget;
            }
        }
        if (cd != null && cd.Character != null)
        {
            bar.fillAmount = cd.Character.SpecialBarValue / cd.Character.SpecialBarTarget;
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
