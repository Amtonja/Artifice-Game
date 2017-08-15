using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PuzzleNode : MonoBehaviour {

	/// <summary>
	/// Puzzle node for Oran door and reliquary puzzle.
	/// When an activate impulse is received, it turns on a sprite and sends a notification to the puzzle controller, 
	/// </summary>

	public GameObject puzzleController; //who to send notifications to
	public int number; //which number in the puzzle this object corresponds to
	public bool bToggle; //whether or not this can be toggled on and off
    public AudioClip clickSound;

	private bool bOn = false; //whether or not this has been activated
	private SpriteRenderer spr;
    private AudioSource _audio;

	// Use this for initialization
	void Start () {
		spr = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Activate (){
		
		if (!bOn) {
			bOn = true;
			puzzleController.SendMessage ("UpdateMe", number);
			Debug.Log ("Node activated!");
			spr.enabled = true;
//            _audio.PlayOneShot(clickSound);
		} else {
			if (bToggle) {
				puzzleController.SendMessage ("UpdateMe", number);
				Debug.Log ("Node deactivated!");
				spr.enabled = false;
				bOn = false;
                _audio.PlayOneShot(clickSound);
            }

		}

	}


	public void Deactivate(){
		//Called by puzzle controller. Turns off a node.
		bOn = false;
		spr.enabled = false;
		Debug.Log ("Node manually deactivated!");
	}



	void OnDrawGizmos(){
		//	void OnDrawGizmosSelected(){
		//		if(targetList != null){
		if(puzzleController != null){
			//			foreach(GameObject target in targetList){

			//draw a line from our position to it
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(this.transform.position, puzzleController.transform.position);

			//			}

		}
	}
}
