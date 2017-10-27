using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterDisplay : MonoBehaviour, ISelectHandler
{
    private Player character;

    void Start()
    {
        
    }
        
    void OnEnable()
    {       
        if (PlayManager.instance.Party.Length >= transform.GetSiblingIndex() + 1)
        {
            Character = PlayManager.instance.Party[transform.GetSiblingIndex()];            
        }        
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        if (Character != null)
        {
            GetComponentInParent<StatMenu>().SelectedCharacter = Character;
        }
    }

    public Player Character
    {
        get
        {
            return character;
        }

        set
        {
            if (character == value) return;
            character = value;            
            BroadcastMessage("CharacterDisplayChange");
        }
    }
}
