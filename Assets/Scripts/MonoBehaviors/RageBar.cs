﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RageBar : PartyUIElement
{
    private Image barImage;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        barImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            barImage.fillAmount = (float)player.RageBarValue / (float)player.RageBarTarget;
        }
    }
}