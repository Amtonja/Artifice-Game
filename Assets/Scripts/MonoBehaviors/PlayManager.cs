using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The singleton manager for the current play session
/// This is more specific than the GameManager and non-persistent
/// </summary>
public class PlayManager : MonoBehaviour {

	/// <summary>
	/// Currently only 2 states the game can be in, explore or combat
	/// If exploreMode is true, we are exploring
	/// If false, we are in combat
	/// </summary>
	private bool exploreMode;

	/// <summary>
	/// Singleton instance
	/// </summary>
	public static PlayManager instance;

	/// <summary>
	/// Reference to the combat UI manager, so that when combat
	/// begins, the UI will automatically show all the characters info.
	/// </summary>
	public CombatUIManager combatUI;

	/// <summary>
	/// The current players in the party
	/// </summary>
	public Player[] party;

	/// <summary>
	/// The enemies the player is currently engaged with.
	/// Usually this list is empty unless in combat.
	/// </summary>
	private List<Player> combatantEnemies;

	void Start () {
		exploreMode = true;
		instance = this;
		combatantEnemies = new List<Player>();
	}

	/// <summary>
	/// This is called when a player controlled character encounters an enemy.
	/// This is currently only being called by OnTriggerEnter through the Player
	/// script attached to a player controlled character.
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	public void EnemyEncountered(Player enemy) {
		exploreMode = false;
		combatantEnemies.Add(enemy);
		SetupCombatPositions(enemy);

		enemy.EnterCombat();
		for (int i = 0; i < party.Length; i++) {
			party[i].EnterCombat();
			combatUI.SetupPlayerUI(party[i]);
		}
	}

	/// <summary>
	/// Moves the players and combatants to their appropriate battle positions as
	/// set up in the CombatSpace script. This assumes that the enemy has a CombatSpace
	/// object childed to the enemy.
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	private void SetupCombatPositions(Player enemy) {
		CombatSpace space = enemy.transform.FindChild("CombatSpace").GetComponent<CombatSpace>();
		for (int i = 0; i < party.Length; i++) {
			party[i].GetComponent<Movement>().enabled = false;
			party[i].transform.position = space.PlayerPosition(i);
		}
		enemy.transform.position = space.EnemyPosition();
	}

	/// <summary>
	/// Call this when the encounter is complete.
	/// </summary>
	public void EncounterComplete() {
		exploreMode = true;

		for (int i = 0; i < party.Length; i++) {
			party[i].ExitCombat();
		}
	}

	public List<Player> EnemyCombatants {
		get {
			return combatantEnemies;
		}
	}

	public bool ExploreMode {
		get {
			return exploreMode;
		}
	}
}
