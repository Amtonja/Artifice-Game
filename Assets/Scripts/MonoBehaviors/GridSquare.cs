using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    SpriteRenderer myRenderer;

    public Color friendlyColor, hostileColor, neutralColor;

    private bool containsPlayer, containsEnemy;    

    void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        //float xSize = myRenderer.bounds.size.x;
        //float ySize = myRenderer.bounds.size.y;
        //transform.localScale = new Vector3(0.5f / xSize, 0.5f / ySize, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
            //&& myRenderer.sprite.bounds.Contains(other.GetComponent<Player>().FootPos))
        {
            ContainsPlayer = true;
            SetToFriendlyColor();
        }

        else if (other.CompareTag("Enemy"))
           // && myRenderer.sprite.bounds.Contains(other.GetComponent<Player>().FootPos))
        {
            ContainsEnemy = true;
            SetToHostileColor();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ContainsPlayer = ContainsEnemy = false; // this is assuming two entities can't share a square
        SetToNeutralColor();
    }

    public void SetToFriendlyColor()
    {
        myRenderer.color = friendlyColor;
    }

    public void SetToHostileColor()
    {        
        myRenderer.color = hostileColor;
    }

    public void SetToNeutralColor()
    {
        myRenderer.color = neutralColor;
    }

    #region C# Properties
    public bool ContainsPlayer
    {
        get
        {
            return containsPlayer;
        }

        set
        {
            containsPlayer = value;
        }
    }

    public bool ContainsEnemy
    {
        get
        {
            return containsEnemy;
        }

        set
        {
            containsEnemy = value;
        }
    }
    #endregion
}
