using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice
{
    public enum MagicDamageType
    {
        Wind, Lightning, Fire, Ice, Force
    }

    [CreateAssetMenu]
    public class Spell : ScriptableObject
    {
        public string spellName = "New Spell";
        public string methodName;

        public MagicDamageType damageType;
        public GameObject visualEffect;
        public AudioSource soundEffect;
    }
}