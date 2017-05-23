using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Parent : MonoBehaviour {

	/// <summary>
	/// Parents an object to another object
	/// </summary>

//	public GameObject target;
	public int playerIndex = 0;
	public GameObject parent;

	public GameObject passTarget;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate (){
//		target.transform.parent = parent.transform;
		PlayManager.instance.party[playerIndex].transform.parent = parent.transform;
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
