using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_DisableCollision : MonoBehaviour {

	//Disables an object's collision.

	public BoxCollider2D target;

	public GameObject passTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Called via CM modules to activate this script.
	public void Activate(){
		target.enabled = false;

		passTarget.SendMessage ("Activate");
	}


	//For gizmo and editor debugging
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
