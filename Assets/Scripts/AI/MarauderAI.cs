using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{
	public class MarauderAI : AIBase//MonoBehaviour
	{

		//Temporary komodo script

//		private Player _player; //ref to our Player script
//
//		// Use this for initialization
//		void Start()
//		{
//			_player = GetComponent<Player>();
//		}

		private float waitForWander = 2f; //Need to wait for combat to be ready before wandering
		private float waitForWanderCurrent = 0f;

		private bool bHoldPos = false;

		// Update is called once per frame
		public override void CombatUpdate()
		{
			if (_enemy.IsMyTurn)
			{

				//Cancel forcedMove
				_movement.StopForcedMove(false);

				Debug.Log(gameObject.name.ToString() + "'s turn!");

				float randA = Random.value; //which target
				float randB = Random.value; //which attack

				GameObject person;
				if (randA < 0.3f) {
					person = GameObject.Find ("Evans");
				} else if (randA < 0.6f) {
					person = GameObject.Find ("Hurley");
				} else {
					person = GameObject.Find ("Russo");
				}
				CombatEntity target = person.GetComponent<CombatEntity>();

				if (randB < 0.5f) {
                    _enemy.ActiveWeapon = GetComponent<Gear>().primaryWeapon;
					_enemy.MyCombatAction = _enemy.WeaponAttack;
				} else {
                    _enemy.ActiveWeapon = GetComponent<Gear>().secondaryWeapon;
					_enemy.MyCombatAction = _enemy.WeaponAttack;
				}


				_enemy.MyCombatAction(target, _enemy.ActiveWeapon);

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
