﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterController : MonoBehaviour {


	//0 Activation -> 1 Leg raise -> 2 change leg layer order -> 3 move -> 4 hammer down -> 5 Pass to passtarget -> 6 End


	public GameObject attackLeg;

	public Transform movePos;

	private int state = 0;

	private float moveSpeed = 8.0f;

	public GameObject passTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				state = 1;
				attackLeg.GetComponent<Animator>().Play(Animator.StringToHash("LegUp"));
				Debug.Log ("Waiting on leg!");
			}

		} else if (state == 1) {
			//Waiting on leg


		} else if (state == 2) {
			attackLeg.GetComponent<SpriteRenderer> ().sortingLayerName = "ObscureChars";
			state = 3;

		} else if (state == 3) {
			if (Vector3.Distance (this.transform.position, new Vector3 (movePos.position.x, movePos.position.y, 0)) > 0.1f) {

				//			float step = moveSpeed * Time.deltaTime;
				//			Vector2 moveDelta = Vector2.MoveTowards (this.transform.position, intendedPosition, step);
				Vector3 moveDelta = new Vector3 (movePos.position.x, movePos.position.y, 0f) - this.transform.position;

				transform.Translate ((moveDelta.normalized * moveSpeed) * Time.deltaTime);

			} else {
				Debug.Log ("We're there!");
				state = 4;
			}

		} else if (state == 4) {
			attackLeg.GetComponent<Animator>().Play(Animator.StringToHash("Attack"));

		} else if (state == 5) {
			Debug.Log ("Passing to next target!");
			passTarget.SendMessage ("Activate");
			state = 6;

		} else if(state == 6) {
			//do nothing
		}



	}

	public void LegUp(){
		state = 2;
	}

	public void FinishedAttack (){
		state = 5;
	}


	public void Activate(){
		state = 1;
		attackLeg.GetComponent<Animator>().Play(Animator.StringToHash("LegUp"));
		Debug.Log ("Waiting on leg!");

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
