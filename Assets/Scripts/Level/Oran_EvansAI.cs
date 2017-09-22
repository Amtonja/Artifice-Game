using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{
	public class Oran_EvansAI : AIBase {

		//Typical AI, except it counts how many rounds he's had. After five attacks

		private int turnMax = 5; //How many turns evans should fight before canceling the fight to continue with the cutscene event chain
		private int currentTurn = 0;

		public GameObject passTarget; //for next target

		public override void CombatUpdate()
		{
			if (_enemy.IsMyTurn) {

				currentTurn++;
				if (currentTurn == turnMax) {
					Debug.Log ("Evans fight over!");


					PlayManager.instance.CancelCombat ();

					this.GetComponent<Movement>().bNPC = false; //just in case. I mean player movement input shouldn't happen at this point but whatever

					//send activation pulse
					passTarget.SendMessage("Activate");

					Destroy (this); //removes this component
					return;

				}
				Debug.Log (gameObject.name.ToString () + "'s turn!");

				float randA = Random.value; //which target
				float randB = Random.value; //which attack

				GameObject person;
				if (randA < 0.5f) {
					person = GameObject.Find ("Hurley");
				} else {
					person = GameObject.Find ("Russo");
				}
				CombatEntity target = person.GetComponent<CombatEntity> ();

				if (randB < 0.5f) {
					//                    //_player.MeleeAttack(target);
					_enemy.MyCombatAction = _enemy.PiercingAttack;
				} else {
					_enemy.MyCombatAction = _enemy.ProjectileAttack;
				}

				_enemy.MyCombatAction(target);

			}
				
		}
	}

}
