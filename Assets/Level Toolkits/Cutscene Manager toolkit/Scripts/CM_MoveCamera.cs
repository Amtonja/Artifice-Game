using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_MoveCamera : MonoBehaviour {

	//Moves camera around to specific locations. Will need CM_HoldCam to be used beforehand. 

	public GameObject targetPos;
	private Vector3 endPos;
	public float moveSpeed = 3f;
	public bool bMoveToPlayer = false;

	public Vector2 buffer; //when moving, manually account for screen edges

	public GameObject passTarget;

	private bool bRunning = false;

	private GameObject cam; 
	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		if (!bMoveToPlayer) {
			endPos = new Vector3 (targetPos.transform.position.x + buffer.x, targetPos.transform.position.y + buffer.y, -10);
		}
	}
	
	// The camera needs to be moved during LateUpdate or it'll look choppy. 
	void LateUpdate () {
		if (bRunning) {
			if (Vector3.Distance(cam.transform.position, new Vector3(endPos.x, endPos.y, -10f)) > 0.1f){ //always needs to be -10 z!
				Vector3 moveDelta = new Vector3(endPos.x, endPos.y, -10f) - cam.transform.position;
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
			endPos = PlayManager.instance.party [0].transform.position;
//			endPos.position.x += buffer.x;
//			endPos.position.y += buffer.y;
			endPos = new Vector3 (endPos.x + buffer.x, endPos.y + buffer.y, -10);
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
