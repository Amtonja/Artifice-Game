using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters {
	public class tempKomodoAI : MonoBehaviour {

		//Temporary komodo script

		private Player _player; //ref to our Player script

		// Use this for initialization
		void Start () {
			_player = GetComponent<Player> ();
		}
		
		// Update is called once per frame
		void Update () {
			if (_player.IsMyTurn) {
				Debug.Log (this.gameObject.name.ToString () + "'s turn!");
				GameObject evans = GameObject.Find ("Evans");
				Entity target = evans.GetComponent<Entity> ();
				_player.MeleeAttack (target);


			}
		}
	}
}
