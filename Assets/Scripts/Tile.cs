using UnityEngine;
using System.Collections.Generic;

public class Tile : MonoBehaviour {
    private Sprite background;

	void Start () {
	    if(background == null) {
            background = Resources.Load<Sprite>("DefaultBackground");
        }
	}

    public static GameObject CreateTile(GameObject prefab, Vector3 position, Sprite background) {
        GameObject tile = (GameObject)Instantiate(prefab, position, Quaternion.identity);
        tile.GetComponent<SpriteRenderer>().sprite = background;
        // DEBUG
        TextMesh t = new GameObject().AddComponent<TextMesh>();
        t.transform.parent = tile.transform;
        t.transform.localPosition = Vector3.zero + new Vector3(0, 0, -1);
        t.alignment = TextAlignment.Center;
        t.anchor = TextAnchor.MiddleCenter;
        // END DEBUG
        return tile;
    }
}
