using System.Collections;
using System.Collections.Generic;
using Artifice.Characters;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class Player : CombatEntity
{
    private float agilityBarValue = 0f, magicBarValue = 0f, rageBarValue = 0f, specialBarValue = 0f;
    private float agilityBarTarget = 1f, magicBarTarget = 1f, rageBarTarget = 1f, specialBarTarget = 1f;

    private Equipment equipment;
    private Weapon activeWeapon;

    private CombatAction _combatAction;
    private SpellDelegate _spell;

    // Use this for initialization
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        equipment = GetComponent<Equipment>();

        ResetStats();
        Stats.maxHealth += equipment.armor.healthValue;
        health = Stats.maxHealth;
        DefenseValue = Mathf.FloorToInt(Stats.defense * equipment.armor.armorValue * 0.8f);
        MagicDefenseValue = Mathf.FloorToInt(Stats.magicDefense * equipment.armor.magicValue * 0.8f);

        // The time for a character's action bar to fill is equal to the default time minus a percentage equal to their Speed stat
        ActionBarTargetTime = actionBarDefaultTarget - (actionBarDefaultTarget * Stats.speed / 100f);

        v3spellOrigin = new Vector3(spellOrigin.x * transform.localScale.x, spellOrigin.y * transform.localScale.y, 0f);
    }

    void Update()
    {
        //Minor layer tweak to prevent characters from overlapping in weird ways
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(this.transform.position.y * 10);

        if (InCombat && health > 0)
        {
            //For damage flickering
            if (alphaColor < 1)
            {

                Color newColor = Color.white;//new Color(255/4, 255/4, 255, alphaColor);
                newColor[3] = alphaColor;
                this.gameObject.GetComponent<SpriteRenderer>().color = newColor;//.a = alphaColor;
                _animator.Play(Animator.StringToHash("HitFrame"));
                alphaColor = alphaColor + alphaColor * 0.07f;
                if (alphaColor >= 1.0f)
                {
                    alphaColor = 1;
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    //					PlayManager.instance.UnpauseGame ();
                    PlayManager.instance.UpdateAttacked();
                    _animator.Play(Animator.StringToHash("Idle"));
                }
            }
            //If we're paused, don't do below stuff
            if (PlayManager.instance.PauseCombat == true)
            {
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
                RageBarValue += 0.1f;
                MagicBarValue += 0.2f;
                AgilityBarValue += 1f / 3f;
                defending = false;
                PlayManager.instance.PauseGame();

            }
        }
    }

    private CombatEntity tempTarget; //because we need to pass the target entity to the attack end state
    private Weapon tempWeapon;

    /// <summary>
    /// Damage calculation.
    /// </summary>
    /// <param name="target">The entity targeted by the attack</param>
    /// <returns></returns>
    private int CalculateAttackDamage(CombatEntity target, Weapon weapon)
    {
        return Mathf.FloorToInt((Stats.attack * weapon.baseAttackValue) + UnityEngine.Random.Range(0, 7) - target.DefenseValue);
    }

    private int CalculateMagicDamage(CombatEntity target)
    {
        return Mathf.FloorToInt((Stats.magic * equipment.primaryWeapon.baseMagicValue) + UnityEngine.Random.Range(0, 7) - target.MagicDefenseValue);
    }

    public void WeaponAttack(CombatEntity target, Weapon weapon)
    {
        tempTarget = target;
        tempWeapon = weapon;
        // Since we currently only have ranged and melee animations for each character anyway
        if (weapon.type != WeaponType.PISTOL && weapon.type != WeaponType.RIFLE)
        {
            _animator.Play(Animator.StringToHash("SwordAttack"));
        }
        else
        {
            _animator.Play(Animator.StringToHash("GunAttack"));
        }

        ActionBarTimer = 0f;
        IsMyTurn = false;
        PlayManager.instance.SendAttackCount(1);
        PlayManager.instance.StartingAttack();
    }

    public void EndWeaponAttack()
    {
        if (CalculateHit(tempTarget))
        {
            int damage = CalculateAttackDamage(tempTarget, tempWeapon);

            // check for resistance and weaknesses
            switch (tempWeapon.damageType)
            {
                case DamageType.BLUNT:
                    if (tempTarget.MyRes.bBlunt)
                    {
                        damage = (int)(damage * 0.75f);
                        ShowWeakText(tempTarget);
                    }
                    else if (tempTarget.MyWeak.bBlunt)
                    {
                        damage = (int)(damage * 1.5f);
                        ShowStrongText(tempTarget);
                    }
                    break;
                case DamageType.PIERCING:
                    if (tempTarget.MyRes.bPiercing)
                    {
                        damage = (int)(damage * 0.75f);
                        ShowWeakText(tempTarget);
                    }
                    else if (tempTarget.MyWeak.bPiercing)
                    {
                        damage = (int)(damage * 1.5f);
                        ShowStrongText(tempTarget);
                    }
                    break;
                case DamageType.PROJECTILE:
                    if (tempTarget.MyRes.bProjectile)
                    {
                        damage = (int)(damage * 0.75f);
                        ShowWeakText(tempTarget);
                    }
                    else if (tempTarget.MyWeak.bProjectile)
                    {
                        damage = (int)(damage * 1.5f);
                        ShowStrongText(tempTarget);
                    }
                    break;
            } // end switch
            tempTarget.TakeDamage(damage);
        } // end if
        else
        {
            PlayManager.instance.CreatePopupText("Miss", tempTarget.transform, Color.gray, Vector3.zero);
            PlayManager.instance.UpdateAttacked();
        }
    }

    //public void PiercingAttack(CombatEntity target)
    //{
    //    Debug.Log("Piercing attack on " + target.name);
    //    tempTarget = target;
    //    _animator.Play(Animator.StringToHash("SwordAttack"));
    //    ActionBarTimer = 0f;
    //    IsMyTurn = false;
    //    //        PlayManager.instance.UnpauseGame();
    //    PlayManager.instance.SendAttackCount(1);
    //    PlayManager.instance.StartingAttack();
    //}

    ////Called by animator. Ensures damage is dealt on the correct attack frame
    //public void EndPiercingAttack()
    //{
    //    _audio.PlayOneShot(meleeSFX);
    //    int damage = CalculateAttackDamage(tempTarget);
    //    if (CalculateHit(tempTarget))
    //    {
    //        //check for resistance and weaknesses
    //        if (tempTarget.MyRes.bPiercing)
    //        {
    //            damage = (int)((float)damage * 0.75f);
    //            ShowWeakText(tempTarget);
    //        }
    //        else if (tempTarget.MyWeak.bPiercing)
    //        {
    //            damage = (int)((float)damage * 1.5f);
    //            ShowStrongText(tempTarget);
    //        }

    //        tempTarget.TakeDamage(damage);
    //    }
    //    else
    //    {
    //        PlayManager.instance.CreatePopupText("Miss", tempTarget.transform, Color.gray, Vector3.zero);
    //        PlayManager.instance.UpdateAttacked();
    //    }
    //}


    //public void BluntAttack(CombatEntity target)
    //{
    //    Debug.Log("Blunt attack on " + target.name);
    //    tempTarget = target;
    //    _animator.Play(Animator.StringToHash("SwordAttack"));
    //    ActionBarTimer = 0f;
    //    IsMyTurn = false;
    //    //        PlayManager.instance.UnpauseGame();
    //    PlayManager.instance.SendAttackCount(1);
    //    PlayManager.instance.StartingAttack();
    //}

    ////Called by animator. Ensures damage is dealt on the correct attack frame
    //public void EndBluntAttack()
    //{
    //    Debug.Log("Ending blunt attack!");
    //    _audio.PlayOneShot(meleeSFX);
    //    int damage = CalculateAttackDamage(tempTarget);
    //    if (CalculateHit(tempTarget))
    //    {
    //        //check for resistance and weaknesses
    //        if (tempTarget.MyRes.bBlunt)
    //        {
    //            damage = (int)((float)damage * 0.75f);
    //            ShowWeakText(tempTarget);
    //        }
    //        else if (tempTarget.MyWeak.bBlunt)
    //        {
    //            damage = (int)((float)damage * 1.5f);
    //            ShowStrongText(tempTarget);
    //        }
    //        Debug.Log("Sending TakeDamage from blunt!");
    //        tempTarget.TakeDamage(damage);
    //    }
    //    else
    //    {
    //        PlayManager.instance.CreatePopupText("Miss", tempTarget.transform, Color.gray, Vector3.zero);
    //        PlayManager.instance.UpdateAttacked();
    //    }
    //}



    //public void ProjectileAttack(CombatEntity target)
    //{
    //    tempTarget = target;
    //    _animator.Play(Animator.StringToHash("GunAttack"));
    //    ActionBarTimer = 0f;
    //    IsMyTurn = false;
    //    //        PlayManager.instance.UnpauseGame();
    //    PlayManager.instance.SendAttackCount(1);
    //    PlayManager.instance.StartingAttack();
    //}

    ////Called by animator. Ensures damage is dealt on the correct attack frame
    //public void EndProjectileAttack()
    //{
    //    _audio.PlayOneShot(rangedSFX);
    //    int damage = CalculateAttackDamage(tempTarget);
    //    if (CalculateHit(tempTarget))
    //    {
    //        //check for resistance and weaknesses
    //        if (tempTarget.MyRes.bProjectile)
    //        {
    //            damage = (int)((float)damage * 0.75f);
    //            ShowWeakText(tempTarget);
    //        }
    //        else if (tempTarget.MyWeak.bProjectile)
    //        {
    //            damage = (int)((float)damage * 1.5f);
    //            ShowStrongText(tempTarget);
    //        }

    //        tempTarget.TakeDamage(damage);
    //    }
    //    else
    //    {
    //        PlayManager.instance.CreatePopupText("Miss", tempTarget.transform, Color.gray, Vector3.zero);
    //        PlayManager.instance.UpdateAttacked();
    //    }
    //}

    public void Defend()
    {
        Debug.Log(Stats.characterName + " defends");
        RageBarValue += 0.1f;
        MagicBarValue += 0.2f;
        AgilityBarValue += 1f / 3f;
        defending = true;
        ActionBarTimer = 0f;
        IsMyTurn = false;

        // Pretend this was an attack, I guess
        PlayManager.instance.SendAttackCount(1);
        PlayManager.instance.StartingAttack();
        PlayManager.instance.UpdateAttacked();
    }

    /// <summary>
    /// The beginning of a spell casting. It plays the spellcasting animation
    /// and tells PlayManager that an attack is being carried out.
    /// </summary>
    /// <param name="target">The target of the spell.</param>
    public void BeginSpellCast(CombatEntity target, Weapon weapon)
    {
        //_movement.ForceLock(true);
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
        Debug.Log(name + " finishes casting " + MySpell.Method);
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
    public IEnumerator DealSpellDamage(CombatEntity target, GameObject spellVisual, float spellDuration, int damage)
    {
        Debug.Log(name + " casts " + spellVisual.name + "at " + target.name + "!");
        PlayManager.instance.DarkenBG(true);
        yield return new WaitForSeconds(spellDuration);

        target.TakeDamage(damage);
        Destroy(spellVisual);
        PlayManager.instance.DarkenBG(false);
        _animator.SetBool("SpellComplete", true);

        //send this to AI targets
        target.GetComponent<AIBase>().BHold = false;
    }

    public IEnumerator DealSpellHealing(CombatEntity target, GameObject spellVisual, float spellDuration)
    {
        PlayManager.instance.DarkenBG(true);
        yield return new WaitForSeconds(spellDuration);

        int healing = Mathf.FloorToInt(Stats.magic * equipment.primaryWeapon.baseMagicValue + UnityEngine.Random.Range(0, 7));
        target.Heal(healing);
        Destroy(spellVisual);
        PlayManager.instance.DarkenBG(false);
        _animator.SetBool("SpellComplete", true);

        PlayManager.instance.UpdateAttacked();

        //_movement.ForceLock(false);
    }

    public void BoltSpell(CombatEntity target)
    {
        //_animator.Play(Animator.StringToHash("CastSpell"));
        GameObject lightningBolt = Instantiate(Resources.Load("Prefabs/SimpleLightningBoltPrefab"), transform) as GameObject;
        lightningBolt.GetComponent<LineRenderer>().sortingLayerName = "VisualEffects"; // Doing this here because it's not exposed in inspector
        DigitalRuby.LightningBolt.LightningBoltScript lbs = lightningBolt.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
        lbs.StartObject = gameObject;
        lbs.StartPosition = v3spellOrigin;
        lbs.EndObject = target.gameObject;


        //tell enemy target to hold still. Completely disables the AI until it's done

        target.GetComponent<AIBase>().BHold = true;

        int damage = CalculateMagicDamage(tempTarget);

        //check for resistance and weaknesses
        if (target.MyRes.bLightning)
        {
            damage = (int)((float)damage * 0.75f);
            ShowWeakText(tempTarget);
        }
        else if (target.MyWeak.bLightning)
        {
            damage = (int)((float)damage * 1.5f);
            ShowStrongText(tempTarget);
        }

        float duration = lbs.GetComponent<AudioSource>().clip.length * 2;
        StartCoroutine(DealSpellDamage(target, lightningBolt, duration, damage));
        //        PlayManager.instance.UnpauseGame();
    }

    public void GustSpell(CombatEntity target)
    {
        GameObject gust = Instantiate(Resources.Load("Prefabs/Gust")) as GameObject;
        gust.transform.position = target.transform.position;

        //tell enemy target to hold still. Completely disables the AI until it's done
        target.GetComponent<AIBase>().BHold = true;
        //cancel their movement
        target.GetComponent<Movement>().StopForcedMove(false);


        int damage = CalculateMagicDamage(tempTarget);
        //check for resistance and weaknesses
        if (target.MyRes.bWind)
        {
            damage = (int)((float)damage * 0.75f);
            ShowWeakText(tempTarget);
        }
        else if (target.MyWeak.bWind)
        {
            damage = (int)((float)damage * 1.5f);
            ShowStrongText(tempTarget);
        }
        float duration = 1.5f;
        StartCoroutine(DealSpellDamage(target, gust, duration, damage));
    }

    public void CureSpell(CombatEntity target)
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

    public void AimSpell(CombatEntity target)
    {
        // functionality currently unknown
    }

    public void ScorchSpell(CombatEntity target)
    {
        GameObject fireball = Instantiate(
            Resources.Load("Prefabs/Scorch", typeof(GameObject)),
            transform.position + v3spellOrigin,
            Quaternion.identity,
            transform) as GameObject;

        int damage = CalculateMagicDamage(target);

        //check for resistance and weaknesses
        if (target.MyRes.bFire)
        {
            damage = (int)(damage * 0.75f);
            ShowWeakText(tempTarget);
        }
        else if (target.MyWeak.bFire)
        {
            damage = (int)(damage * 1.5f);
            ShowStrongText(tempTarget);
        }

        fireball.GetComponent<EffectSettings>().Target = target.gameObject;
        fireball.GetComponent<EffectSettings>().MoveDistance = Vector3.Distance(transform.position, target.transform.position);
        float duration = fireball.GetComponent<AudioSource>().clip.length;
        StartCoroutine(DealSpellDamage(target, fireball, duration, damage));
    }

    public void FrostSpell(CombatEntity target)
    {
        GameObject frost = Instantiate(
            Resources.Load("Prefabs/Frost", typeof(GameObject)),
            transform.position + v3spellOrigin,
            Quaternion.identity,
            transform) as GameObject;

        int damage = CalculateMagicDamage(target);

        //check for resistance and weaknesses
        if (target.MyRes.bIce)
        {
            damage = (int)(damage * 0.75f);
            ShowWeakText(tempTarget);
        }
        else if (target.MyWeak.bIce)
        {
            damage = (int)(damage * 1.5f);
            ShowStrongText(tempTarget);
        }

        frost.GetComponent<EffectSettings>().Target = target.gameObject;
        frost.GetComponent<EffectSettings>().MoveDistance = Vector3.Distance(transform.position, target.transform.position);
        float duration = frost.GetComponent<AudioSource>().clip.length;
        StartCoroutine(DealSpellDamage(target, frost, duration, damage));
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
        if (this.GetComponent<Movement>().CheckForceLock) { return; }


        //		Debug.Log (PlayManager.instance.party[0].gameObject.name.ToString());
        //Only the character in the lead should be able to instigate things
        if (PlayManager.instance.party[0].gameObject == this.gameObject)
        {
            if (other.gameObject.tag.Equals("Battleground"))
            {
                other.gameObject.GetComponent<Battleground>().Begin();
            }

            if (other.gameObject.tag.Equals("AreaTransition"))
            {
                Debug.Log("Transition to new area!");
                other.gameObject.GetComponent<AreaTransition>().Begin();
            }

            if (other.gameObject.tag.Equals("CM_Trigger"))
            {
                Debug.Log("CM_Trigger found");
                other.gameObject.GetComponent<CM_Trigger>().Activate();
            }
        }
    }

    public void AddExperience(int xp)
    {
        ExperienceTotal += xp;
    }

    public void TurnCoat()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError(name + " can't become an enemy");
            return;
        }
        GetComponent<Movement>().bNPC = true;
        enemy.enabled = true;
        this.enabled = false;
    }

    #region implemented abstract members of Entity

    public override void Interact()
    {
        throw new NotImplementedException();
    }

    public override void Die()
    {
        Debug.Log(gameObject.name + " Died!");

        _audio.PlayOneShot(deathSFX);

        // TODO: Put player in KO state
        // Animate falling down   
        _animator.Play(Animator.StringToHash("Death"));
        PlayManager.instance.UpdateAttacked();
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

    public Weapon ActiveWeapon
    {
        get
        {
            return activeWeapon;
        }

        set
        {
            activeWeapon = value;
        }
    }
    #endregion
}
