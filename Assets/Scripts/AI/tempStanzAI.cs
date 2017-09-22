using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Artifice.Interfaces;

namespace Artifice.Characters {
	public class tempStanzAI : MonoBehaviour {

		private Player _player; //ref to our Player script

		private bool bToggle = false; //just toggles between attacks

		// Use this for initialization
		void Start () {
			_player = GetComponent<Player> ();
		}

//		private Entity temptarget;
		// Update is called once per frame
		void Update () {
			if (_player.IsMyTurn) {
				Debug.Log (gameObject.name.ToString () + "'s turn!");

				float randA = Random.value; //which target
//				float randB = Random.value; //which attack

				GameObject person;
				if (randA < 0.5f) {
					person = GameObject.Find ("Evans");

				} else {
					person = GameObject.Find ("Hurley");
				}
				CombatEntity target = person.GetComponent<CombatEntity> ();
//				temptarget = target;

				if(!bToggle){
//					_player.BoltSpell (target);
					_player.MySpell = _player.BoltSpell;
					_player.MyCombatAction = _player.BeginSpellCast;
					bToggle = true;
					Debug.Log ("Stanz casts bolt!");
				} else {
//					_player.FireBreath(target);
					_player.MySpell = _player.FireBreath;
					_player.MyCombatAction = _player.BeginSpellCast;
					bToggle = false;
					Debug.Log ("Stanz casts fire breath!");
				}
//				_player.MyCombatAction = _player.MeleeAttack; 

				_player.MyCombatAction(target);

			}
		}
	}
}
