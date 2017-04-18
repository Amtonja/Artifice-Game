using System.Collections;
using System.Collections.Generic;
using Artifice.Characters;
using UnityEngine;
using System;

public class Player : Entity {

	/// <summary>
	/// This field is temporary, and will be replaced by the XML reader
	/// </summary>
	[SerializeField]
	private EntityInfo entityInfo;       

    // Use this for initialization
    void Start () {

		//TODO: This will be loaded in via XML reader later
		this.title = entityInfo.name;
		this.id = entityInfo.id;
		this.stats = new CombatStats();
		this.stats.Level = 1;
		this.stats.BaseAccuracy = entityInfo.combatStats.accuracy;
		this.stats.BaseDefense = entityInfo.combatStats.defense;
		this.stats.BaseEvasion = entityInfo.combatStats.evasion;
		this.stats.BaseMagic = entityInfo.combatStats.magic;
		this.stats.BaseMagicDefense = entityInfo.combatStats.magicDefense;
		this.stats.BaseMaxHealth = entityInfo.combatStats.maxHealth;
		this.stats.BaseSpeed = entityInfo.combatStats.speed;
		this.stats.BaseStrength = entityInfo.combatStats.strength;

        stats.MaxHealth = stats.BaseMaxHealth;
        health = stats.MaxHealth;
	}

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            TakeDamage(20);
        }
    }

	/// <summary>
	/// A struct to hold necessary info for this entity,
	/// allowing it to be easily accessed and modified
	/// in the Unity Editor.
	/// </summary>
	[Serializable]
	private struct EntityInfo {
		public string name;
		public string id;
		public CombatStatsInfo combatStats;
		public CharacterRace race;
	}

	/// <summary>
	/// A struct to hold necessary info for this entity,
	/// allowing it to be easily accessed and modified
	/// in the Unity Editor.
	/// </summary>
	[Serializable]
	private struct CombatStatsInfo {
		[Range(1,20)]
		public int strength, defense, magic, speed, evasion, accuracy, magicDefense, luck;

		[Range(10,200)]
		public int maxHealth;
	}

	/// <summary>
	/// Raises the TriggerEnter2D event.
	/// This will be used (for now) to handle when combat should begin.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other) {
		if(other.gameObject.tag.Equals("Enemy")) {
			PlayManager.instance.EnemyEncountered(other.gameObject.GetComponent<Player>());
		}
	}

	#region implemented abstract members of Entity

	public override void Interact ()
	{
		throw new NotImplementedException ();
	}

	public override void Die ()
	{
		Debug.Log(gameObject.name + " Died!");
	}

	public override void EnterCombat ()
	{
		base.EnterCombat ();
	}

	public override void ExitCombat ()
	{
		base.ExitCombat ();
	}

    #endregion

    #region C# Properties
    
    #endregion

}
