using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Canvas))]
public class CombatGrid : MonoBehaviour
{
    /// <summary>
    /// The width and height of the grid in squares.
    /// </summary>
    public int gridWidth, gridHeight;

    private float gridOffsetX, gridOffsetY, cellSizeX, cellSizeY;
    private Transform gridOrigin;

    private GridSquare[,] _cells;

    /// <summary>
    /// Arrays of vectors containing the starting positions of players and enemies
    /// for the current combat. They are expressed as INTEGER coordinates referring
    /// to squares on the grid with the origin square (0,0) at lower left.
    /// </summary>
    public Vector2[] playerStartPositions, enemyStartPositions;

    public GridSquare cellPrefab;    

    void Awake()
    {
        // Need to know size of the cells before anything else starts happening in OnEnable or Start
        CellSizeX = cellPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * cellPrefab.transform.localScale.x;
        CellSizeY = cellPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y * cellPrefab.transform.localScale.y;
    }

    // Use this for initialization
    void Start()
    {        
        
    }   

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateGrid()
    {
        _cells = new GridSquare[gridWidth, gridHeight];

        gridOrigin = transform.Find("GridOrigin");
        gridOrigin.localPosition = new Vector3(0, 0);

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GridSquare square = Instantiate(cellPrefab, new Vector3(i * CellSizeX, j * CellSizeY),
                    Quaternion.identity, transform.GetChild(0)) as GridSquare;
                square.transform.localPosition = new Vector3(i * CellSizeX, j * CellSizeY);
                _cells[i, j] = square;
            }
        }

        for (int i = 0; i < enemyStartPositions.Length; i++)
        {
            Cells[(int)enemyStartPositions[i].x, (int)enemyStartPositions[i].y].SetToHostileColor();
        }
    }

    #region C# Properties
    public float CellSizeX
    {
        get
        {
            return cellSizeX;
        }

        set
        {
            cellSizeX = value;
        }
    }

    public float CellSizeY
    {
        get
        {
            return cellSizeY;
        }

        set
        {
            cellSizeY = value;
        }
    }

    public GridSquare[,] Cells
    {
        get
        {
            return _cells;
        }
    }
    #endregion
}
