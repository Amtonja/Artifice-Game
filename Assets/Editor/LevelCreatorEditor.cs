using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorEditor : Editor {

    LevelCreator script;

    private int curX = 0, curY = 0;
    private float tileSpacing = 2.56f;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        script = (LevelCreator)target;

        if (GUILayout.Button("Reset Cursor")) {
            ResetCursor();
        }

        if(GUILayout.Button("Reset All")) {
            ResetLevel();
        }
    }

    void OnSceneGUI() {
        if (script == null) return;
        if (script.Tiles == null) ResetLevel();
        Rect size = new Rect(0, 0, 260, 130);
        float sizeButton = 20f;
        GUI.BeginGroup(new Rect(Screen.width - size.width, Screen.height - size.height - 50, size.width, size.height));
        GUI.Box(size, "Level Editor Controls");

        Rect rc = new Rect(sizeButton + sizeButton/2, sizeButton, sizeButton, sizeButton);

        Sprite buttonImage = Resources.Load<Sprite>("Editor/LevelCreator/up");
        if (GUI.Button(rc, buttonImage.texture)) {
            script.transform.position += new Vector3(0, tileSpacing, 0);
            if (curY-- <= 0) {
                script.Height++;
                for (int i = 0; i < script.Width; i++) {
                    AddTile(0, 0, tileSpacing * (script.Width-1) - i * tileSpacing, tileSpacing);
                }
                for (int i = 0; i < script.Tiles.Count; i++) {
                    script.Tiles[i].transform.position += new Vector3(0, -tileSpacing, 0);
                }
                script.transform.position += new Vector3(0, -tileSpacing, 0);
                curY = 0;
            }
            Camera.main.transform.position = script.transform.position;
        }

        rc.y += sizeButton;
        rc.x -= sizeButton;
        buttonImage = Resources.Load<Sprite>("Editor/LevelCreator/left");
        if (GUI.Button(rc, buttonImage.texture)) {
            script.transform.position += new Vector3(-tileSpacing, 0, 0);
            if (curX-- <= 0) {
                script.Width++;
                for(int i = 0; i < script.Height; i++) {
                    AddTile(i, 0, -tileSpacing, i * -tileSpacing);
                }
                for (int i = 0; i < script.Tiles.Count; i++) {
                    script.Tiles[i].transform.position += new Vector3(tileSpacing, 0, 0);
                }
                script.gameObject.transform.position += new Vector3(tileSpacing, 0, 0);
                curX = 0;
            }
            Camera.main.transform.position = script.transform.position;
        }

        rc.x += sizeButton * 2;
        buttonImage = Resources.Load<Sprite>("Editor/LevelCreator/right");
        if (GUI.Button(rc, buttonImage.texture)) {
            script.transform.position += new Vector3(tileSpacing, 0, 0);
            Camera.main.transform.position = script.transform.position;
            if (curX++ >= script.Width-1) {
                script.Width++;
                for (int i = 0; i < script.Height; i++) {
                    if(i == script.Height - 1) AddTile(new Vector2(tileSpacing * (script.Width - 1), -tileSpacing * (script.Height - 1)));
                    else AddTile(i, script.Width-1, tileSpacing * (script.Width - 1), -tileSpacing * i);
                }
            }
        }

        rc.y += sizeButton;
        rc.x -= sizeButton;
        buttonImage = Resources.Load<Sprite>("Editor/LevelCreator/down");
        if (GUI.Button(rc, buttonImage.texture)) {
            script.transform.position += new Vector3(0, -tileSpacing, 0);
            Camera.main.transform.position = script.transform.position;
            if (curY++ >= script.Height - 1) {
                script.Height++;
                for(int i = 0; i < script.Width; i++) {
                    AddTile(new Vector2(tileSpacing * i, -tileSpacing * (script.Height - 1)));
                }
            }
        }

        // DEBUG
        if(GUI.Button(new Rect(0, 0, rc.width, rc.height), "1")) {
            for(int i = 0; i < script.Tiles.Count; i++) {
                script.Tiles[i].GetComponentInChildren<TextMesh>().text = i.ToString();
            }
        }
        //END DEBUG

        GUI.EndGroup();
    }

    private void ResetLevel() {
        if(script.Tiles != null) {
            for(int i = 0; i < script.Tiles.Count; i++) {
                if(script.Tiles[i] != null) DestroyImmediate(script.Tiles[i].gameObject);
            }
        }
        curX = 0;
        curY = 0;
        script.Height = 1;
        script.Width = 1;
        script.Tiles = new List<Tile>();
        ResetCursor();
        AddTile();
    }

    private void ResetCursor() {
        script.transform.position = Vector3.zero;
        Camera.main.transform.position = Vector3.zero;
    }

    private void AddTile() {
        script.Tiles.Add(Tile.CreateTile(script.tile, script.transform.position, script.background).GetComponent<Tile>());
    }

    private void AddTile(Vector2 pos) {
        script.Tiles.Add(Tile.CreateTile(script.tile, pos, script.background).GetComponent<Tile>());
    }

    private void AddTile(int row, int col) {
        script.Tiles.Insert(LevelCreator.Offset(row, col, script.Width), Tile.CreateTile(script.tile, script.transform.position, script.background).GetComponent<Tile>());
    }

    private void AddTile(int row, int col, float x, float y) {
        Vector3 pos = new Vector3(x, y, script.transform.position.z);
        script.Tiles.Insert(LevelCreator.Offset(row, col, script.Width), Tile.CreateTile(script.tile, pos, script.background).GetComponent<Tile>());
    }
}
