using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Function : MonoBehaviour {

	public GameObject target;
	public string text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("Chain started!");
			target.SendMessage (text);
		}
	}
}
