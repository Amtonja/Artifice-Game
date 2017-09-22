using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters 
{
	public class ArcaneyeAI : AIBase //MonoBehaviour 
	{

		//Temporary republic eye script

		//		private Player _player; //ref to our Player script
		//
		//
		//		// Use this for initialization
		//		void Start () {
		//			_player = GetComponent<Player> ();
		//		}

		private float waitForWander = 2f; //Need to wait for combat to be ready before wandering
		private float waitForWanderCurrent = 0f;

		private bool bHold = false;

		// Update is called once per frame
		public override void CombatUpdate () {
			if (_enemy.IsMyTurn) {
				Debug.Log (gameObject.name.ToString () + "'s turn!");

//				float randA = Random.value; //which target

				GameObject person;
//				if (randA < 0.3f) {
//					person = GameObject.Find ("Evans");
//				} else if (randA < 0.6f) {
					person = GameObject.Find ("Hurley");
//				} else {
//					person = GameObject.Find ("Russo");
//				}
				CombatEntity target = person.GetComponent<CombatEntity> ();

				//				_player.MeleeAttack (target);
				//_player.MyCombatAction = _player.MeleeAttack; 
				_enemy.MySpell = _enemy.EyeLaser;
				_enemy.MyCombatAction = _enemy.BeginSpellCast;

				Debug.Log("Target = " + target.name.ToString());

				_enemy.MyCombatAction(target);
				bHold = true;
				waitForWanderCurrent = 0;

			}else if (!bHold){
//				waitForWanderCurrent += Time.deltaTime;
//				if (waitForWanderCurrent >= waitForWander) {
//					Wander ();
//				}
			}
		}


		public override void ResumeWander(){
			bHold = false;
		}
	}
}
