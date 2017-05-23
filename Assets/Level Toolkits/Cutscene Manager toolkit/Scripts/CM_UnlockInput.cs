using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_UnlockInput : MonoBehaviour {

	/// <summary>
	/// Unlocks player movement.
	/// </summary>

	public GameObject passTarget;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate (){
		for (int i = 0; i < PlayManager.instance.party.Length; i++) {
			PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock(false);
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
