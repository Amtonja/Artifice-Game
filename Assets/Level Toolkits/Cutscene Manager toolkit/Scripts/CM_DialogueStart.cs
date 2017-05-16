using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Dialogue integration with the CM chain.
/// </summary>

public class CM_DialogueStart : MonoBehaviour {

	/// <summary>
	/// The name of the dialogue.
	/// </summary>
	public string dialogueName; 

	// Use this for initialization
	void Start () {
//		DialogueManager.StartConversation (dialogueName);
//		DialogueManager.
//		this.gameObject.SendMessage("DoThing");

	}

	// Update is called once per frame
	void Update () {

//		if (DialogueManager. {
//			PixelCrushers.DialogueSystem.SendMessageOnDialogueEvent(
//			Debug.Log ("Convo over?");
//		}
	}


	public void DoThing(){
		UnityEngine.Debug.Log ("Batman!");

	}

	//Called via CM modules to activate this script.
	public void Activate(){
		DialogueManager.StartConversation (dialogueName);
	}

}
