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
    
    private float agilityBarValue = 0f, magicBarValue = 0f, rageBarValue = 0f, specialBarValue = 0f;
    private float agilityBarTarget = 20f, magicBarTarget = 20f, rageBarTarget = 20f, specialBarTarget = 20f;

    private Vector3 _footPos;

    private CombatAction _combatAction;

	private Animator _animator;
        
    // Use this for initialization
    void Start () {

		_animator = GetComponent<Animator>();

        _footPos = transform.Find("Base").localPosition;

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
                ActionBarValue += (Stats.Speed / 2.0f) * Time.deltaTime; // Don't use this value                
            }            
            else if (ActionBarValue >= ActionBarTarget)
            {
                // take a turn
                IsMyTurn = true;
                PlayManager.instance.PauseGame();

            }
            
            // Update the other four bars
            if (AgilityBarValue < AgilityBarTarget)
            {
                AgilityBarValue += (Stats.Speed / 12.0f) * Time.deltaTime; // Don't use this value either
            } 

            if (MagicBarValue < MagicBarTarget)
            {
                MagicBarValue += (Stats.Magic / 10.0f) * Time.deltaTime; // Still no
            }

            if (RageBarValue < RageBarTarget)
            {
                RageBarValue += (Stats.Strength / 10.0f) * Time.deltaTime; // What do you think?
            }

            if (SpecialBarValue < SpecialBarTarget)
            {
                SpecialBarValue += (Stats.Loyalty / 10.0f) * Time.deltaTime; // Look, just get real formulas for these
            }
        }
    }

	private Entity tempTarget; //because we need to pass the target entity to the attack end state

    public void MeleeAttack(Entity target)
    {        
        Debug.Log("Melee attack on " + target.name);
		tempTarget = target;
		_animator.Play( Animator.StringToHash( "SwordAttack" ));



		ActionBarValue = 0f;
		IsMyTurn = false;
		PlayManager.instance.UnpauseGame();
    }

	//Called by animator. Ensures damage is dealt on the correct attack frame
	public void EndMeleeAttack(){
		int damage = Stats.Strength; // Get real formula
		tempTarget.TakeDamage(damage - tempTarget.Stats.Defense); // Get real formula


	}

    public void RangedAttack(Entity target)
    {

		tempTarget = target;
		_animator.Play( Animator.StringToHash( "GunAttack" ));
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
    }

	//Called by animator. Ensures damage is dealt on the correct attack frame
	public void EndRangedAttack(){
		int damage = Stats.Accuracy; // Get real formula
		tempTarget.TakeDamage(damage - tempTarget.Stats.Defense); // Get real formula


	}


    public void BoltSpell(Entity target)
    {
        int damage = Stats.Magic; // Get real formula
        target.TakeDamage(damage - target.Stats.MagicDefense); // Get real formula
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
    }

    // Assuming these will eventually be different
    public void GustSpell(Entity target)
    {
        int damage = Stats.Magic; // Get real formula
        target.TakeDamage(damage - target.Stats.MagicDefense); // Get real formula
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
    }

    public void CureSpell(Entity target)
    {
        Debug.Log("Casting Cure on " + target.name);
        int healthRestored = Stats.Magic; // Get real formula
        target.Heal(healthRestored);
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
    }

    public void AimSpell(Entity target)
    {
        int accuracyBuff = Stats.Magic; // Get real formula
        target.Stats.Accuracy += accuracyBuff; // Get real formula
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
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

    public delegate void CombatAction(Entity target);

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
//		_animator = GetComponent<Animator>(); //Can't find the instance if it's not here? What?
		_animator.Play( Animator.StringToHash( "SwordIdle" ) );
//		Movement move = this.gameObject.GetComponent<Movement> ();
//		move.ForceLock (true); //probably redundant
		base.EnterCombat ();
	}

	public override void ExitCombat ()
	{
//		Movement move = this.gameObject.GetComponent<Movement> ();
//		move.ForceLock (false);
		base.ExitCombat ();
	}

    #endregion

    #region C# Properties
    public float AgilityBarValue
    {
        get
        {
            return agilityBarValue;
        }

        set
        {
            agilityBarValue = value;
        }
    }

    public float MagicBarValue
    {
        get
        {
            return magicBarValue;
        }

        set
        {
            magicBarValue = value;
        }
    }

    public float RageBarValue
    {
        get
        {
            return rageBarValue;
        }

        set
        {
            rageBarValue = value;
        }
    }

    public float SpecialBarValue
    {
        get
        {
            return specialBarValue;
        }

        set
        {
            specialBarValue = value;
        }
    }

    public float AgilityBarTarget
    {
        get
        {
            return agilityBarTarget;
        }

        set
        {
            agilityBarTarget = value;
        }
    }

    public float MagicBarTarget
    {
        get
        {
            return magicBarTarget;
        }

        set
        {
            magicBarTarget = value;
        }
    }

    public float RageBarTarget
    {
        get
        {
            return rageBarTarget;
        }

        set
        {
            rageBarTarget = value;
        }
    }

    public float SpecialBarTarget
    {
        get
        {
            return specialBarTarget;
        }

        set
        {
            specialBarTarget = value;
        }
    }

    public Vector3 FootPos
    {
        get
        {
            return _footPos;
        }
    }

    public CombatAction MyCombatAction
    {
        get
        {
            return _combatAction;
        }

        set
        {
            _combatAction = value;
        }
    }
    #endregion
}
