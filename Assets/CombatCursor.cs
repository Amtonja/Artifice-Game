using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CombatCursor : MonoBehaviour
{
    private Player selectedCharacter;

    public float verticalOffset; // may need to vary according to the size of the target sprite

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void MoveToSelectedCharacter()
    {
        GetComponent<TextMeshProUGUI>().text = SelectedCharacter.Name;
        transform.position = Camera.main.WorldToScreenPoint(SelectedCharacter.transform.position + Vector3.up * verticalOffset);
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
            MoveToSelectedCharacter();
        }
    }
}
