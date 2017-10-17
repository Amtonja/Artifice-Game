using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class FlagsOran : MonoBehaviour {


	//Simple flag checker thing for Oran. 6 current cutscene states.

	public GameObject flagOne; //CM teleport, starts initial cutscene (scene 1)
	public GameObject flagTwo; //relequary door dialogue (scene 2)
	public GameObject flagThree; //relequary door open
	public GameObject flagThreeB; //(relequary sprite on)
	public GameObject flagFour; //entered relequary conversation (scene 3)
	public GameObject flagFive; //secondary relequary conversation (scene 4)
	public GameObject flagSix; //third relequary conversation (scene 5)
	public GameObject flagSeven; //elevator puzzle complete
	public GameObject flagEight; //Winter scene over (winter)
	public GameObject flagEightB; //exploding walls
	public GameObject flagEightC; //trigger
	public GameObject flagEightD; //key

	//so start and awake don't work well with the dialogue engine, so we have to let it run a frame or two before going
	private int count = 0;
	private bool bFinished = false;


	// Use this for initialization
	void Update () {
		if (bFinished) {
			return;
		}

//		count++;
//		if (count >= 1) {
			bFinished = true;
//		}


		//Enable/disable cutscenes the player has already experienced


		//Introductory cutscene as they enter oran desert

		if (DialogueLua.DoesVariableExist("Oran Cutscene 1") && !DialogueLua.GetVariable("Oran Cutscene 1").AsBool)
		{
			flagOne.SendMessage ("Activate");
//			flagOne.SetActive(false);
			//Might as well flag it as complete
			DialogueLua.SetVariable("Oran Cutscene 1", true);
			Debug.Log (flagOne.name.ToString() + "woooobles");
		}
		//Door to relequary conversation
		if (DialogueLua.DoesVariableExist("Oran Cutscene 2") && DialogueLua.GetVariable("Oran Cutscene 2").AsBool)
		{
			flagTwo.SetActive (false); //turn off conversation object
		}
		//relequary door open
		if (DialogueLua.DoesVariableExist("Oran Cutscene 3") && DialogueLua.GetVariable("Oran Cutscene 3").AsBool)
		{
			flagThree.SetActive (false); //disable door
			flagThreeB.GetComponent<PuzzleNode>().SilentActivate(); //turn on puzzle light
		}

		//Entered relequary conversation
		if (DialogueLua.DoesVariableExist("Oran Cutscene 4") && DialogueLua.GetVariable("Oran Cutscene 4").AsBool)
		{
			flagFour.SetActive (false);
		}

		//Second relequary conversation
		if (DialogueLua.DoesVariableExist("Oran Cutscene 5") && DialogueLua.GetVariable("Oran Cutscene 5").AsBool)
		{
			flagFive.SetActive (false);
		}

		//Third relequary conversation
		if (DialogueLua.DoesVariableExist("Oran Cutscene 6") && DialogueLua.GetVariable("Oran Cutscene 6").AsBool)
		{
			flagSix.SetActive (false);
		}

		//elevator puzzle complete
		if (DialogueLua.DoesVariableExist("Oran Cutscene 7") && DialogueLua.GetVariable("Oran Cutscene 7").AsBool)
		{
			flagSeven.SendMessage("SilentActivate");
		}

		//Winter scene over
		if (DialogueLua.DoesVariableExist("Oran Cutscene 8") && DialogueLua.GetVariable("Oran Cutscene 8").AsBool)
		{
			flagEight.SetActive (false); //turn off winter
			flagEightB.SetActive (false); //turn off exploding walls
			flagEightC.SetActive (false); //turn off trigger
			flagEightD.SetActive (false); //turn off key
		}
	}
	

}
