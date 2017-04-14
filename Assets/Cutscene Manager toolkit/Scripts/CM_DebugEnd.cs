using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_DebugEnd : MonoBehaviour {

	/// <summary>
	/// Presently does nothing other than have an endpoint for the CM chain so it won't generate errors.
	/// </summary>

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate(){
		Debug.Log ("End of chain reached!");
	}
}
