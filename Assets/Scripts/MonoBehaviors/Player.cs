﻿using System.Collections;
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

        ResetStats();
        health = Stats.MaxHealth;

        ActionBarTarget = 20;
	}

    void Update ()
    {
        if (InCombat)
        {
            if (ActionBarValue < ActionBarTarget)
            {
                ActionBarValue += Stats.Speed / 10.0f; // Don't use this value                
            }            
            else if (ActionBarValue >= ActionBarTarget)
            {
                // take a turn
                IsMyTurn = true;                
            } 
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
        // DEBUG
        //public float abv;
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

    /// <summary>
    /// Sets all combat stats equal to their base values.
    /// Used for initialization or when all modifying effects are removed.
    /// </summary>
    void ResetStats()
    {
        Stats.Accuracy = Stats.BaseAccuracy;
        Stats.Defense = Stats.BaseDefense;
        Stats.Evasion = Stats.BaseEvasion;
        Stats.Loyalty = Stats.BaseLoyalty;
        Stats.Magic = Stats.BaseMagic;
        Stats.MagicDefense = Stats.BaseMagicDefense;
        Stats.MaxHealth = Stats.BaseMaxHealth;
        Stats.Speed = Stats.BaseSpeed;
        Stats.Strength = Stats.BaseDefense;
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
