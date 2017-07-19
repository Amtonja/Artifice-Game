using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ActionBar : PartyUIElement
{
    private Image barImage;
    private GameObject barBorder;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        barImage = GetComponent<Image>();
        barImage.fillAmount = 0f;
        barBorder = transform.GetChild(0).gameObject;
    }

    void OnEnable()
    {
        if (player == null)
        {
            barImage.enabled = false;
            barBorder.SetActive(false);
        }
        else
        {
            barImage.enabled = true;
            barBorder.SetActive(true);
        }
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
