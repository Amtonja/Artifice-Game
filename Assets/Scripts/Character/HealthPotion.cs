using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artifice.Characters
{
    [CreateAssetMenu]
    public class HealthPotion : Consumable
    {
        public int amount;
        public bool isPercentage;

        public override void OnUse(CombatEntity target)
        {
            if (!isPercentage)
            {
                target.Heal(amount);
            }
            else
            {
                float percentage = amount / 100f;
                target.Heal(Mathf.FloorToInt(target.Stats.maxHealth * percentage));
            }

            Debug.Log("Potion used on " + target.Name);
        }
    }
}
