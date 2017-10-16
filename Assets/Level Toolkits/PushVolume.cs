using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushVolume : MonoBehaviour {
	//When entered, bumps a character in a direction.

	//Push player who entered (main player)
	//reset following characters movepaths

	public enum pushDir {Up, Down, Left, Right};
	public pushDir direction;

	private float timer = 0.5f;
	private float timerCurrent = 0f;

	private bool bRunning = false;

	private float pushSpeedMax = 2f;
	private float pushSpeedCurrent = 2f;

	private Vector2 moveDelta;


	void Start(){

		if (direction == pushDir.Up) {
			moveDelta.y = 1;
		} else if (direction == pushDir.Down) {
			moveDelta.y = -1;
		} else if (direction == pushDir.Left) {
			moveDelta.x = -1;
		} else if (direction == pushDir.Right) {
			moveDelta.x = 1;
		}


	}



	void OnTriggerStay2D(Collider2D other)
	{
		if (bRunning) {
			return;
		}
		if (other.gameObject == PlayManager.instance.party [0].gameObject) {

			Debug.Log ("Main character detected!");

			bRunning = true;

			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
				PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (true);
			}
		}

	}

	void Update(){
		if (!bRunning) {
			return;
		}
		timerCurrent += Time.deltaTime;
		if (timerCurrent < timer) {
			PlayManager.instance.party [0].gameObject.transform.Translate((moveDelta.normalized * pushSpeedCurrent) * Time.deltaTime);
			pushSpeedCurrent = pushSpeedCurrent * 0.95f;
		} else {
			timerCurrent = 0; 
			bRunning = false;
//			PlayManager.instance.party [0].GetComponent<Movement> ().ForceLock (false);
			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
				PlayManager.instance.party [i].GetComponent<Movement> ().ForceLock (false);
				PlayManager.instance.party [i].GetComponent<Movement> ().ResetFollowList ();
				pushSpeedCurrent = pushSpeedMax;
			}

		}



	}
}
