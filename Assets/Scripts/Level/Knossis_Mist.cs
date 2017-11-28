using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knossis_Mist : MonoBehaviour {

	public float scrollSpeedX = 0.3f;
	public float scrollSpeedY = 0.3f;
	private Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
		//Adjust opacity
		Color newColor = Color.white;//new Color(255/4, 255/4, 255, alphaColor);
		newColor[3] = 0.5f;
		rend.material.color = newColor;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float offsetX = Time.time/2 + scrollSpeedX;
		float offsetY = Time.time/2 + scrollSpeedY;
		rend.material.mainTextureOffset = new Vector2 (offsetX,-offsetY);
//		rend.material.mainTextureOffset = new Vector2 (scrollSpeedX,-scrollSpeedY);
//		Debug.Log (rend.material.);
	}
}
