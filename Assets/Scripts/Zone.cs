using UnityEngine;

public class Zone : MonoBehaviour {
    private Sprite background;
    private bool occupied, locked;

    private int index;

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

    public bool Occupied {
        get { return occupied; }
        set { occupied = value; }
    }

    public bool Locked {
        get { return locked; }
        set { locked = value; }
    }

    public int Index {
        get { return index; }
        set { index = value; }
    }
}
