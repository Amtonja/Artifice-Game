﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ActionBar : PartyUIElement
{    
    private Image barImage;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        barImage = GetComponent<Image>();
        barImage.fillAmount = 0;     
    }   

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            barImage.fillAmount = player.ActionBarTimer / player.ActionBarTargetTime;
        }
    }
}