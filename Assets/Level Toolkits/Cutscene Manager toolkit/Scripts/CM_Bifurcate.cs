using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Bifurcate : MonoBehaviour {

	//A simple class that points to a different chain after it's been activated once.

	private bool bActivated = false;

	public GameObject passTargetA;
	public GameObject passTargetB;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate(){
		if (!bActivated) {
			passTargetA.SendMessage ("Activate");
			bActivated = true;
		} else {
			passTargetB.SendMessage ("Activate");
		}
	}

	void OnDrawGizmos(){
		//	void OnDrawGizmosSelected(){
		//		if(targetList != null){
		if(passTargetA != null){
			//			foreach(GameObject target in targetList){

			//draw a line from our position to it
			Gizmos.color = Color.green;
			Gizmos.DrawLine(this.transform.position, passTargetA.transform.position);

			//			}

		}
		if(passTargetB != null){
			//			foreach(GameObject target in targetList){

			//draw a line from our position to it
			Gizmos.color = Color.red;
			Gizmos.DrawLine(this.transform.position, passTargetB.transform.position);

			//			}

		}
	}
}
