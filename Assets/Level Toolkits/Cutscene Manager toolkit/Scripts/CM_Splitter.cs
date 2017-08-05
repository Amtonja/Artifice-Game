using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Splitter : MonoBehaviour {


	public GameObject[] passTarget;

	public void Activate (){

			foreach (GameObject target in passTarget) {
				target.SendMessage ("Activate");

				Debug.Log ("Splitter started!");
			}



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
