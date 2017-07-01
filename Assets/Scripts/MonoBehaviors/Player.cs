using System.Collections;
using System.Collections.Generic;
using Artifice.Characters;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class Player : Entity
{

    ///// <summary>
    ///// This field is temporary, and will be replaced by the XML reader
    ///   Comment out again when switching back to XML
    ///// </summary>
    [SerializeField]
    private EntityInfo entityInfo;

    public bool useXMLStats;

    [SerializeField]
    private Vector2 spellOrigin;

    private Vector3 v3spellOrigin;

    private int agilityBarValue = 0, magicBarValue = 0, rageBarValue = 0, specialBarValue = 0;
    private int agilityBarTarget = 3, magicBarTarget = 5, rageBarTarget = 10, specialBarTarget;
    private float actionBarDefaultTarget = 6f;

    private Vector3 _footPos;

    private CombatAction _combatAction;
    private SpellDelegate _spell;

    private Animator _animator;

    [SerializeField]
    private string characterName, characterID;

    [SerializeField]
    private AudioClip meleeSFX, rangedSFX, footstepSFX, takeDamageSFX, deathSFX;

    private AudioSource _audio;

    private int experienceTotal;
    
    // Consider these temporary variables standing in for actual equipment values
    public int addedMeleeAttackValue, addedRangedAttackValue, addedMagicAttackValue, armorValue;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _footPos = transform.Find("Base").localPosition;

        //TODO: This will be loaded in via XML reader later
        title = CharacterName;
        id = characterID;

        if (useXMLStats)
        {
            stats = new CombatStats(1, id); // for reading stats from XML
            
            Debug.Log("Stats loaded for " + CharacterName);
            Debug.Log(stats.ToString());
        }        
        else
        {
            stats = new CombatStats(); // default constructor for reading stats from entityInfo
            stats.Level = 1;            
            stats.BaseDefense = entityInfo.combatStats.defense;            
            stats.BaseMagic = entityInfo.combatStats.magic;
            stats.BaseMagicDefense = entityInfo.combatStats.magicDefense;
            stats.BaseMaxHealth = entityInfo.combatStats.maxHealth;
            stats.BaseSpeed = entityInfo.combatStats.speed;
            stats.BaseAttack = entityInfo.combatStats.attack;
        }        

        ResetStats();
        health = Stats.MaxHealth;
        DefenseValue = Mathf.FloorToInt((Stats.Defense + armorValue) * 0.2f);
        MagicDefenseValue = Mathf.FloorToInt((Stats.MagicDefense + armorValue) * 0.2f);    

        // The time for a character's action bar to fill is equal to the default time minus a percentage equal to their Speed stat
        // Likewise for the other bars
        ActionBarTargetTime = actionBarDefaultTarget - (actionBarDefaultTarget * stats.Speed / 100f);
        //AgilityBarTarget = agilityBarDefaultTarget - (agilityBarDefaultTarget * stats.Speed / 100f);
        //MagicBarTarget = magicBarDefaultTarget - (magicBarDefaultTarget * stats.Speed / 100f);

        v3spellOrigin = new Vector3(spellOrigin.x * transform.localScale.x, spellOrigin.y * transform.localScale.y, 0f);        
    }

    void Update()
    {
		//Minor layer tweak to prevent characters from overlapping in weird ways
		GetComponent<SpriteRenderer>().sortingOrder = -(int)(this.transform.position.y* 10);

        if (InCombat && health > 0)
        {
			//For damage flickering
			if(alphaColor < 1){

				Color newColor = Color.white;//new Color(255/4, 255/4, 255, alphaColor);
				newColor[3] = alphaColor;
				this.gameObject.GetComponent<SpriteRenderer>().color = newColor;//.a = alphaColor;
				_animator.Play(Animator.StringToHash("HitFrame"));
				alphaColor = alphaColor + alphaColor * 0.07f;
				if(alphaColor >= 1.0f){ 
					alphaColor = 1;
					this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
//					PlayManager.instance.UnpauseGame ();
					PlayManager.instance.UpdateAttacked();
					_animator.Play(Animator.StringToHash("Idle"));
				}
			}
			//If we're paused, don't do below stuff
			if (PlayManager.instance.PauseCombat == true) {
				return;
			}

            if (ActionBarTimer < ActionBarTargetTime)
            {
                ActionBarTimer += Time.deltaTime;         
            }
            else if (ActionBarTimer >= ActionBarTargetTime)
            {
                // take a turn
                IsMyTurn = true;
                AgilityBarValue++;
                MagicBarValue++;
                RageBarValue++;
                
                PlayManager.instance.PauseGame();
            }            
        }
    }

    public void PlayFootstepSFX()
    {       
        _audio.PlayOneShot(footstepSFX);        
    }

    private Entity tempTarget; //because we need to pass the target entity to the attack end state

    /// <summary>
    /// Determine if an attack will hit by comparing accuracy vs. evasion
    /// and a random number
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool CalculateHit(Entity target)
    {
        bool isAHit = true;

        int attackRoll = UnityEngine.Random.Range(1, 100);

        if (attackRoll <= 5)
        {
            isAHit = false;
            Debug.Log(CharacterName + " missed!");
        }
        
        return isAHit;
    }

    public void MeleeAttack(Entity target)
    {
        Debug.Log("Melee attack on " + target.name);
        tempTarget = target;
        _animator.Play(Animator.StringToHash("SwordAttack"));
        ActionBarTimer = 0f;
        IsMyTurn = false;
//        PlayManager.instance.UnpauseGame();
		PlayManager.instance.SendAttackCount(1);
		PlayManager.instance.StartingAttack ();
    }

    //Called by animator. Ensures damage is dealt on the correct attack frame
    public void EndMeleeAttack()
    {
        _audio.PlayOneShot(meleeSFX);
        int damage = Stats.Attack + addedMeleeAttackValue + UnityEngine.Random.Range(0, 7) - tempTarget.DefenseValue; 
        if (CalculateHit(tempTarget))
        {
            tempTarget.TakeDamage(damage);
        }
        else
        {
            PlayManager.instance.CreatePopupText("Miss", tempTarget.transform, Color.gray);
            PlayManager.instance.UpdateAttacked();            
        }
    }

    public void RangedAttack(Entity target)
    {
        tempTarget = target;
        _animator.Play(Animator.StringToHash("GunAttack"));
        ActionBarTimer = 0f;
        IsMyTurn = false;
//        PlayManager.instance.UnpauseGame();
		PlayManager.instance.SendAttackCount(1);
		PlayManager.instance.StartingAttack ();
    }

    //Called by animator. Ensures damage is dealt on the correct attack frame
    public void EndRangedAttack()
    {
        _audio.PlayOneShot(rangedSFX);
        int damage = Stats.Attack + addedRangedAttackValue + UnityEngine.Random.Range(0, 7) - tempTarget.DefenseValue;
        if (CalculateHit(tempTarget))
        {
            tempTarget.TakeDamage(damage);
        }
        else
        {
            PlayManager.instance.CreatePopupText("Miss", tempTarget.transform, Color.gray);
            PlayManager.instance.UpdateAttacked();
        }
    }

    /// <summary>
    /// The beginning of a spell casting. It plays the spellcasting animation
    /// and tells PlayManager that an attack is being carried out.
    /// </summary>
    /// <param name="target">The target of the spell.</param>
    public void BeginSpellCast(Entity target)
    {
        Debug.Log(name + " begins casting a spell...");
        tempTarget = target;
        _animator.SetBool("SpellComplete", false);
        _animator.Play(Animator.StringToHash("CastSpell"));
        ActionBarTimer = 0f;
        IsMyTurn = false;
        PlayManager.instance.SendAttackCount(1);
        PlayManager.instance.StartingAttack();
    }

    /// <summary>
    /// The end of an actual casting of a spell (but before the effect actually happens.)
    /// This is called by an animation event. 
    /// </summary>
    public void EndSpellCast()
    {
        //Debug.Log(name + " finishes casting " + MySpell.Method);
        MySpell(tempTarget);
    }

    /// <summary>
    /// Called by a spell method to display the visual effect for the given duration, then deal the damage
    /// and clean up the visual effect. It also tells the animator the spell is finished so it can transition
    /// back to the necessary idle state.
    /// </summary>
    /// <param name="target">Target of the spell.</param>
    /// <param name="spellVisual">Game object instantiated for the spell's visual effect.</param>
    /// <param name="spellDuration">Duration in seconds of the visual effect.</param>
    /// <returns></returns>
    public IEnumerator DealSpellDamage(Entity target, GameObject spellVisual, float spellDuration)
    {
        Debug.Log(name + " casts " + spellVisual.name + "at " + target.name + "!");
        PlayManager.instance.DarkenBG(true);
        yield return new WaitForSeconds(spellDuration);

        int damage = Stats.Magic + addedMagicAttackValue + UnityEngine.Random.Range(0, 7) - target.MagicDefenseValue; 
        target.TakeDamage(damage);
        Destroy(spellVisual);
        PlayManager.instance.DarkenBG(false);
        _animator.SetBool("SpellComplete", true);
    }

    public IEnumerator DealSpellHealing(Entity target, GameObject spellVisual, float spellDuration)
    {
        PlayManager.instance.DarkenBG(true);
        yield return new WaitForSeconds(spellDuration);

        int healing = Stats.Magic + addedMagicAttackValue + UnityEngine.Random.Range(0, 7); 
        target.Heal(healing);
        Destroy(spellVisual);
        PlayManager.instance.DarkenBG(false);
        _animator.SetBool("SpellComplete", true);

        PlayManager.instance.UpdateAttacked();
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

        float duration = lbs.GetComponent<AudioSource>().clip.length * 2;
        StartCoroutine(DealSpellDamage(target, lightningBolt, duration));       
//        PlayManager.instance.UnpauseGame();
    }

    // Assuming these will eventually be different
    public void GustSpell(Entity target)
    {
		//_animator.Play(Animator.StringToHash("CastSpell"));
        int damage = Stats.Magic; // Get real formula
        target.TakeDamage(damage - target.Stats.MagicDefense); // Get real formula       
//        PlayManager.instance.UnpauseGame();
    }

    public void CureSpell(Entity target)
    {
        Debug.Log("Casting Cure on " + target.name);
        GameObject cure = Resources.Load("Prefabs/CureSpell") as GameObject;
        Sprite targetSprite = target.GetComponent<SpriteRenderer>().sprite;

        ParticleSystem ps = cure.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule particleBox = ps.shape;
        particleBox.box = new Vector3(targetSprite.bounds.size.x * 2, 1, 0.1f);
        cure = Instantiate(cure, transform);        
        //cure.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + targetSprite.rect.height / 2);
        cure.transform.position = target.transform.position + new Vector3(0, targetSprite.bounds.size.y / 2, 0);
        
        float duration = cure.GetComponent<AudioSource>().clip.length;
        StartCoroutine(DealSpellHealing(target, cure, duration));
    }

    public void AimSpell(Entity target)
    {
        // functionality currently unknown
    }

    public void FireBreath(Entity target)
    {		
        Quaternion rotToTarget = Quaternion.LookRotation(target.transform.position - transform.position);
        GameObject firebreath = Instantiate(
            Resources.Load("Prefabs/FireBreath", typeof(GameObject)),
            transform.position + v3spellOrigin,
            rotToTarget,
            transform) as GameObject;

        float duration = firebreath.GetComponent<AudioSource>().clip.length;
        StartCoroutine(DealSpellDamage(target, firebreath, duration));

        //int damage = Stats.Magic;
        //target.TakeDamage(damage - target.Stats.MagicDefense);
        //ActionBarValue = 0f;
        //IsMyTurn = false;
        //PlayManager.instance.UnpauseGame();
    }

    public void EyeLaser(Entity target)
    {
        GameObject eyeLaser = Instantiate(Resources.Load("Prefabs/EyeLaser", typeof(GameObject)), transform) as GameObject;
        LineRenderer lr = eyeLaser.GetComponent<LineRenderer>();
        lr.sortingLayerName = "VisualEffects";
        lr.numPositions = 2;
        Vector3[] positions = { transform.position + v3spellOrigin, target.transform.position };
        lr.SetPositions(positions);
        StartCoroutine(DealSpellDamage(target, eyeLaser, 1.5f));
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
        public int attack, defense, magic, speed, evasion, accuracy, magicDefense, luck;

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
		//Check to make sure we're not forcelocked. If we are, we're probably being moved around manually,
		//meaning we need to avoid triggers
		if(this.GetComponent<Movement>().CheckForceLock){return;}


//		Debug.Log (PlayManager.instance.party[0].gameObject.name.ToString());
		//Only the character in the lead should be able to instigate things
		if (PlayManager.instance.party [0].gameObject == this.gameObject) {
//        if (other.gameObject.tag.Equals("Enemy"))
			if (other.gameObject.tag.Equals ("Battleground")) {
				//            PlayManager.instance.EnemyEncountered(other.gameObject.GetComponent<Player>());
				other.gameObject.GetComponent<Battleground> ().Begin ();
			}

			if (other.gameObject.tag.Equals ("AreaTransition")) {
				Debug.Log ("Transition to new area!");
				other.gameObject.GetComponent<AreaTransition> ().Begin ();
			}

			if (other.gameObject.tag.Equals ("CM_Trigger")) {
				Debug.Log ("CM_Trigger found");
				other.gameObject.GetComponent<CM_Trigger> ().Activate ();
			}
		}
    }

    /// <summary>
    /// Sets all combat stats equal to their base values.
    /// Used for initialization or when all modifying effects are removed.
    /// </summary>
    void ResetStats()
    {        
        Stats.Defense = Stats.BaseDefense;
        Stats.Magic = Stats.BaseMagic;
        Stats.MagicDefense = Stats.BaseMagicDefense;
        Stats.MaxHealth = Stats.BaseMaxHealth;
        Stats.Speed = Stats.BaseSpeed;
        Stats.Attack = Stats.BaseAttack;
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

		PlayManager.instance.UpdateAttacked ();
		PlayManager.instance.RemoveEnemy (this);
    }

    public void AddExperience(int xp)
    {
        experienceTotal += xp;
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

        _audio.PlayOneShot(deathSFX);

        if (gameObject.CompareTag("Enemy"))
        {
            PlayManager.instance.experiencePool += Stats.XpValue;
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            StartCoroutine(FadeOut(sr));
        }
        else if (gameObject.CompareTag("Player"))
        {
            // TODO: Put player in KO state
            // Animate falling down   
            _animator.Play(Animator.StringToHash("Death"));
            PlayManager.instance.UpdateAttacked();                   
        }
    }

    public override void Revive()
    {
        Debug.Log(gameObject.name + " was revived!");

        _animator.Play(Animator.StringToHash("Intro"));
    }

    public override void EnterCombat()
    {
        //		_animator = GetComponent<Animator>(); //Can't find the instance if it's not here? What?
        _animator.Play(Animator.StringToHash("Intro"));
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

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        _audio.PlayOneShot(takeDamageSFX);
    }

    #endregion

    #region C# Properties
    public int AgilityBarValue
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

    public int MagicBarValue
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

    public int RageBarValue
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

    public int SpecialBarValue
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

    public int AgilityBarTarget
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

    public int MagicBarTarget
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

    public int RageBarTarget
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

    public int SpecialBarTarget
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

    public string CharacterName
    {
        get
        {
            return characterName;
        }

        set
        {
            characterName = value;
        }
    }

    #endregion
}
