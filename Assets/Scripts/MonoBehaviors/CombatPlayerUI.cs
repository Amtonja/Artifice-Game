using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatPlayerUI : MonoBehaviour {

	private Player activePlayer;

	//All the various ui elements that need to be updated during battle
	private Image actionBar, specialBar, rageBar, magicBar, agilityBar, healthBar;
	private Text playerName, playerLevel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	/// <summary>
	/// Sets the active player, and also updates the
	/// UI to initially display info about the character.
	/// </summary>
	/// <value>The active player.</value>
	public Player ActivePlayer {
		set {
			activePlayer = value;
			playerName.text = activePlayer.Name;
			playerLevel.text = activePlayer.Stats.Level.ToString("00");
		}
		get {
			return activePlayer;
		}
	}
}
