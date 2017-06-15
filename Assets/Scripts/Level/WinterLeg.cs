using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterLeg : MonoBehaviour {

	//Animation controller can only call functions as an event to scripts attached to it

	public void LegUpEnded(){
		this.transform.parent.GetComponent<WinterController> ().LegUp ();
	}

	public void AttackEnded(){
		this.transform.parent.GetComponent<WinterController> ().FinishedAttack ();
	}
}
