using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{

	public class WinterBossAI : AIBase {

		public GameObject leg; 

		public bool bPause = false;

		private float pauseTimer = 0f;
		private float pauseTimerMax = 1f;
		
		public override void CombatUpdate()
		{
			//There's a weird timing issue for just turning bPause off so lets just do this instead
			if (bPause) {
				pauseTimer += Time.deltaTime;
				if (pauseTimer >= pauseTimerMax) {
					pauseTimer = 0f;
					bPause = false;
				}

			}

			if (_enemy.IsMyTurn && !bPause) {
				Debug.Log (gameObject.name.ToString () + "'s turn!");

				//Cancel forcedMove
				_movement.StopForcedMove (false);

				leg.GetComponent<WinterBossLeg> ().LegAttack ();
				bPause = true;

			}

		}
	}
}