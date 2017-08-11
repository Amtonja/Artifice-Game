using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterEyeController : MonoBehaviour {

	//Exists so the seperately-animated eye can pass on that it moved to the WinterController
	public void EyeMoved(){
		this.transform.parent.GetComponent<WinterController> ().EyeMoved ();

	}
}
