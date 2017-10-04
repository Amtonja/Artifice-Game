using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_Fade : MonoBehaviour {

	public SpriteRenderer target; //the target
	// Use this for initialization

	public bool bFadeIn = true; 
	public float fadeRate = 0.007f;

	public GameObject passTarget;

	private bool bWorking = false;

	private float alphaColor = 1.0f;

	void Start(){
		if (!bFadeIn) { //flip for fade out
			alphaColor = 0f;
		}

	}

	void Update()
	{
		if (bWorking) {
			
			if(bFadeIn){
				Color newColor = Color.white;//new Color(255/4, 255/4, 255, alphaColor);
				newColor[3] = alphaColor;
				target.gameObject.GetComponent<SpriteRenderer>().color = newColor;//.a = alphaColor;
				alphaColor = alphaColor - fadeRate;
				if (alphaColor < 0) {
					bWorking = false;
					passTarget.SendMessage ("Activate");
				}
			} else if(!bFadeIn){
				Color newColor = Color.white;//new Color(255/4, 255/4, 255, alphaColor);
				newColor[3] = alphaColor;
				target.gameObject.GetComponent<SpriteRenderer>().color = newColor;//.a = alphaColor;
				alphaColor = alphaColor + fadeRate;
				if (alphaColor > 1) {
					bWorking = false;
					passTarget.SendMessage ("Activate");
				}
			}

		}
	}


	public void Activate (){
		bWorking = true;
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

