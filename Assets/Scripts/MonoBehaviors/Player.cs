using System.Collections;
using System.Collections.Generic;
using Artifice.Characters;
using UnityEngine;
using System;

public class Player : Entity
{

    /// <summary>
    /// This field is temporary, and will be replaced by the XML reader
    /// </summary>
    [SerializeField]
    private EntityInfo entityInfo;

    [SerializeField]
    private Vector2 spellOrigin;

    private Vector3 v3spellOrigin;

    private float agilityBarValue = 0f, magicBarValue = 0f, rageBarValue = 0f, specialBarValue = 0f;
    private float agilityBarTarget = 20f, magicBarTarget = 20f, rageBarTarget = 20f, specialBarTarget = 20f;

    private Vector3 _footPos;

    private CombatAction _combatAction;
    private SpellDelegate _spell;

    private Animator _animator;

    // Use this for initialization
    void Start()
    {

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

        v3spellOrigin = new Vector3(spellOrigin.x * transform.localScale.x, spellOrigin.y * transform.localScale.y, 0f);
    }

    void Update()
    {
        if (InCombat && health > 0)
        {
			//For damage flickering
			if(alphaColor <= 1){

				Color newColor = Color.white;//new Color(255/4, 255/4, 255, alphaColor);
				newColor[3] = alphaColor;
				this.gameObject.GetComponent<SpriteRenderer>().color = newColor;//.a = alphaColor;
				alphaColor = alphaColor + alphaColor * 0.7f;
				if(alphaColor >= 1.0f){ alphaColor = 1;}
			}

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
        _animator.Play(Animator.StringToHash("SwordAttack"));
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
    }

    //Called by animator. Ensures damage is dealt on the correct attack frame
    public void EndMeleeAttack()
    {
        int damage = Stats.Strength; // Get real formula
        tempTarget.TakeDamage(damage - tempTarget.Stats.Defense); // Get real formula
    }

    public void RangedAttack(Entity target)
    {
        tempTarget = target;
        _animator.Play(Animator.StringToHash("GunAttack"));
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
    }

    //Called by animator. Ensures damage is dealt on the correct attack frame
    public void EndRangedAttack()
    {
        int damage = Stats.Accuracy; // Get real formula
        tempTarget.TakeDamage(damage - tempTarget.Stats.Defense); // Get real formula
    }

    public void BeginSpellCast(Entity target)
    {
        Debug.Log(name + " begins casting a spell...");
        //_spell = spell;
        tempTarget = target;
        _animator.Play(Animator.StringToHash("CastSpell"));
    }

    public void EndSpellCast()
    {
        Debug.Log(name + " finishes casting " + MySpell.Method);
        MySpell(tempTarget);
    }

    public void BoltSpell(Entity target)
    {
		//_animator.Play(Animator.StringToHash("CastSpell"));
        GameObject lightningBolt = Instantiate(Resources.Load("Prefabs/SimpleLightningBoltPrefab"), transform) as GameObject;
        lightningBolt.GetComponent<LineRenderer>().sortingLayerName = "VisualEffects"; // Doing this here because it's not exposed in inspector
        DigitalRuby.LightningBolt.LightningBoltScript lbs = lightningBolt.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
        lbs.StartObject = gameObject;
        lbs.StartPosition = v3spellOrigin;
        lbs.EndObject = target.gameObject;
        Destroy(lightningBolt, 0.5f);

        int damage = Stats.Magic; // Get real formula
        target.TakeDamage(damage - target.Stats.MagicDefense); // Get real formula
        ActionBarValue = 0f;
        IsMyTurn = false;
        PlayManager.instance.UnpauseGame();
    }

    // Assuming these will eventually be different
    public void GustSpell(Entity target)
    {
		//_animator.Play(Animator.StringToHash("CastSpell"));
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

    public void FireBreath(Entity target)
    {
		_animator.Play(Animator.StringToHash("CastSpell"));
        //Debug.Log(name + " breathing fire on " + target.name);
        Quaternion rotToTarget = Quaternion.LookRotation(target.transform.position - transform.position);
        GameObject firebreath = Instantiate(
            Resources.Load("Prefabs/FireBreath", typeof(GameObject)),
            transform.position + v3spellOrigin,
            rotToTarget,
            transform) as GameObject;
        //Destroy(firebreath, 0.5f); // not used if the object destroys itself anyway

        int damage = Stats.Magic;
        target.TakeDamage(damage - target.Stats.MagicDefense);
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
    private struct EntityInfo
    {
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
    private struct CombatStatsInfo
    {
        [Range(1, 20)]
        public int strength, defense, magic, speed, evasion, accuracy, magicDefense, luck;

        [Range(1, 200)]
        public int maxHealth;
    }

    /// <summary>
    /// Raises the TriggerEnter2D event.
    /// This will be used (for now) to handle when combat should begin.
    /// </summary>
    /// <param name="other">Other.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
//        if (other.gameObject.tag.Equals("Enemy"))
		if(other.gameObject.tag.Equals("Battleground"))
        {
//            PlayManager.instance.EnemyEncountered(other.gameObject.GetComponent<Player>());
			other.gameObject.GetComponent<Battleground>().Begin();
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

    IEnumerator FadeOut(SpriteRenderer sr)
    {
        float elapsedTime = 0.0f;
        float totalTime = 1.0f;
        Color targetColor = new Color(1f, 0f, 1f, 0f);
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            sr.color = Color.Lerp(Color.white, targetColor, (elapsedTime / totalTime));
            yield return null;
        }
    }

    public delegate void CombatAction(Entity target);
    public delegate void SpellDelegate(Entity target);

    #region implemented abstract members of Entity

    public override void Interact()
    {
        throw new NotImplementedException();
    }

    public override void Die()
    {
        Debug.Log(gameObject.name + " Died!");

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(sr));   
    }

    public override void EnterCombat()
    {
        //		_animator = GetComponent<Animator>(); //Can't find the instance if it's not here? What?
        _animator.Play(Animator.StringToHash("SwordIdle"));
        //		Movement move = this.gameObject.GetComponent<Movement> ();
        //GetComponent<Movement>().ForceLock(true); //probably redundant
        base.EnterCombat();
    }

    public override void ExitCombat()
    {
        //		Movement move = this.gameObject.GetComponent<Movement> ();
        //		move.ForceLock (false);
        //GetComponent<Movement>().ForceLock(false);
        base.ExitCombat();
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

    public SpellDelegate MySpell
    {
        get
        {
            return _spell;
        }

        set
        {
            _spell = value;
        }
    }
    #endregion
}
