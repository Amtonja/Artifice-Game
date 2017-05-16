﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransition : MonoBehaviour {

	/// <summary>
	/// Used to transition between rooms inside a scene. (As opposed to changing scenes to load a new area entirely.)
	/// </summary>

	private enum transitionState {waiting, transitionClose, transitionOpen, cleanup}
	private transitionState state;

	public Vector2 enterPos; //where the player is positioned on the new room

	public AreaTransitionEffect effect; //which effect to use.

	public AreaInfo newArea;

	private GameObject cam;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
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
		state = transitionState.transitionClose;
		//lock character input
		for (int i = 0; i < PlayManager.instance.party.Length; i++) {
			PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (true);
		}
		//Send notification to AreaTransitionEffect to begin the effect
		effect.Begin(this);
	}

	public void CloseComplete(){
		//Called by the transition effect to tell us it's done transitioning to black.
		//now that the screen is dark, move characters to new position.
		PlayManager.instance.party[0].transform.position = enterPos;

		//Change the current areaInfo to the new one.
		cam.GetComponent<CameraSystem>().currentArea = newArea;

		//recenter camera
		cam.GetComponent<CameraSystem>().ResetPos();
		//recenter effect
		effect.ResetPos();



		//Move the camera position.

		//Tell AreaTransitionScript to fade back from black again.
		effect.Continue();

		//Change state to wait while it does that.
		state = transitionState.transitionOpen;
	}

	public void OpenComplete(){
		//Unlock character controls.
		for (int i = 0; i < PlayManager.instance.party.Length; i++) {
			PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (false);
		}
		//Go back to waiting.
		state = transitionState.waiting;

	}
}
