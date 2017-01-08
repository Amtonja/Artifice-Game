using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour {

	/// <summary>
	/// This is a reference to the gameobject that should be spawned above
	/// each combatant when we enter combat. This is set up in Start()
	/// </summary>
	private GameObject combatPlayerUI;

	/// <summary>
	/// This list is populated when the PlayManager indicates we have entered combat
	/// </summary>
	private CombatPlayerUI[] combatants;


	void Start () {
		combatPlayerUI = transform.FindChild("Canvas/PlayerUI").gameObject;
	}

	public void SetupPlayerUI(Player p) {
		for(int i = 0; i < combatants.Length; i++) {
			if(combatants[i].ActivePlayer == null) {
				combatants[i].ActivePlayer = p;
				return;
			}
		}
	}
}
