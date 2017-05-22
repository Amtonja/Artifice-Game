using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_MoveObject : MonoBehaviour {

	//Move multiple objects in a direction for a distance. Used mostly for non-player objects; does not account for animation.


	public GameObject target;
	private Vector3 startPosition;
	private Transform[] playerStartPositions;
	public bool bAndPlayers = false;
	public Vector2 dir;
	public float distance;
	public float moveSpeed;




	/// <summary>
	/// The pass target. This is the next link in the CM chain.
	/// </summary>
	public GameObject passTarget;

	private bool bRunning = false;


	// Use this for initialization
	void Start () {
		dir = Vector2.down;
	}
	
	// Update is called once per frame
	void Update () {
		if (!bRunning) {
			return;
		}

		target.transform.Translate ((dir * moveSpeed) * Time.deltaTime);
		if (Vector3.Distance (startPosition, target.transform.position) >= distance) {
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
		startPosition = target.transform.position;
		bRunning = true;
	}
}
