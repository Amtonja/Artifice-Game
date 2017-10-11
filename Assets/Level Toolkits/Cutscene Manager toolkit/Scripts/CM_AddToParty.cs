using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_AddToParty : MonoBehaviour {

	//Turn off NPC
	//Set party to be one more, assign
	//set follow target to be previous last party position

	public GameObject target; //character in question

	public GameObject passTarget; //next link in chain

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Activate(){
		target.GetComponent<Movement> ().FollowTarget = PlayManager.instance.Party [PlayManager.instance.party.Length - 1].GetComponent<Movement>();
		target.GetComponent<Movement> ().bNPC = false;

		//Workaround for resizing an array
		Player[] temp = new Player[PlayManager.instance.Party.Length + 1];
//		Debug.Log ("Party length is " + PlayManager.instance.Party.Length.ToString ());
		Debug.Log ("Temp length is " + temp.Length.ToString ());
		temp [temp.Length - 1] = target.gameObject.GetComponent<Player>();
		PlayManager.instance.Party.CopyTo(temp, 0);
		PlayManager.instance.Party = temp;

		passTarget.SendMessage ("Activate");
	}

	void OnDrawGizmos(){
		//	void OnDrawGizmosSelected(){
		//		if(targetList != null){
		if(passTarget != null){

			//draw a line from our position to it
			Gizmos.color = Color.green;
			Gizmos.DrawLine(this.transform.position, target.transform.position);

			//			}

		}
	}

}
