using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oran_ElevatorPuzzle : MonoBehaviour {


	/// <summary>
	/// For Oran elevator puzzle.
	/// Correct combination is 1 for left, 3 for right.
	/// </summary>


	//Need to move the elevator into position when conditions are met, and enable elevator trigger collision, and disable elevator blocking collision

	public GameObject passTarget; 

	private bool[] bNodes;

	public PuzzleNode[] nodes;

	//For visual indicators on the elevator
	public GameObject rightNode;
	public GameObject leftNode;

	private bool bComplete = false;
	// Use this for initialization
	void Start () {
		bNodes = new bool[8];
	}
	
	// Update is called once per frame
	void Update () {
		if (bComplete) {
			return;
		}
		//turn on and off lights
//		if (bNodes [0]) {
//			leftNode.SetActive (true);
//		} else {
//			leftNode.SetActive (false);
//		}
//		if (bNodes [6]) {
//			rightNode.SetActive (true);
//		} else {
//			rightNode.SetActive(false);
//		}

	}

	//Called by nodes to update their status
	public void UpdateMe(int num){
		bNodes [num] = true;
		if (num < 4) {
			//Left group
			if (num != 0) {
				leftNode.SetActive (false);
			} else {
				if (leftNode.activeSelf) {
					leftNode.SetActive (false);
				} else {
					leftNode.SetActive (true);
				}
			}


			for (int i = 0; i<4; i++) {
				if (i != num) {
					nodes [i].Deactivate ();
					bNodes [i] = false;
				}
			}
		} else {
			//right group

			if (num != 6) {
				rightNode.SetActive (false);
			} else {
				if (rightNode.activeSelf) {
					rightNode.SetActive (false);
				} else {
					rightNode.SetActive (true);
				}
			}
			for (int i = 4; i<8; i++) {
				if (i != num) {
					nodes [i].Deactivate ();
					bNodes [i] = false;
				}
			}
		}

		if (bNodes [0] && bNodes [6]) {
			passTarget.SendMessage ("Activate");
			Debug.Log ("Puzzle done! Elevator ready!");
            PixelCrushers.DialogueSystem.QuestLog.SetQuestState("Oran Elevator Puzzle", PixelCrushers.DialogueSystem.QuestState.Success);

			rightNode.SetActive(false);
			leftNode.SetActive(false);
			bComplete = true;
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
