using Artifice.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleground : MonoBehaviour {

	/// <summary>
	/// This script is used to hold information for instanced localized battles.
	/// 
	/// Moves players and enemies into position, handles introductory affects (into animations, pausing for effect, etc)
	/// 
	/// Communicates with PlayManager, which handles actual combat.
	/// </summary>


//	public List enemies;

	public Transform[] playerPosList;
	public Transform[] enemyPosList;

	public List<Player> enemies;
	private Player[] party;

	private int readyCount = 0; //How many times characters tell us they're ready for combat. Needs to be equal to the player count
	private int totalReadyCount;

	public Transform[] secondPlayerPosList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	//Called by a player object when they collide with a battleground's trigger.
	public void Begin(){
		
		UnityEngine.Debug.Log ("Battleground started!");
		//Disable our collider so it doesn't trigger multiple times
		this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		PlayManager.instance.EnemyCombatants = enemies;
		PlayManager.instance.ExploreMode = false;
		party = PlayManager.instance.Party;

		totalReadyCount = party.Length + enemies.Count; //Later, should be party + enemies

		//Move everything into their spaces
		for (int i = 0; i < party.Length; i++) {
			party [i].GetComponent<Movement> ().GetForcedSender (this.gameObject);
			party [i].GetComponent<Movement> ().StartForcedMove (playerPosList [i].transform.position);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].GetComponent<Movement> ().GetForcedSender (this.gameObject);
			enemies [i].GetComponent<Movement> ().StartForcedMove (enemyPosList [i].transform.position);
		}

	}

	//Called afte forceMove
	public void MoveComplete(){
		readyCount++;
		if (readyCount == totalReadyCount) {
			Debug.Log ("Errybody ready to party!");
			//Have everyone in party do into animations
			PlayManager.instance.EnemyEncountered();
		}

	}

	//Called by 
	public void ChangeHeroFormation(int num){


	}
}
