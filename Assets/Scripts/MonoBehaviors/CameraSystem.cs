using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This references the PlayManager to figure out who is in the party and what
/// should be visible to the camera.
/// TODO: This will eventually need to take into account the bounds of the area
/// </summary>
public class CameraSystem : MonoBehaviour {
	
	void Update () {
		Vector3 averagePosition = new Vector3(0f,2f,0f);

		if (PlayManager.instance.ExploreMode) {
			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
				averagePosition += PlayManager.instance.party[i].transform.position;
			}

			averagePosition /= PlayManager.instance.party.Length;
		} else {
			for (int i = 0; i < PlayManager.instance.party.Length; i++) {
				averagePosition += PlayManager.instance.party[i].transform.position;
			}

			foreach(Player enemy in PlayManager.instance.EnemyCombatants) {
				averagePosition += enemy.transform.position;
			}

			averagePosition /= (PlayManager.instance.party.Length + PlayManager.instance.EnemyCombatants.Count);
		}

		transform.position = Vector3.MoveTowards(transform.position,
			averagePosition - Vector3.forward*10f,
			Time.deltaTime*Vector3.Distance(transform.position, averagePosition - Vector3.forward*10f));
	}
}
