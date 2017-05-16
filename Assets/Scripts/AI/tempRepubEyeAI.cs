using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters {
	public class tempRepubEyeAI : MonoBehaviour {

		//Temporary republic eye script

		private Player _player; //ref to our Player script


		// Use this for initialization
		void Start () {
			_player = GetComponent<Player> ();
		}

		// Update is called once per frame
		void Update () {
			if (_player.IsMyTurn) {
				Debug.Log (gameObject.name.ToString () + "'s turn!");

				float randA = Random.value; //which target

				GameObject person;
				if (randA < 0.5f) {
					person = GameObject.Find ("Evans");
				} else {
					person = GameObject.Find ("Hurley");
				}
				Entity target = person.GetComponent<Entity> ();

//				_player.MeleeAttack (target);
				_player.MyCombatAction = _player.MeleeAttack; 

				_player.MyCombatAction(target);

			}
		}
	}
}
