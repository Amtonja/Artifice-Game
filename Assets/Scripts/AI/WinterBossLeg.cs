using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{
	public class WinterBossLeg : AIBase {

		//Exists as an intermediary between the WinterBossAI and it's leg.

	//	public void EndWeaponAttack(){
	////		this.GetComponentInParent<Enemy> ().EndWeaponAttack ();
	//
	//	}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		//Called by WinterBossAI
		public void LegAttack(){
			Player person;

			int partylength = PlayManager.instance.party.Length; //might need to be -1
			//Characters[Mathf.RoundtoInt(Random.Range(Characters.length))}
			person = PlayManager.instance.party[Mathf.RoundToInt(Random.Range(0,partylength))];

			CombatEntity target = person.GetComponent<CombatEntity> ();

			_enemy.MyCombatAction = _enemy.WeaponAttack;                    


			Debug.Log ("Target = " + target.name.ToString ());
			_enemy.ActiveWeapon = _enemy.GetComponent<Gear> ().primaryWeapon;
			_enemy.MyCombatAction (target, _enemy.ActiveWeapon);


		}
	}
}