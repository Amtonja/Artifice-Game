using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{

	public class WinterBossAI : AIBase {

		public GameObject leg; 
		
		public override void CombatUpdate()
		{
			if (_enemy.IsMyTurn) {
				Debug.Log (gameObject.name.ToString () + "'s turn!");

				//Cancel forcedMove
				_movement.StopForcedMove (false);

				leg.GetComponent<WinterBossLeg> ().LegAttack ();

			}

		}
	}
}