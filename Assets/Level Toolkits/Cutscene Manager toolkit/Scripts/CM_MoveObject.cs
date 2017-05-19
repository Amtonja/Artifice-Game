using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_MoveObject : MonoBehaviour {

	//Move multiple objects in a direction for a distance. Used mostly for non-player objects; does not account for animation.


	public GameObject[] targets;
	public Vector2 dir;
	public float distance;


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

	}
}
