using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice.Characters
{
    public enum WeaponType
    {
        SWORD, 
        RIFLE,
        PISTOL,
        GAUNTLETS,
        LASH,
        DAGGER,
        BOW,
        SPEAR,
        STAFF
    }

    public enum AbilityLearningLevel
    {
        LOW,
        MEDIUM,
        MEDIUM_HIGH,
        HIGH,
        VERY_HIGH
    } 
    
    public enum DamageType
    {
        PIERCING,
        BLUNT,
        PROJECTILE
    }   

    [CreateAssetMenu]
    public class Weapon : Equipment
    {        
        public WeaponType type;
        public DamageType damageType;
        public float baseAttackValue;
        public float baseMagicValue;
        public List<CombatAction> abilities;        
        public AbilityLearningLevel learningLevel;

        // Variables that vary per instance
        public System.Guid guid;
        public int XP;
        public List<CombatAction> unlockedAbilities;
        public float attackValue;
        public float magicValue;

        public void ResetStats()
        {
            XP = 0;
            attackValue = baseAttackValue;
            magicValue = baseMagicValue;
            unlockedAbilities = new List<CombatAction>();
        }
    }
}
