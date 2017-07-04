using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{
	public class ScorpioAI : AIBase//MonoBehaviour
	{

		//Temporary komodo script

//		private Player _player; //ref to our Player script
//
//		// Use this for initialization
//		void Start()
//		{
//			_player = GetComponent<Player>();
//		}

		// Update is called once per frame
		public override void CombatUpdate()
		{
			if (_player.IsMyTurn)
			{
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
				Entity target = person.GetComponent<Entity>();

				//                if (randB < 0.5f)
				//                {
				//                    //_player.MeleeAttack(target);
				_player.MyCombatAction = _player.MeleeAttack;                    
				//                }
				//                else
				//                {
				//_player.FireBreath(target);
//				_player.MySpell = _player.FireBreath;
//				_player.MyCombatAction = _player.BeginSpellCast;
				//                }

				_player.MyCombatAction(target);
			}
		}
	}
}
