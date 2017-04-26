using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by the Cutscene Manager system. If a character was forced to face a direction, this'll manually undo that lock. ForceMove-ing them to move
/// can undo it as well.
/// </summary>

public class CM_EndFaceDir : MonoBehaviour {


	/// <summary>
	/// The character in question to face a direction.
	/// </summary>
	public GameObject target;

	/// <summary>
	/// The move reference, for convenience.
	/// </summary>
	private Movement moveRef;

	// Use this for initialization
	void Start () {
		moveRef = target.GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			Activate ();
		}
	}

	public void Activate(){
		target.GetComponent<Movement> ().StopFaceDir ();
	}
}
