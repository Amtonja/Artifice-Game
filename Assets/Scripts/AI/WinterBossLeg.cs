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

		public GameObject dad; //okay GetComponentInParent is returning this object's component so I'm cheating here

		//Called by WinterBossAI
		public void LegAttack(){
			Player person;

			int partylength = PlayManager.instance.party.Length; //might need to be -1
			//Characters[Mathf.RoundtoInt(Random.Range(Characters.length))}
			person = PlayManager.instance.party[Mathf.RoundToInt(Random.Range(0,partylength))];

			CombatEntity target = person.GetComponent<CombatEntity> ();
			Debug.Log ("Target name is " + target.name.ToString ());

			_enemy.MyCombatAction = _enemy.WeaponAttack;                    


			Debug.Log ("Target = " + target.name.ToString ());
			_enemy.ActiveWeapon = _enemy.GetComponent<Gear> ().primaryWeapon;
			_enemy.MyCombatAction (target, _enemy.ActiveWeapon);
			dad.GetComponent<Enemy> ().IsMyTurn = false;
			Debug.Log("Dad's ismyturn! is " + dad.GetComponent<Enemy>().IsMyTurn.ToString());
			dad.GetComponent<WinterBossAI> ().bPause = false; //we need this or above attack will not resolve, it'll clip the animation each frame
			Debug.Log("Dad's bPause! is " + dad.GetComponent<WinterBossAI>().bPause.ToString());
			dad.GetComponent<Enemy> ().ActionBarTimer = 0f;

		}
	}
}