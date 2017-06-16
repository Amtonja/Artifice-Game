using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_MoveCamera : MonoBehaviour {

	//Moves camera around to specific locations. Will need CM_HoldCam to be used beforehand. 

	public Transform endPos;
	public float moveSpeed = 3f;
	public bool bMoveToPlayer = false;

	public GameObject passTarget;

	private bool bRunning = false;

	private GameObject cam; 
	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// The camera needs to be moved during LateUpdate or it'll look choppy. 
	void LateUpdate () {
		if (bRunning) {
			if (Vector3.Distance(cam.transform.position, new Vector3(endPos.position.x, endPos.position.y, -10f)) > 0.1f){ //always needs to be -10 z!
				Vector3 moveDelta = new Vector3(endPos.position.x, endPos.position.y, -10f) - cam.transform.position;
				cam.transform.Translate((moveDelta.normalized * moveSpeed) * Time.deltaTime);
			} else {
				Debug.Log("Camera at position!");
				bRunning = false;
				passTarget.SendMessage ("Activate");
			}
		}
	}

	public void Activate(){
		if (bMoveToPlayer) {
			endPos = PlayManager.instance.party [0].transform;
		}
		bRunning = true;
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
