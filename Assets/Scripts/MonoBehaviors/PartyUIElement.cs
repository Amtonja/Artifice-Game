using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Characters;

public class PartyUIElement : MonoBehaviour
{
    protected Player player;

    // Use this for initialization
    protected virtual void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        int playerIndex = 0;
        bool result = int.TryParse(name.Substring(name.Length - 1), out playerIndex);
        
        if (result)
        {
            player = PlayManager.instance.party[playerIndex];
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

    }
}
