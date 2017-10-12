using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SceneFlagManager : MonoBehaviour {


	public GameObject[] battlegrounds;

	// Use this for initialization
	void Start () {
		//Check to see which battlegrounds have been wiped in the save file, and set those as deactivated

		int num = 1;
		foreach (GameObject bat in battlegrounds) {
			if (DialogueLua.DoesVariableExist ("Battleground " + num.ToString ()) && DialogueLua.GetVariable ("Battleground " + num.ToString ()).AsBool) {
				//Bool is active, so disable
				bat.SetActive (false);
			}

			num++;

		}

		//If AreaInfo flag is not blank, game was saved in a specific area, so we need to update that
		if (DialogueLua.GetVariable ("AreaInfo").AsString.ToString () != "") {
			GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
			GameObject newArea = GameObject.Find (DialogueLua.GetVariable ("AreaInfo").AsString);


			cam.GetComponent<CameraSystem> ().currentArea = newArea.GetComponent<AreaInfo> ();

		}


	}
	



	//Updates the temporary flags when a battleground is cleared. When combat ends, PlayManager informs the battleground,
	// which in turn calls this for itself. This is to turn off cleared battlegrounds if the player saves and reloads the area.
	//These are reset to false when a new scene is transitioned to
	public void UpdateBattleground (GameObject bat){
		int batNum = System.Array.IndexOf (battlegrounds, bat) + 1;
		DialogueLua.SetVariable ("Battleground " + batNum.ToString (), true);


		Debug.Log ("Battleground wiped, flag set!");
	}


	//Nukes local variables, such as the current areainfo and battlegrounds. Called only when an area is left and transitioned to a new scene
	public void WipeLocals(){


		for (int i = 1; i <= 15; i++) {
			string str = "Battleground " + i.ToString ();
			DialogueLua.SetVariable(str, false);
		

			Debug.Log ("Entry " + i.ToString () + " wiped!");
		}


		//Wipe the current AreaInfo flag
		DialogueLua.SetVariable("AreaInfo", "");

		Debug.Log ("All locals wiped!");
	}

	public void BatCheck(){
		Debug.Log ("Battleground 1 is " + DialogueLua.GetVariable ("Battleground 1").AsBool.ToString ());

	}
}
