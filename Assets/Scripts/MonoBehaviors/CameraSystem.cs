using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This references the PlayManager to figure out who is in the party and what
/// should be visible to the camera.
/// TODO: This will eventually need to take into account the bounds of the area
/// </summary>
public class CameraSystem : MonoBehaviour {

	public AreaInfo currentArea; //AreaInfo tells us the bounds of the area, among other things. 
	
	void Update () { //old, loose-position code
//		Vector3 averagePosition = new Vector3(0f,2f,0f);
//
//		if (PlayManager.instance.ExploreMode) {
//			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
//				averagePosition += PlayManager.instance.party[i].transform.position;
//			}
//
//			averagePosition /= PlayManager.instance.party.Length;
//		} else {
//			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
//				averagePosition += PlayManager.instance.party[i].transform.position;
//			}
//
//			foreach(Player enemy in PlayManager.instance.EnemyCombatants) {
//				averagePosition += enemy.transform.position;
//			}
//
//			averagePosition /= (PlayManager.instance.party.Length + PlayManager.instance.EnemyCombatants.Count);
//		}
//
//		transform.position = Vector3.MoveTowards(transform.position,
//			averagePosition - Vector3.forward*10f,
//			Time.deltaTime*Vector3.Distance(transform.position, averagePosition - Vector3.forward*10f));
	}


	void LateUpdate(){

		if (PlayManager.instance.ExploreMode) {


			Vector3 fixedLocation = new Vector3(PlayManager.instance.party[0].transform.position.x, PlayManager.instance.party[0].transform.position.y, this.transform.position.z);

			//check to see if we're going to head outside bounds of the x range. 
			//5 is added to x checks to have the camera's center not be the exact edge.
			//3 is added to y checks for the same reason. Note that because this is 16 by 9, y checks
			//do not perfectly line up with vertical edges, because the camera can't anyway.

			if(PlayManager.instance.party[0].transform.position.x < currentArea.minX + 5f){
				fixedLocation.x = currentArea.minX + 5f;
			}
			if(PlayManager.instance.party[0].transform.position.x > currentArea.maxX - 5F){
				fixedLocation.x = currentArea.maxX - 5f;
			}
			if(PlayManager.instance.party[0].transform.position.y < currentArea.minY + 3f){
				fixedLocation.y = currentArea.minY + 3f;
			}
			if(PlayManager.instance.party[0].transform.position.y > currentArea.maxY - 3f){
				fixedLocation.y = currentArea.maxY - 3f;
			}

			this.transform.position = fixedLocation;



		}



	}
}
