using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Disabler : MonoBehaviour {

	/// <summary>
	/// Disables an object.
	/// </summary>

	public GameObject target;

	public GameObject passTarget;
	// Use this for initialization

	public void Activate (){
		target.SetActive (false);
		passTarget.SendMessage ("Activate");
		//		Debug.Log ("Interactable activated!");
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
