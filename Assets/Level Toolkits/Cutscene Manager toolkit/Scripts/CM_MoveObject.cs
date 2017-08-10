using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_MoveObject : MonoBehaviour {

	//Move multiple objects in a direction for a distance. Used mostly for non-player objects; does not account for animation.


	public GameObject target;
    
//	private Vector3 startPosition;
	public bool bMovePlayer = false;
	public GameObject endLocation;
	public float moveSpeed;

	public bool bJitter = false;
	public float jitterAmount = 0f;

	/// <summary>
	/// The pass target. This is the next link in the CM chain.
	/// </summary>
	public GameObject passTarget;

	private bool bRunning = false;


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (!bRunning) {
			return;
		}

//		target.transform.Translate ((dir * moveSpeed) * Time.deltaTime);
//		if (Vector3.Distance (startPosition, target.transform.position) >= distance) {
//			End ();
//
//		}

		Vector3 moveDelta = new Vector3(endLocation.transform.position.x, endLocation.transform.position.y, 0f) - target.transform.position;
		if (bJitter) {
			moveDelta.x += jitterAmount;
			jitterAmount = jitterAmount * -1;
		}
		if (Vector3.Distance (target.transform.position, endLocation.transform.position) > 0.1f && !bJitter ||
			Vector3.Distance (target.transform.position, endLocation.transform.position) > 0.1f + jitterAmount && bJitter) {
			target.transform.Translate ((moveDelta.normalized * moveSpeed) * Time.deltaTime);
		} else {
			target.transform.position = endLocation.transform.position;
			End ();
		}
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


	void End(){
		passTarget.SendMessage ("Activate");
		bRunning = false;

	}

	public void Activate(){
//		startPosition = target.transform.position;
		bRunning = true;
	}
}
