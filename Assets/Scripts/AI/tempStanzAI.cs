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
				Entity target = person.GetComponent<Entity> ();

//				if (randB < 0.5f) {
				if(!bToggle){
					_player.BoltSpell (target);
					bToggle = true;
					Debug.Log ("Stanz casts bolt!");
				} else {
					_player.FireBreath(target);
					bToggle = false;
					Debug.Log ("Stanz casts fire breath!");
				}

			}
		}
	}
}
