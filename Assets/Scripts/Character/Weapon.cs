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

    [CreateAssetMenu]
    public class Weapon : Equipment
    {        
        public WeaponType type;
        public float baseAttackValue;
        public float baseMagicValue;
        public List<Spell> abilities;
        public AbilityLearningLevel learningLevel;        
    }
}
