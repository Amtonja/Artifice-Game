using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Explode : MonoBehaviour {

	private bool bRunning = false;

	Vector2 moveDir; 

	private float xSpeed = 2f; //how fast it's moving horizontally. Sent via Exode on instantiation. 12, 6, 2
	private float ySpeed = 8f; //how fast (and far) it goes up

	// Use this for initialization
	void Start () {
		moveDir.x = xSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (bRunning) {
			//		if(Vector2.Distance(this.transform.position, mainCam.transform.position) > 30.0f) { Destroy (this.gameObject);}
			ySpeed = ySpeed - 0.5f;//(ySpeed * 0.9f); //reduce ySpeed and adjust trajectory
			//		UnityEngine.Debug.Log ("ySpeed = " + ySpeed.ToString());
			moveDir.y = ySpeed;
			transform.Translate (moveDir * Time.deltaTime);
		}
	}

	public void Activate(){

		bRunning = true;

	}
}
