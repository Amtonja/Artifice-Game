using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_HoldCam : MonoBehaviour {

	/// <summary>
	/// Holds the camera's position.
	/// </summary>


	public bool bOn = true;
	public GameObject passTarget;



	void Activate(){
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		cam.GetComponent<CameraSystem> ().LockCam (bOn);
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
