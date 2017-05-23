using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Reset : MonoBehaviour {

	/// <summary>
	/// Sends a reset pulse to an object.
	/// </summary>

	public GameObject target;
	public GameObject passTarget;

	public void Activate()
	{
		target.SendMessage ("Reset");
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

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(this.transform.position, target.transform.position);

			//			}

		}
	}

}
