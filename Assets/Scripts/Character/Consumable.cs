using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice.Characters
{    
    public abstract class Consumable : Item
    {
        public abstract void OnUse(CombatEntity target);
    }
}
