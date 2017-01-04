using UnityEngine;
using System.Reflection;
using System;
using Artifice.Interfaces;

namespace Artifice.Items
{
    /// <summary>
    /// Potions class that extends items. Potions have targets that they can effect
    /// </summary>
    public class Potion : Item, IXML {
        protected string methodGroupString;
        protected string effectAmountString;

        protected string[] methodGroups;
        protected float[] effectAmounts;

        public Potion() : base() {
            methodGroups = new string[] { "BasicHealing" };
            effectAmounts = new float[] { 10 };
        }

        public Potion(string _id, string _title, string _description, RarityLevel _rarity, int _worth, string _methodGroups, string _effectAmounts) : base(_id, _title, _description, _rarity, _worth) {
            _methodGroups = _methodGroups.Replace(" ", "");
            methodGroups = _methodGroups.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            _effectAmounts = _effectAmounts.Replace(" ", "");
            effectAmounts = Array.ConvertAll(_effectAmounts.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries), float.Parse);
        }

        /// <summary>
        /// Generic potion use that works for all potions. Uses reflection to find the method name based on the method group (string)
        /// assigned to it.
        /// </summary>
        /// <param name="targets">The targets to affect</param>
        public void UsePotion(object[] targets) {
            // Grab the method from by the potion's method groups
            for(int j = 0; j < methodGroups.Length; j++) {
                MethodInfo method = typeof(Potion).GetMethod(methodGroups[j]);
                if (method != null) {
                    for (int i = 0; i < targets.Length; i++) {
                        // Invoke the found method on each target
                        method.Invoke(this, new object[] { targets[i], effectAmounts[j] });
                    }
                }
            }
        }

        /// <summary>
        /// Heals the target by the effect amount of the potion
        /// </summary>
        /// <param name="target">The target to heal</param>
        public void BasicHealing(GameObject target, float effectAmount) {
            Debug.Log(target.name + " healed by " + effectAmount + " points!");
        }

        /// <summary>
        /// Heals the target by the effect amount of the potion and is advanced
        /// </summary>
        /// <param name="target">The target to heal</param>
        public void AdvancedHealing(GameObject target, float effectAmount) {
            Debug.Log(target.name + " healed by " + effectAmount + " points, but advancedly!");
        }

        public new string ID {
            get { return id; }
            set { id = value; }
        }

        public new string Title {
            get { return title; }
            set { title = value; }
        }

        public new string Description {
            get { return description; }
            set { description = value; }
        }

        public new RarityLevel Rarity {
            get { return rarity; }
            set { rarity = value; }
        }

        public new int Worth {
            get { return worth; }
            set { worth = value; }
        }

        public string MethodGroups {
            get { return methodGroupString; }
            set { methodGroupString = value; }
        }

        public string EffectAmounts {
            get { return effectAmountString; }
            set { effectAmountString = value; }
        }

        public new string ResourcesDir {
            get { return "Files/Potions"; }
        }
    } 
}
