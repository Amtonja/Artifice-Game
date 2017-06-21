using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_FlipSprite : MonoBehaviour {

	//Flips a sprite horizontally.

	public Transform target; //the target
	// Use this for initialization

	public bool bFlip = true; //turn false to revert

	public GameObject passTarget;


	public void Activate (){
		if (bFlip) {
			target.transform.localScale = new Vector3 (-2, 2, 1);
		} else {
			target.transform.localScale = new Vector3 (2, 2, 1);
		}

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
