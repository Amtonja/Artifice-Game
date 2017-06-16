using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oran_WallExplode : MonoBehaviour {

	//Used to explode the various chunks of the wall in the reliquary.

	public GameObject[] chunkList; //list of the individual wall chunks

	public float[] xSpeed;
	public float[] ySpeed;
//	private Vector2[] chunkDirs;
	private float gravity = 0.5f;


	public GameObject passTarget;

	private bool bRunning = false;

	private int count; //for how many have been removed

	// Use this for initialization
	void Start () {
//		chunkDirs = new Vector2[chunkList.Length];
	}
	
	// Update is called once per frame
	void Update () {
		if (bRunning) {
			count = 0; //reset counter

			for(int i = 0; i < chunkList.Length; i++){
//				Debug.Log ("Moving chunk!");
				ySpeed [i] = ySpeed [i] - gravity;
				Vector2 moveDir = new Vector2 (xSpeed[i], ySpeed[i]);
				chunkList[i].transform.Translate (moveDir * Time.deltaTime);

				if (Vector2.Distance (this.transform.position, chunkList [i].transform.position) > 30.0f) {
					count++;
				}

			}

			if (count == chunkList.Length) {
				for (int i = 0; i < chunkList.Length; i++) {
					Destroy (chunkList [i].gameObject);
				}
				bRunning = false;
//				Debug.Log ("Done moving chunks!");
				passTarget.SendMessage ("Activate");
			}

		}
	}

	public void Activate(){

		bRunning = true;
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
