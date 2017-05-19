using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Layer : MonoBehaviour {

	//Changes the layer of a sprite. Used so you can, say, hide a sprite behind another.

	public SpriteRenderer _sprite;
	public string newLayer; //name of layer to change this sprite into

	/// <summary>
	/// The pass target. This is the next link in the CM chain.
	/// </summary>
	public GameObject passTarget;

	// Use this for initialization
	void Start () {
//		_sprite.sortingLayerName = "VisualEffects";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate (){
		_sprite.sortingLayerName = newLayer;
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
