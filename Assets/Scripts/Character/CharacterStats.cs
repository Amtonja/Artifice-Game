using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice.Characters
{
    [CreateAssetMenu]
    public class CharacterStats : ScriptableObject
    {
        public string characterName;
        public int startingLevel;

        public int attack, defense, magic, magicDefense, speed, maxHealth, xpValue;

        public PhysicalDamageType primaryDamage, secondaryDamage, damageWeakness;

        public List<Spell> spells;
    }
}
