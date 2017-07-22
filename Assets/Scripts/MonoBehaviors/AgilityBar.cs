using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AgilityBar : PartyUIElement
{
    private Slider barSlider;
    public Image barBorder;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        barSlider = GetComponent<Slider>();        
    }

    void OnEnable()
    {
        if (player == null)
        {
            barSlider.enabled = false;
            barBorder.enabled = false;
        }
        else
        {
            barSlider.enabled = true;
            barBorder.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            barSlider.value = (float)player.AgilityBarValue / (float)player.AgilityBarTarget;
        }
    }
}
