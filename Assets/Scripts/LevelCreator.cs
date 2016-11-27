using UnityEngine;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {
    public GameObject tile;
    public Sprite background;

    private int width = 1, height = 1;

    private List<Zone> tiles;

    public static int Offset(int row, int col, int rowLen) {
        return ((row * rowLen) + col);
    }

    public int Width {
        get { return width; }
        set { width = value; }
    }

    public int Height {
        get { return height; }
        set { height = value; }
    }

    public List<Zone> Tiles {
        get { return tiles; }
        set { tiles = value; }
    }
}
