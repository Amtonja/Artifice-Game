using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Interact : MonoBehaviour {

	/// <summary>
	/// Basic class that can be interacted with to start something as part of the CM.
	/// </summary>

	public GameObject passTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Activate (){
		passTarget.SendMessage ("Activate");
//		Debug.Log ("Interactable activated!");
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
