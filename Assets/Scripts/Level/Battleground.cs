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
	public bool bPlayersFightRight = true;
	public Transform[] enemyPosList;

	public List<Player> enemies;
	private Player[] party;

	private int readyCount = 0; //How many times characters tell us they're ready for combat. Needs to be equal to the player count
	private int totalReadyCount;

	public Transform[] secondPlayerPosList;

	private Vector2 range; //limitations of this battleground's borders, for purposes of pre-combat wandering functionality

	private bool bRunning = false; //used for resetting battleground so player can walk out of its collision
	private int blinkTotal = 7;
	private int blinkCurrent = 0;

	private float alphaColor = 1.0f; //for temporary blink

	// Use this for initialization
	void Start () {
		//Get boundaries, send to AI for idle wandering
		BoxCollider2D box = this.GetComponent<BoxCollider2D> ();
		range.x = box.bounds.size.x /2; //needs to be half the value
		range.y = box.bounds.size.y /2;
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].GetComponent<AIBase> ().SetOrigin(this.transform.position);
			enemies [i].GetComponent<AIBase> ().SetBoundries (range);

		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (bRunning) {
			Resetting ();
		}
	}



	//Called by a player object when they collide with a battleground's trigger.
	public void Begin(){
		
		UnityEngine.Debug.Log ("Battleground started!");
		//Disable our collider so it doesn't trigger multiple times
		this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		PlayManager.instance.EnemyCombatants = enemies;
		PlayManager.instance.ExploreMode = false;
		party = PlayManager.instance.Party;

		PlayManager.instance.CurrentBattleground = this;

		totalReadyCount = party.Length + enemies.Count; //Later, should be party + enemies

		//Move everything into their spaces
		for (int i = 0; i < party.Length; i++) {
			party [i].GetComponent<Movement> ().GetForcedSender (this.gameObject);
			party [i].GetComponent<Movement> ().StartForcedMove (playerPosList [i].transform.position);
			party [i].GetComponent<Movement> ().BIgnoreFollow = true;
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].GetComponent<Movement> ().StopForcedMove (false);
			enemies [i].GetComponent<AIBase> ().CombatStart (bPlayersFightRight);// tells to stop wandering
			enemies [i].GetComponent<Movement> ().GetForcedSender (this.gameObject);
			enemies [i].GetComponent<Movement> ().StartForcedMove (enemyPosList [i].transform.position);
		}

	}

	//Run command sent from players, to PlayManager, that ends up here to pause and then re-enable the combat collision.
	public void RunAway(){
		bRunning = true;
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].GetComponent<AIBase> ().BHold = true;// tells to stop wandering
		}
		alphaColor = 0.1f;
		for (int i = 0; i < party.Length; i++) {
			party [i].GetComponent<Movement> ().BIgnoreFollow = false;
		}
	}

	private void Resetting(){
		//Make them blink
		if (blinkCurrent < blinkTotal) {
			Color newColor = Color.white;//new Color(255/4, 255/4, 255, alphaColor);
			newColor [3] = alphaColor;

			for (int i = 0; i < enemies.Count; i++) {
				enemies [i].GetComponent<SpriteRenderer> ().color = newColor;//.a = alphaColor;
			}

			alphaColor = alphaColor + alphaColor * 0.07f;
			if (alphaColor >= 1.0f) { 
				alphaColor = 0.1f;
//				this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				blinkCurrent++;

	
//				_animator.Play (Animator.StringToHash ("Idle"));
			}
		} else {
			for (int i = 0; i < enemies.Count; i++) {
				enemies [i].GetComponent<SpriteRenderer> ().color = Color.white;
				enemies [i].GetComponent<AIBase> ().BHold = false; //resume
				enemies [i].GetComponent<AIBase> ().Reset();

				//reset health
				enemies [i].GetComponent<Player>().Heal(99999);

				bRunning = false;
				this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
				readyCount = 0;
				blinkCurrent = 0;
			}
			Debug.Log ("Done blinking!");
		}

	}

	//Called afte forceMove
	public void MoveComplete(){
		readyCount++;
		if (readyCount == totalReadyCount) {
			Debug.Log ("Errybody ready to party!");

			//Make sure everything is facing the right direction
			for (int i = 0; i < party.Length; i++) {
				if (bPlayersFightRight) {
					party [i].transform.localScale = new Vector3 (1, 1, 1);
				} else {
					party [i].transform.localScale = new Vector3 (-1, 1, 1);
				}

			}
			//Later change to send the AI script a message to tel them what direction they need to face to attack for when they wander
			for (int i = 0; i < enemies.Count; i++) {
				if (!bPlayersFightRight) {
					enemies [i].transform.localScale = new Vector3 (1, 1, 1);
				} else {
					enemies [i].transform.localScale = new Vector3 (-1, 1, 1);
					Debug.Log ("Enemies facing left!");
				}

			}

			//Have everyone in party do into animations
			PlayManager.instance.EnemyEncountered();
		}

	}

	//Called by ???
	public void ChangeHeroFormation(int num){


	}
}
