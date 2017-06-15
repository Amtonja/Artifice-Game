using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_StartOnLoad : MonoBehaviour {

	//Starts a CM chain when the scene loads.

	public GameObject passTarget;

	private float timer = 1.0f;
	private float currentTimer = 0f;

	private bool bOn = true;

	// Use this for initialization
	void Start () {
		passTarget.SendMessage ("Activate");
		//		Debug.Log ("Interactable activated!");
	}

	void Update(){
		if (!bOn) {
			return;
		}

		currentTimer += Time.deltaTime;
		if (currentTimer >= timer) {
			passTarget.SendMessage ("Activate");
		}
	}


	void OnDrawGizmos(){
		//	void OnDrawGizmosSelected(){
		//		if(targetList != null){
		if(passTarget != null){
			//			foreach(GameObject target in targetList){

			//draw a line from our position to it
			Gizmos.color = Color.green;
			Gizmos.DrawLine(this.transform.position, passTarget.transform.position);

			//			}

		}
	}
}
