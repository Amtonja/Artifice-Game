using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{
    public abstract class Entity : MonoBehaviour, IIdentifiable, IInteractable, IHealth<int>, ICombatable
    {
        protected string title;
        protected string id;
        protected bool canInteract;
        protected bool inCombat;       
        protected int health;
        protected CharacterRace race;
        protected float actionBarValue = 0f;
        protected float actionBarTarget;
        private int defenseValue, magicDefenseValue;
        private bool isMyTurn = false;

		public bool bIsEnemy = false; //Used for AI

        private bool healthChanged = false;

        public CharacterStats baseStats;
        private CharacterStats currentStats;

        public abstract void Interact();        

        protected float alphaColor = 1.0f; //for temporary blink

		//Damage resistance types for this character. [System.Serializable] to expose to editor for testing purposes.
		[System.Serializable]
		public struct Resistances {
			public bool bBlunt;
			public bool bPeircing;
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
			public bool bPeircing;
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
            health -= _damage;
            health = Mathf.Clamp(health, 0, Stats.maxHealth);
            Debug.Log(Stats.characterName + " took " + _damage + " and is at " + health + " HP out of " + Stats.maxHealth + " HP");
            PlayManager.instance.CreatePopupText(_damage.ToString(), transform, Color.red);
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
            PlayManager.instance.CreatePopupText(_health.ToString(), transform, Color.green);
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
        #endregion
    }
}