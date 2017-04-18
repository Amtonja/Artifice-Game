using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters {
    public abstract class Entity : MonoBehaviour, IIdentifiable, IInteractable, IHealth<int>, ICombatable {
        protected string title;
        protected string id;
        protected bool canInteract;
        protected bool inCombat;
        protected CombatStats stats;
        protected int health;
        protected CharacterRace race;
        private bool healthChanged = false;

        public abstract void Interact();

        public virtual void TakeDamage(int _damage) {
            HealthChanged = true;
            health -= _damage;
            //Debug.Log(title + "took " + _damage + " and is at " + health + " HP out of " + stats.MaxHealth + " HP");
            if (health <= 0) Die();
        }

        public virtual void Heal(int _health) {
            HealthChanged = true;
            health = Mathf.Clamp(health, 0, stats.MaxHealth);
        }

        public abstract void Die();

        public virtual void EnterCombat() {
            inCombat = true;
        }

        public virtual void ExitCombat() {
            inCombat = false;
        }

        #region C# Properties
        public string Name {
            get { return title; }
            set { title = value; }
        }
        public string ID {
            get { return id; }
            set { id = value; }
        }

        public bool CanInteract {
            get { return canInteract; }
            set { canInteract = value; }
        }

        public bool InCombat {
            get { return inCombat; }
            set { inCombat = value; }
        }

        public CombatStats Stats {
            get { return stats; }
        }

        public int Health {
            get { return health; }
        }

        public CharacterRace Race {
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
        #endregion
    }
}