using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    SpriteRenderer myRenderer;

    public Color friendlyColor, hostileColor, neutralColor;

    void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        
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
            SetToFriendlyColor();
        }

        else if (other.CompareTag("Enemy"))
           // && myRenderer.sprite.bounds.Contains(other.GetComponent<Player>().FootPos))
        {
            SetToHostileColor();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
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
}
