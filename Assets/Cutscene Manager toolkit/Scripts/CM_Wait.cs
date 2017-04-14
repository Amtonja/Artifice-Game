using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by the Cutscene Manager system to simply wait around before moving to the next link in the CM chain. 
/// </summary>

public class CM_Wait : MonoBehaviour {

	/// <summary>
	/// Waits for x number of seconds before passing to next link in chain.
	/// </summary>
	public float waitTimer = 0f;
	private float currentTimer = 0f;



	/// <summary>
	/// The pass target. This is the next link in the CM chain.
	/// </summary>
	public GameObject passTarget;

	/// <summary>
	/// Whether or not this link has been activated. (Might consider just deactivating the object, but better safe than sorry.)
	/// </summary>
	private bool bActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (bActive == false)
			return;
		
		currentTimer += Time.deltaTime;
		if (currentTimer >= waitTimer) {
			currentTimer = 0f; //In case this node is reused
			Debug.Log ("Wait timer complete!");
			passTarget.SendMessage ("Activate");
			bActive = false;
		}
		
	}


	//Called via CM modules to activate this script.
	public void Activate(){
		bActive = true;
	}


	//For gizmo and editor debugging
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
