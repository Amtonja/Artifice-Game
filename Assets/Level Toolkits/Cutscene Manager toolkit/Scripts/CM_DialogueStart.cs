using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Dialogue integration with the CM chain.
/// </summary>

public class CM_DialogueStart : MonoBehaviour
{

    /// <summary>
    /// The name of the dialogue.
    /// </summary>
    public string dialogueName;

    /// <summary>
    /// Next object in the CM chain.
    /// </summary>
    public GameObject passTarget;

    // Use this for initialization
    void Start()
    {
        //		DialogueManager.StartConversation (dialogueName);
        //		DialogueManager.
        //		this.gameObject.SendMessage("DoThing");

    }

    // Update is called once per frame
    void Update()
    {

        //		if (DialogueManager. {
        //			PixelCrushers.DialogueSystem.SendMessageOnDialogueEvent(
        //			Debug.Log ("Convo over?");
        //		}
    }


    //public void DoThing()
    //{
    //    Debug.Log("Batman!");
    //}

    public void DialogueComplete()
    {
        Debug.Log("Conversation " + dialogueName + " is over.");
        passTarget.SendMessage("Activate");
    }

    //Called via CM modules to activate this script.
    public void Activate()
    {
        Debug.Log("Starting conversation " + dialogueName + ".");
        DialogueManager.StartConversation(dialogueName);
    }

    void OnDrawGizmos()
    {
        if (passTarget != null)
        {
            //draw a line from our position to it
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, passTarget.transform.position);
        }
    }
}
