using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Teleport : MonoBehaviour {

	/// <summary>
	/// Teleports an object to a location.
	/// </summary>

	public GameObject target;
	public GameObject newPos;
	public Vector3 corrections;

	public bool bUsePlayer = false;
	public int playerNumber;
	public GameObject passTarget;



	public void Activate()
	{
		Vector3 tempPos = newPos.transform.position;
		if (corrections.x != 0) {
			tempPos.x = corrections.x;
		}
		if (corrections.y != 0) {
			tempPos.y = corrections.y;
		}
		if (corrections.z != 0) {
			tempPos.z = corrections.z;
		}

		if (!bUsePlayer) {
			target.transform.position = tempPos;
		} else {
			PlayManager.instance.party [playerNumber].transform.position = tempPos;
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