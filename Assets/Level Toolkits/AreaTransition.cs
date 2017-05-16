using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransition : MonoBehaviour {

	/// <summary>
	/// Used to transition between rooms inside a scene. (As opposed to changing scenes to load a new area entirely.)
	/// </summary>

	private enum transitionState {waiting, transitionClose, transitionOpen, cleanup}
	private transitionState state;

	public Vector2 enterPos; //where the player is positioned on the new room

	public GameObject effect; //which effect to use.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == transitionState.waiting) {
			//Nothing goes here. Literally just sitting around waiting for a player to start this.

		} else if (state == transitionState.transitionClose) {
			//Waiting on AreaTransitionEffect to do its thing and darken the screen.

		} else if (state == transitionState.transitionOpen) {
			//Waiting for AreaTransitionEffect to reveal the screen again.




		}

		
	}


	//Called by Player.cs when it touches the collider attached to this object.
	public void Begin(){
		state = transitionState.transitionOpen;
		//lock character input

		//Send notification to AreaTransitionEffect to begin the effect
	}

	public void CloseComplete(){
		//Called by the transition effect to tell us it's done transitioning to black.
		//now that the screen is dark, move characters to new position.

		//Change the current areaInfo to the new one.

		//Move the camera position.

		//Tell 
		state = transitionState.transitionOpen;
	}
}
