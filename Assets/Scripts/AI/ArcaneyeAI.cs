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

		// Update is called once per frame
		public override void CombatUpdate () {
			if (_player.IsMyTurn) {
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
				Entity target = person.GetComponent<Entity> ();

				//				_player.MeleeAttack (target);
				//_player.MyCombatAction = _player.MeleeAttack; 
				_player.MySpell = _player.EyeLaser;
				_player.MyCombatAction = _player.BeginSpellCast;

				_player.MyCombatAction(target);

			}else {
				waitForWanderCurrent += Time.deltaTime;
				if (waitForWanderCurrent >= waitForWander) {
					Wander ();
				}
			}
		}
	}
}
