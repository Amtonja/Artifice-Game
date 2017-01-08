using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CombatSpace : MonoBehaviour {

	public int sizeX, sizeY, offsetX, offsetY;
	private int prevOffsetX, prevOffsetY;

	/// <summary>
	/// The positions that the player characters should be moved to
	/// when combat begins. The 0th index will be where the party leader
	/// will be placed, and the rest of the party members will move based
	/// on their index in the party (see PlayManager)
	/// </summary>
	public Vector2[] playerStartPositions;

	/// <summary>
	/// What position should the enemy be moved to when combat starts
	/// </summary>
	public Vector2 enemyStartPosition;

	private int curSize;

	/// <summary>
	/// This is called whenever the tiles need to be updated
	/// </summary>
	private void ResetTiles() {
		curSize = sizeX*sizeY;
		while(transform.childCount > 1) {
			DestroyImmediate(transform.GetChild(1).gameObject);
		}
		transform.GetChild(0).localPosition = new Vector3(-sizeX/2,sizeY/2,0f);
		for(int i = 1; i < curSize; i++) {
			GameObject temp = GameObject.Instantiate(transform.GetChild(0).gameObject, transform);
			temp.transform.localPosition = new Vector3(-sizeX/2 + i%sizeX, sizeY/2 - (int)(i/sizeX),0f);
		}

		for(int i = 0; i < curSize; i++) {
			Color c = Color.white;
			foreach(Vector2 v in playerStartPositions) {
				if(((Vector2)transform.GetChild(i).localPosition).Equals(v)) {
					c = Color.green;
				}
			}
			if(((Vector2)transform.GetChild(i).localPosition).Equals(enemyStartPosition)) {
				c = Color.red;
			}
			transform.GetChild(i).localPosition += new Vector3(offsetX, offsetY);
			transform.GetChild(i).GetComponent<SpriteRenderer>().color =
				new Color(c.r,c.g,c.b,(0.9f-
					Vector3.Distance(new Vector3(offsetX,offsetY,0f),transform.GetChild(i).localPosition)
					/Vector3.Distance(new Vector3(offsetX,offsetY,0f),transform.GetChild(0).localPosition))/4f);
		}
		prevOffsetX = offsetX;
		prevOffsetY = offsetY;
	}

	public Vector3 PlayerPosition(int index) {
		return transform.TransformPoint((Vector3)playerStartPositions[index]);
	}

	public Vector3 EnemyPosition() {
		return transform.TransformPoint((Vector3)enemyStartPosition);
	}
	
	void Update () {
		if(sizeX*sizeY != curSize
			|| offsetX != prevOffsetX
			|| offsetY != prevOffsetY) {
			if(sizeX > 0 && sizeY > 0) ResetTiles();
		}
	}
}