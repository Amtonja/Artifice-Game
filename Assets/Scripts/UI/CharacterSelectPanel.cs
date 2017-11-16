using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPanel : MonoBehaviour
{
    private Player selectedCharacter;    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Player SelectedCharacter
    {
        get
        {
            return selectedCharacter;
        }

        set
        {
            selectedCharacter = value;
        }
    }
}
