using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CM_LoadScene : MonoBehaviour {


	//Loads a new scene.
	public string scene;

	// Use this for initialization
	void Activate(){
//		Application.LoadLevel (scene);
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}
}
