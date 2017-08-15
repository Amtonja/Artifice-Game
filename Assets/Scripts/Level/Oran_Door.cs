using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oran_Door : MonoBehaviour {

	/// <summary>
	/// Oran Dooropening thing to get into the reliquary.
	/// </summary>

	public GameObject passTarget; 

	private bool[] bNodes;

	// Use this for initialization
	void Start () {
		bNodes = new bool[1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Called by nodes to update their status
	public void UpdateMe(int num){
		bNodes [num] = !bNodes [num];
		if (bNodes [0]){// && bNodes [1]) {
			passTarget.SendMessage ("Activate");
			Debug.Log ("Puzzle done! Opening door!");
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

}
