using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by the Cutscene Manager system to force a character to move in a direction. Currently this is only one space at at time.
/// </summary>

public class CM_ForceMove : MonoBehaviour {


	/// <summary>
	/// The character in question to move.
	/// </summary>
	public GameObject target;

	/// <summary>
	/// Use players instead
	/// </summary>
	public bool bUsePlayers = false;

	/// <summary>
	/// The index of the party to use if above is true
	/// </summary>
	public int partyIndex = 0; 

	/// <summary>
	/// The move reference, for convenience.
	/// </summary>
//	private Movement moveRef;

	/// <summary>
	/// Direction to move.
	/// </summary>
//	public enum directions {Up, Down, Left, Right};
//	public directions moveDir;

	/// <summary>
	/// How many spaces to move.
	/// </summary>
//	public int spaces = 1;

	/// <summary>
	/// Where to move the character.
	/// </summary>
	public GameObject movePos; 

	/// <summary>
	/// The pass target. This is the next link in the CM chain.
	/// </summary>
	public GameObject passTarget;

	/// <summary>
	/// Whether or not this link has been activated. (Might consider just deactivating the object, but better safe than sorry.)
	/// </summary>
//	private bool bActive = false;


	// Use this for initialization
	void Start () {
//		moveRef = target.GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//Called via CM modules to activate this script.
	public void Activate(){
//		bActive = true;
		Vector2 newDir = new Vector2();
		newDir = movePos.transform.position;
//		if (moveDir == directions.Up) {newDir.y = spaces;}
//		if(moveDir == directions.Down){newDir.y = -spaces;}
//		if (moveDir == directions.Left){newDir.x = -spaces;}
//		if(moveDir == directions.Right) {newDir.x = spaces;}
		if (bUsePlayers) {
			target = PlayManager.instance.party [partyIndex].gameObject;
		}

		target.GetComponent<Movement> ().StartForcedMove (newDir);
		target.GetComponent<Movement> ().GetForcedSender (this.gameObject);

//		Debug.Log ("ForceMove activated!");
	}

	//called by the character's movement script to signal it is done forcing a move
	public void MoveComplete(){
		passTarget.SendMessage ("Activate");
//		Debug.Log ("Force move complete! Next target!");
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
