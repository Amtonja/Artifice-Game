using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knossis_LaserPuzzle : MonoBehaviour {

	//Receives trigger Activate 

	//When a switch is flipped, pan camera over to the eye



	public GameObject laserTrigger; //the trigger volume
	public GameObject turret; //to disable the turret and base

	private bool bComplete = false; //puzzle complete
	private bool bPushRunning = false; //actively in the process of pushing the player

	private float timer = 0.5f;
	private float timerCurrent = 0f;
	private float pushSpeedMax = 2f;
	private float pushSpeedCurrent = 2f;


	public GameObject iceNode;
	public GameObject fireNode;
	private bool bIced = false;  //node 0
	private bool bFired = false; //node 1
	private int currentAction = 0;

	public Transform cameraPos;
	private bool bCameraMoving = false;
	private bool bCameraReturning = false;
	private float cameraMoveSpeed = 3f;

	private float waitTimer = 1f;
	private float waitTimerCurrent = 0f;

	//Working state for puzzles
	// 0 = rest
	// 1 = camera moving to position
	// 2 = brief pause
	// 3 = action (fire or ice presentation)
	// 4 = brief pause
	// 5 = camera move to player position
	// 6 = reset all

	private int state = 0;

	private GameObject cam; 
	private Vector3 endPos; //for camera direction
	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");

		endPos = new Vector3 (cameraPos.transform.position.x, cameraPos.transform.position.y, -10);

	}
	
	// Update is called once per frame
	void Update () {
		if (bComplete) {
			return;
		}

		if (bPushRunning) {
			Push ();
		}

		if (state == 0) {
			//Idle
			return;
		} else if (state == 1) {
			//moving to camera position

		} else if (state == 2) {
			//brief pause
			waitTimerCurrent += Time.deltaTime;
			if(waitTimerCurrent >= waitTimer){
				waitTimerCurrent = 0;
				state = 3;
			}
		} else if (state == 3) {
			//action (fire or ice presentation)

			state = 4; //temp until we get actual effects

			//currentAction needs to be 2 for effect to show up to destroy turret


		} else if (state == 4) {
			//brief pause
			waitTimerCurrent += Time.deltaTime;
			if(waitTimerCurrent >= waitTimer){
				waitTimerCurrent = 0;
				state = 5;
				endPos = PlayManager.instance.party [0].transform.position;
				endPos.z = -10f;
				bCameraMoving = true;
				bCameraReturning = true;
			}

		} else if (state == 5) {
			//Move back to player position
		} else if (state == 6) {
			//turn off both nodes, reset, go to 0

			//reset camera
			endPos = new Vector3 (cameraPos.transform.position.x, cameraPos.transform.position.y, -10);
			bCameraReturning = false;
			state = 0;
			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
				PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (false);
			}
			cam.GetComponent<CameraSystem> ().LockCam (false);
			iceNode.GetComponent<PuzzleNode> ().Deactivate ();
			fireNode.GetComponent<PuzzleNode> ().Deactivate ();
		}
	}

	// The camera needs to be moved during LateUpdate or it'll look choppy. 
	void LateUpdate () {
		if (bCameraMoving) {
			if (Vector3.Distance(cam.transform.position, new Vector3(endPos.x, endPos.y, -10f)) > 0.1f){ //always needs to be -10 z!
				Vector3 moveDelta = new Vector3(endPos.x, endPos.y, -10f) - cam.transform.position;
				cam.transform.Translate((moveDelta.normalized * cameraMoveSpeed) * Time.deltaTime);
			} else {
				Debug.Log("Camera at position!");
				bCameraMoving = false;
				if (!bCameraReturning) {
					state = 2;
				} else {
					state = 6;
				}
			}
		}
	}


	private void Push(){
		timerCurrent += Time.deltaTime;
		if (timerCurrent < timer) {
			Vector2 moveDelta;
			moveDelta = Vector2.left;
			PlayManager.instance.party [0].gameObject.transform.Translate((moveDelta.normalized * pushSpeedCurrent) * Time.deltaTime);
			pushSpeedCurrent = pushSpeedCurrent * 0.95f;
		} else {
			timerCurrent = 0; 
			bPushRunning = false;
			//			PlayManager.instance.party [0].GetComponent<Movement> ().ForceLock (false);
			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
				PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (false);
				PlayManager.instance.party [i].GetComponent<Movement> ().ResetFollowList ();
				pushSpeedCurrent = pushSpeedMax;
			}

		}
	}


	//Called by the trigger volume
	public void Activate(){
		//if ice is present but heat is not, ice shattering inum goes here

		//laser burst goes here


		bPushRunning = true;

		for (int i = 0; i < PlayManager.instance.party.Length; i++) {
			PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (true);
		}
	}

	public void UpdateMe(int num){
		currentAction = num; //update which was picked
		if(num == 0){ bIced = true;}
		if (num == 1) {
			if (bIced) {
				currentAction = 2; //correctly solved the puzzle

			} else {
				bFired = true;
			}

		}

		state = 1;
		bCameraMoving = true;
		bCameraReturning = false; //moving to position
		for (int i = 0; i < PlayManager.instance.party.Length; i++) {
			PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (true);
		}
		cam.GetComponent<CameraSystem> ().LockCam (true);
	}
}
