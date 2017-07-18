using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice
{
    public enum PhysicalDamageType
    {
        Blunt, Piercing, Projectile
    }

    [CreateAssetMenu]
    public class CharacterStats : ScriptableObject
    {
        public string characterName;
        public int startingLevel;

        public int attack, defense, magic, magicDefense, speed, maxHealth;

        public PhysicalDamageType primaryDamage, secondaryDamage, damageWeakness;

        public List<SubAction> spells;
    }
}
