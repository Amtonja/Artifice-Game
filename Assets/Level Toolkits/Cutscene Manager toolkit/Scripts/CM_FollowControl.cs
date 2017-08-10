using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_FollowControl : MonoBehaviour {
	//turns off or on following behavior.
	public bool bFollow = false;

	public GameObject passTarget;

	public void Activate (){
		foreach (Player play in PlayManager.instance.Party) {
			play.GetComponent<Movement> ().BIgnoreFollow = bFollow;
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
