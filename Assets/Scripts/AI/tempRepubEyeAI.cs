using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters 
{
	public class tempRepubEyeAI : AIBase //MonoBehaviour 
	{

		//Temporary republic eye script

//		private Player _player; //ref to our Player script
//
//
//		// Use this for initialization
//		void Start () {
//			_player = GetComponent<Player> ();
//		}

		private float waitForWander = 3f; //Need to wait for combat to be ready before wandering
		private float waitForWanderCurrent = 0f;

		private bool bHoldPos = false;

		// Update is called once per frame
		public override void CombatUpdate () {
			if (_player.IsMyTurn) {
				Debug.Log (gameObject.name.ToString () + "'s turn!");

				float randA = Random.value; //which target

				GameObject person;
				if (randA < 0.3f) {
					person = GameObject.Find ("Evans");
					Debug.Log ("Scorpio attacking Evans!");
				} else if (randA < 0.6f) {
					person = GameObject.Find ("Hurley");
					Debug.Log ("Scorpio attacking Hurley!");
				} else {
					person = GameObject.Find ("Russo");
					Debug.Log ("Scorpio attacking Russo!");
				}
				Entity target = person.GetComponent<Entity> ();

				//				_player.MeleeAttack (target);
				//_player.MyCombatAction = _player.MeleeAttack; 
				_player.MySpell = _player.EyeLaser;
				_player.MyCombatAction = _player.BeginSpellCast;

				Debug.Log("Target = " + target.name.ToString());

				_player.MyCombatAction(target);

				waitForWanderCurrent = 0;

				//Hold until animation is complete
				bHoldPos = true;

			}else if (!bHoldPos){
				waitForWanderCurrent += Time.deltaTime;
				if (waitForWanderCurrent >= waitForWander) {
					Wander ();
				}
			}
		}


		public override void ResumeWander(){
			bHoldPos = false;
			waitForWanderCurrent = 0;
		}
	}
}
