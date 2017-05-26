using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Trigger : MonoBehaviour {

	/// <summary>
	/// The player steps on this to activate something.
	/// </summary>


	public GameObject[] passTarget;
	public bool bStayOn = false;
	private bool bTriggered; //only trigger once

	public bool bStartInactive = false;

	// Use this for initialization
	void Start () {
		bTriggered = bStartInactive;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate (){
		if (!bTriggered || bStayOn == true) {
			foreach (GameObject target in passTarget) {
				target.SendMessage ("Activate");
				bTriggered = true;
				Debug.Log ("Trigger started!");
			}
		}


	}

	public void Reset(){
		bTriggered = false;

	}





	void OnDrawGizmos(){
		//	void OnDrawGizmosSelected(){
		//		if(targetList != null){
//		if(passTarget != null){
		foreach(GameObject target in passTarget){

			//draw a line from our position to it
			Gizmos.color = Color.green;
			Gizmos.DrawLine(this.transform.position, target.transform.position);

			//			}

		}
	}
}
