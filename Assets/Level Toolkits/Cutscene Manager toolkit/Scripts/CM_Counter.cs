using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Counter : MonoBehaviour {

	/// <summary>
	/// Waits for a specific number of activations.
	/// </summary>

	public int count; //however many we need
	private int countTotal = 0;

	public GameObject passTarget;

	public void Activate()
	{
		countTotal += 1;
		if (countTotal >= count) {
			passTarget.SendMessage ("Activate");
			countTotal = 0;
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
