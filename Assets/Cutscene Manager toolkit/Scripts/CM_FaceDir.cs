using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by the Cutscene Manager system to force a character to face a direction.
/// Facing a direction is canceled by forcing a movement or by manually using CM_EndFaceDir.
/// </summary>

public class CM_FaceDir : MonoBehaviour {

	/// <summary>
	/// The character in question to face a direction.
	/// </summary>
	public GameObject target;

	/// <summary>
	/// The move reference, for convenience.
	/// </summary>
	private Movement moveRef;

	/// <summary>
	/// The pass target. This is the next link in the CM chain.
	/// </summary>
	public GameObject passTarget;

	/// <summary>
	/// Direction to face.
	/// </summary>
	public enum directions {Up, Down, Left, Right};
	public directions faceDir;


	// Use this for initialization
	void Start () {
		moveRef = target.GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Called via CM modules to activate this script.
	public void Activate(){
		Vector2 newDir = new Vector2();
		if (faceDir == directions.Up) {newDir.y = 1;}
		if(faceDir == directions.Down){newDir.y = -1;}
		if (faceDir == directions.Left){newDir.x = -1;}
		if(faceDir == directions.Right) {newDir.x = 1;}

		target.GetComponent<Movement> ().FaceDir (newDir);
//		target.GetComponent<Movement> ().GetForcedSender (this.gameObject);

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
