using Artifice.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    /// <summary>
    /// A reference to the player component that should also be attached
    /// to this gameobject
    /// </summary>
    private Player player;

    /// <summary>
    /// The coordinate that this player is at. This should always be whole numbers.
    /// </summary>
    private Vector2 coordinate;

    /// <summary>
    /// Where should another player/NPC following this player be?
    /// </summary>
    private Vector2 followPos;

    /// <summary>
    /// Handles input delay, so that the player doesn't move too fast
    /// </summary>
    private float inputDelay;

    /// <summary>
    /// If this player is meant to follow another player, set this variable
    /// </summary>
    public Movement followTarget;

    //private SortableSprite sp;


    void Start()
    {
        player = GetComponent<Player>();
        //sp = GetComponentInChildren<SortableSprite>();
    }

    void Update()
    {
        if (followTarget == null)
        {
            //I use this variable to tell if any input was made
            Vector2 moveVector = Vector2.zero;

            if (inputDelay <= 0f)
            {
                inputDelay = 0.3f;
                Vector2 moveDelta = Vector2.zero;
                if (Input.GetAxis("Vertical") > 0f)
                {
                    moveDelta += Vector2.up;
                    /*
                    sp.UpdateSortingOrder();
                    int next = Artifice.Data.GameManager.Instance.PlayerChunk + 1;
                    if(transform.position.y > Artifice.Data.GameManager.PLAYER_RESET_THRESHOLD * next) {
                        Artifice.Data.GameManager.Instance.PlayerChunk++;
                    }
                    */
                }
                if (Input.GetAxis("Horizontal") < 0f)
                {
                    moveDelta += Vector2.left;
                }
                if (Input.GetAxis("Vertical") < 0f)
                {
                    moveDelta += Vector2.down;
                    /*
                    sp.UpdateSortingOrder();
                    int next = Artifice.Data.GameManager.Instance.PlayerChunk;
                    if (transform.position.y < Artifice.Data.GameManager.PLAYER_RESET_THRESHOLD * next) {
                        Artifice.Data.GameManager.Instance.PlayerChunk--;
                    }
                    */
                }
                if (Input.GetAxis("Horizontal") > 0f)
                {
                    moveDelta += Vector2.right;
                }
                if (moveDelta == Vector2.zero)
                {
                    inputDelay = 0f;
                }
                else
                {
                    followPos = coordinate;
                    coordinate += moveDelta;
                }
            }
            else
            {
                inputDelay -= Time.deltaTime;
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(coordinate.x), Mathf.Floor(coordinate.y), 0f), Time.deltaTime * 5f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(followTarget.followPos.x), Mathf.Floor(followTarget.followPos.y), 0f), Time.deltaTime * 5f);
        }
    }
}
