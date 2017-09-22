using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{
    public abstract class CombatEntity : MonoBehaviour, IIdentifiable, IInteractable, IHealth<int>, ICombatable
    {
        protected string title;
        protected string id;
        protected bool canInteract;
        protected bool inCombat;
        protected bool defending;
        protected int health;
        protected CharacterRace race;
        protected int experienceTotal;
        protected int characterLevel;
        protected float actionBarValue = 0f;
        protected float actionBarTarget;
        protected float actionBarDefaultTarget = 6f;
        private int defenseValue, magicDefenseValue;
        private bool isMyTurn = false;

        public Vector2 spellOrigin;
        protected Vector3 v3spellOrigin;

        protected CombatAction _combatAction;
        protected SpellDelegate _spell;

        protected Animator _animator;
        
        public AudioClip meleeSFX, rangedSFX, footstepSFX, takeDamageSFX, deathSFX;
        protected AudioSource _audio;

        //public bool bIsEnemy = false; //Used for AI

        private bool healthChanged = false;

        public CharacterStats baseStats;
        private CharacterStats currentStats;

        public abstract void Interact();        

        protected float alphaColor = 1.0f; //for temporary blink

		//Damage resistance types for this character. [System.Serializable] to expose to editor for testing purposes.
		[System.Serializable]
		public struct Resistances {
			public bool bBlunt;
			public bool bPiercing;
			public bool bProjectile;

			public bool bWind;
			public bool bFire;
			public bool bLightning;
			public bool bIce;
			public bool bForce;
		}

		public Resistances myRes;

		[System.Serializable]
		public struct Weaknesses {
			public bool bBlunt;
			public bool bPiercing;
			public bool bProjectile;

			public bool bWind;
			public bool bFire;
			public bool bLightning;
			public bool bIce;
			public bool bForce;
		}


		public Weaknesses myWeak;


        public virtual void TakeDamage(int _damage)
        {
            HealthChanged = true;
            if (_damage < 0) { _damage = 0; }
            if (defending)
            {
                _damage /= 2;
            }
            health -= _damage;
            health = Mathf.Clamp(health, 0, Stats.maxHealth);
//            Debug.Log(Stats.characterName + " took " + _damage + " and is at " + health + " HP out of " + Stats.maxHealth + " HP");
            PlayManager.instance.CreatePopupText(_damage.ToString(), transform, Color.red, Vector3.zero);
            if (health <= 0) Die();

            alphaColor = 0.1f;
        }

        public virtual void Heal(int _health)
        {
            HealthChanged = true;
            if (health <= 0)
            {
                Revive();
            }

            health += _health;
            health = Mathf.Clamp(health, 0, Stats.maxHealth);
            PlayManager.instance.CreatePopupText(_health.ToString(), transform, Color.green, Vector3.zero);
        }

		//Quietly resets to max health without a popup. Useful for, say, resetting Evans' health before he turns on his party in Oran
		public virtual void ResetHealth(){
			health = Stats.maxHealth;
		}

        public abstract void Die();

        public abstract void Revive();

        public virtual void EnterCombat()
        {
            ActionBarTimer = 0f;
            inCombat = true;
        }

        public virtual void ExitCombat()
        {
            ActionBarTimer = 0f;
            inCombat = false;
        }

        /// <summary>
        /// Sets all combat stats equal to their base values.
        /// Used for initialization or when all modifying effects are removed.
        /// </summary>
        protected void ResetStats()
        {
            Stats = Instantiate(baseStats);
        }

        protected void PlayFootstepSFX()
        {
            _audio.PlayOneShot(footstepSFX);
        }

        public void ShowWeakText(CombatEntity target)
        {
            PlayManager.instance.CreatePopupText("Weak", target.transform, Color.blue, Vector3.down);
        }

        public void ShowStrongText(CombatEntity target)
        {
            PlayManager.instance.CreatePopupText("Strong", target.transform, Color.red, Vector3.down);
        }

        /// <summary>
        /// Determine if an attack will hit by comparing accuracy vs. evasion
        /// and a random number
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected bool CalculateHit(CombatEntity target)
        {
            bool isAHit = true;

            int attackRoll = UnityEngine.Random.Range(1, 100);

            if (attackRoll <= 5)
            {
                isAHit = false;
                Debug.Log(Stats.characterName + " missed!");
            }

            return isAHit;
        }

        public delegate void CombatAction(CombatEntity target);
        public delegate void SpellDelegate(CombatEntity target);

        #region C# Properties
        public string Name
        {
            get { return title; }
            set { title = value; }
        }
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public bool CanInteract
        {
            get { return canInteract; }
            set { canInteract = value; }
        }

        public bool InCombat
        {
            get { return inCombat; }
            set { inCombat = value; }
        }

        public CharacterStats Stats
        {
            get { return currentStats; }
            set { currentStats = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public CharacterRace Race
        {
            get { return race; }
            set { race = value; }
        }

        public bool HealthChanged
        {
            get
            {
                return healthChanged;
            }

            set
            {
                healthChanged = value;
            }
        }

        public float ActionBarTimer
        {
            get
            {
                return actionBarValue;
            }

            set
            {
                actionBarValue = value;
            }
        }

        public float ActionBarTargetTime
        {
            get
            {
                return actionBarTarget;
            }

            set
            {
                actionBarTarget = value;
            }
        }

        public bool IsMyTurn
        {
            get
            {
                return isMyTurn;
            }

            set
            {
                isMyTurn = value;                
            }
        }

        public int DefenseValue
        {
            get
            {
                return defenseValue;
            }

            set
            {
                defenseValue = value;
            }
        }

        public int MagicDefenseValue
        {
            get
            {
                return magicDefenseValue;
            }

            set
            {
                magicDefenseValue = value;
            }
        }


		public Resistances MyRes {
			get
			{
				return myRes;
			}
		}

		public Weaknesses MyWeak{
			get
			{
				return myWeak;
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

        public int ExperienceTotal
        {
            get
            {
                return experienceTotal;
            }

            set
            {
                experienceTotal = value;
            }
        }

        public int CharacterLevel
        {
            get
            {
                return characterLevel;
            }

            set
            {
                characterLevel = value;
            }
        }
        #endregion
    }
}