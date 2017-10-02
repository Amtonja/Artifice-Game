using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Characters;

[RequireComponent(typeof(Gear), typeof(Animator), typeof(AudioSource))]
public class Enemy : CombatEntity
{
    //public float weaponClass, armorClass;
    private Gear equipment;
    private Weapon activeWeapon;
    
    // Use this for initialization
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        //inventory = GetComponent<Inventory>();
        equipment = GetComponent<Gear>();

        ResetStats();
        //Stats.maxHealth += equipment.armor.healthValue;
        health = Stats.maxHealth;
        DefenseValue = Mathf.FloorToInt(Stats.defense * equipment.armor.armorValue * 0.8f);
        MagicDefenseValue = Mathf.FloorToInt(Stats.magicDefense * equipment.armor.magicValue * 0.8f);

        // The time for a character's action bar to fill is equal to the default time minus a percentage equal to their Speed stat
        ActionBarTargetTime = actionBarDefaultTarget - (actionBarDefaultTarget * Stats.speed / 100f);

        v3spellOrigin = new Vector3(spellOrigin.x * transform.localScale.x, spellOrigin.y * transform.localScale.y, 0f);

        equipment.primaryWeapon.ResetStats();
        equipment.secondaryWeapon.ResetStats();
        ActiveWeapon = equipment.primaryWeapon;        
    }

    // Update is called once per frame
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
        if(!enabled)
        {
            return;
        }
        else if (CalculateHit(tempTarget))
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
        GetComponent<AIBase>().ResumeWander();
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
    //    GetComponent<AIBase>().ResumeWander();
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
    //    GetComponent<AIBase>().ResumeWander();
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
    //    this.GetComponent<AIBase>().ResumeWander();
    //}

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
        //target.GetComponent<AIBase>().BHold = false;
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

        PlayManager.instance.UpdateAttacked();
        PlayManager.instance.RemoveEnemy(this);
    }

    public void EyeLaser(CombatEntity target) //I'm assuming this uses Force?
    {
        GameObject eyeLaser = Instantiate(
            Resources.Load("Prefabs/EyeLaser", typeof(GameObject)),
            transform.position + v3spellOrigin,
            Quaternion.identity,
            transform) as GameObject;

        int damage = CalculateMagicDamage(target);

        //check for resistance and weaknesses
        if (target.MyRes.bForce)
        {
            damage = (int)(damage * 0.75f);
            ShowWeakText(tempTarget);
        }
        else if (target.MyWeak.bForce)
        {
            damage = (int)(damage * 1.5f);
            ShowStrongText(tempTarget);
        }

        eyeLaser.GetComponent<EffectSettings>().Target = target.gameObject;
        eyeLaser.GetComponent<EffectSettings>().MoveDistance = Vector3.Distance(transform.position, target.transform.position);
        float duration = eyeLaser.GetComponent<AudioSource>().clip.length;
        StartCoroutine(DealSpellDamage(target, eyeLaser, duration, damage));        
    }

    public void FireBreath(CombatEntity target)
    {
        Quaternion rotToTarget = Quaternion.LookRotation(target.transform.position - transform.position);
        GameObject firebreath = Instantiate(
            Resources.Load("Prefabs/FireBreath", typeof(GameObject)),
            transform.position + v3spellOrigin,
            rotToTarget,
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
            damage = (int)((float)damage * 1.5f);
            ShowStrongText(tempTarget);
        }

        float duration = firebreath.GetComponent<AudioSource>().clip.length;
        StartCoroutine(DealSpellDamage(target, firebreath, duration, damage));
    }

    #region implemented abstract members of Entity

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        Debug.Log(gameObject.name + " Died!");

        _audio.PlayOneShot(deathSFX);

        PlayManager.instance.experiencePool += Stats.xpValue;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOut(sr));
    }

    public override void Revive()
    {
        Debug.Log(gameObject.name + " was revived!");

        _animator.Play(Animator.StringToHash("Intro"));
    }

    public override void EnterCombat()
    {
        _animator.Play(Animator.StringToHash("Intro"));
        base.EnterCombat();
    }

    public override void ExitCombat()
    {
        base.ExitCombat();
    }

    public override void TakeDamage(int _damage)
    {
        this.GetComponent<AIBase>().Stun();

        base.TakeDamage(_damage);
        _audio.PlayOneShot(takeDamageSFX);
    }

    #endregion

    #region C# Properties
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
