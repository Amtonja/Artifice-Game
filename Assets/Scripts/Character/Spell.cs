using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice.Characters
{
    [CreateAssetMenu]
    public class Spell : CombatAction
    {
        public MagicDamageType damageType;
        public GameObject visualEffect;
        public AudioSource soundEffect;
    }
}