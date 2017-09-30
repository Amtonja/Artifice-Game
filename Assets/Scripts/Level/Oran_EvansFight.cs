using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Characters
{

	public class Oran_EvansFight : MonoBehaviour {

		//Turn evans into an enemy - remove from party, make him an npc, attach Oran_EvansAI to him, set his max HP to something absurd


		public Battleground evansBattleground;

		public GameObject passTarget;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
//			if (Input.GetKeyDown (KeyCode.Space)) {
//				Begin ();
//			}
		}


		public void Begin(){

			//remove evans from party
			Player[] newParty = new Player[2];
			int counter = 0;

			foreach(Player play in PlayManager.instance.Party){
				if(play.gameObject.name != "Evans"){
					newParty [counter] = play;
					counter++;
				}
			}
			PlayManager.instance.Party = newParty;

			GameObject evans = GameObject.Find ("Evans");
			//Remove evans' followtarget
			evans.GetComponent<Movement>().FollowTarget = null;

			//Reset Evans' health
			evans.GetComponent<Player>().ResetHealth();
			//Add evan's AI component
			evans.AddComponent (typeof(Oran_EvansAI));
			//Pass along the passTarget
			evans.GetComponent<Oran_EvansAI>().passTarget = passTarget;
            // Activate Evans's inactive Enemy script
            evans.GetComponent<Player>().TurnCoat();

            // Change Evans's animation events to point to the right functions
            //Animator animator = evans.GetComponent<Animator>();
            //RuntimeAnimatorController rac = animator.runtimeAnimatorController;
            //AnimationClip[] clips = rac.animationClips;
            //foreach (AnimationClip clip in clips)
            //{
            //    if (clip.name == "Evans_GunAttack")
            //    {
            //        Debug.Log("Processing Evans_GunAttack");
            //        clip.events[0].functionName = "EndProjectileAttack";
            //    }
            //    if (clip.name == "Evans_SwordAttack")
            //    {
            //        Debug.Log("Processing Evans_SwordAttack");
            //        clip.events[0].functionName = "EndPiercingAttack";
            //    }
            //}

            //Start up the battleground
            evansBattleground.Begin();
		}

		public void Activate(){
			Begin ();
		}


		void OnDrawGizmos(){
			//	void OnDrawGizmosSelected(){
			//		if(targetList != null){
			if(passTarget != null){
				//			foreach(GameObject target in targetList){

				//draw a line from our position to it
				Gizmos.color = Color.green;
				Gizmos.DrawLine(this.transform.position, passTarget.transform.position);


				//			}

			}
		}
	}




}