using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_PlayAnimation : MonoBehaviour {

	public Movement target; //character in question
	public string animationName; //title of animation, as it appears in the animation controller (NOT the animation file's name)

	public GameObject passTarget;

	public void Activate()
	{
		target.PlayAnimation (animationName);
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

	public void BATMANS(){


	}

}
