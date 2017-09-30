using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MagicBar : PartyUIElement
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
        if (player == null || !player.enabled)
        {
            barSlider.enabled = false;
            barSlider.fillRect.gameObject.SetActive(false);
            barBorder.enabled = false;
        }
        else
        {
            barSlider.enabled = true;
            barSlider.fillRect.gameObject.SetActive(true);
            barBorder.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.enabled)
        {
            barSlider.value = (float)player.MagicBarValue / (float)player.MagicBarTarget;
        }
    }
}