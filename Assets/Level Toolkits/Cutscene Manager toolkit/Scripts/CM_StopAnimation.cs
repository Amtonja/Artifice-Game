using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_StopAnimation : MonoBehaviour {

	//Required to stop a character from using a manual animaion and going back to normal behavior.

	public Movement target;

	public GameObject passTarget;

	public void Activate()
	{
		target.EndAnimation ();
		passTarget.SendMessage ("Activate");
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
