using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knossis_BarrierPuzzle : MonoBehaviour {

	public GameObject[] nodeList;
	public GameObject[] barrierList;
	public GameObject[] battlegroundList;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//received by puzzlenodes
	public void UpdateMe(int num){
		//Turn off barrier, turn on battleground
		barrierList[num].SetActive(false);
		battlegroundList [num].SetActive (true);
		battlegroundList [num].GetComponent<Battleground> ().Begin ();
	}
}
