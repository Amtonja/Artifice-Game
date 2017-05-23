using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitionEffect : MonoBehaviour {

	/// <summary>
	/// Simple script to just hard code some basic area transition effects. 
	/// Called by AreaTransition.
	/// </summary>


	private SpriteRenderer sprite;

	private GameObject cam;

	private bool bRunning = false;

	private float moveSpeed = 28.0f;

	private bool bSecondStage = false;

	private Vector3 escapePos; //where we need to leave

	private GameObject caller; //who called us
	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		sprite = this.GetComponent<SpriteRenderer> ();
		sprite.enabled = false;



	}
	
	// Update is called once per frame
	void Update () {

//		if(Input.GetKeyDown(KeyCode.Space)){
//			Begin(this);
//
//		}

		if (!bRunning) {
			return;
		}

		//Move to center
		if (!bSecondStage) {
			if (Vector3.Distance (this.transform.position, cam.transform.position) > 10) { //8 accounts for z depth difference
				float step = moveSpeed * Time.deltaTime;
				Vector3 newPos = Vector2.MoveTowards (this.transform.position, cam.transform.position, step);
//				transform.Translate ((Vector2.right * moveSpeed) * Time.deltaTime);
				this.transform.position = newPos;
//				Debug.Log (Vector3.Distance (this.transform.position, cam.transform.position));
			} else {
				caller.SendMessage ("CloseComplete");
				Debug.Log ("Faded to black!");
			}

		} else {
			//Move out of view
			if (Vector3.Distance (this.transform.position, escapePos) > 8) {
				float step = moveSpeed * Time.deltaTime;
				Vector2 newPos = Vector2.MoveTowards (this.transform.position, escapePos, step);
				//				transform.Translate ((Vector2.right * moveSpeed) * Time.deltaTime);
				this.transform.position = newPos;
//				Debug.Log (Vector3.Distance (this.transform.position, escapePos));
			} else {
				//we're done
//				caller.OpenComplete();
				caller.SendMessage("OpenComplete");
//				sprite.enabled = false;
				Debug.Log("Moved out of way!");
				bRunning = false;
				bSecondStage = false;
			}
		}

		
	}


	public void Begin(GameObject sender){
		//Begin effect. Called by AreaTransition.
		bRunning = true;

		caller = sender;
		sprite.enabled = true;

		Vector2 newPos = cam.transform.position;
//		this.transform.position.z = 
		newPos.x = cam.transform.position.x - 11f;

		this.transform.position = newPos;
	}

	//Called by sender to continue the transition
	public void Continue(){
		bSecondStage = true;
		//Set up where we need to move to
		escapePos = this.transform.position;
		escapePos.x += 10.5f;

	}

	//Called by sender to reset the pos on top of the camera 
	public void ResetPos(){
		Vector3 temp;
		temp = cam.transform.position;
		temp.z = -8; //why this works I don't know
		this.transform.position = temp;


	}

	public void UpdateSender(GameObject sender){
		caller = sender;

	}
}
