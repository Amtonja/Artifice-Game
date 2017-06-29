using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ActionBar : MonoBehaviour
{
    private Player player;
    private Image barImage;

    // Use this for initialization
    void Start()
    {
        FindPlayer();
        barImage = GetComponent<Image>();
        barImage.fillAmount = 0;     
    }

    void FindPlayer()
    {
        int playerIndex = 0;
        bool result = int.TryParse(name.Substring(name.Length - 1), out playerIndex);
        if (result)
        {
            player = PlayManager.instance.Party[playerIndex];
        }
        else
        {
            Debug.LogError(name + ": Couldn't find a player index; make sure object name ends in an array index");
        }

        if (player == null)
        {
            Debug.LogError(name + ": Couldn't find a party member of index " + playerIndex + " in the party list");
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
