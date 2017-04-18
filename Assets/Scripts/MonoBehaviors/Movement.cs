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

	/// <summary>
	/// Used by the cutscene manager; overrides player input and forces character to move in a direction.
	/// </summary>
	private bool bForceMove = false;

	/// <summary>
	/// A reference to the CM_ForceMove object that told us to do this, so we can tell it we're done when the process ends.
	/// </summary>
	private GameObject forcedSender;

	/// <summary>
	/// Forced movement. inputed Vector2 added to current position to tell use where we're trying to be.
	/// </summary>
	private Vector2 intendedPosition; 

    //private SortableSprite sp;


    void Start()
    {
        player = GetComponent<Player>();
        //sp = GetComponentInChildren<SortableSprite>();
		coordinate = new Vector2(this.transform.position.x, this.transform.position.y);
		followPos = coordinate;
    }

    void Update()
    {
		//skip everything except forced movement if that's what we're using
		if (bForceMove) 
		{
			ForceMove ();
			return;
		}

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

			transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(coordinate.x), Mathf.Floor(coordinate.y), 0f), Time.deltaTime*5f);
		} else {
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(followTarget.followPos.x), Mathf.Floor(followTarget.followPos.y), 0f), Time.deltaTime*5f);
		}
	}

	//Forces a character to move in a direction.
	private void ForceMove(){
		if (this.transform.position != new Vector3(intendedPosition.x, intendedPosition.y, 0)) {
			Vector2 moveVector = Vector2.zero;

			if(inputDelay <= 0f) {
				inputDelay = 0.3f;
				Vector2 moveDelta = Vector2.zero;
				if (intendedPosition.y > this.transform.position.y) {
					moveDelta += Vector2.up;
					Debug.Log ("Forcing up!");
				}
				else if (intendedPosition.x < this.transform.position.x) {
					moveDelta += Vector2.left;
					Debug.Log ("Forcing left!");
				}
				else if (intendedPosition.y < this.transform.position.y) {
					moveDelta += Vector2.down;
					Debug.Log ("Forcing down!");
				}
				else if (intendedPosition.x > this.transform.position.x) {
					moveDelta += Vector2.right;
					Debug.Log ("Forcing right!");
				}
				if(moveDelta == Vector2.zero) {
					inputDelay = 0f;
				} else {
					followPos = coordinate;
					coordinate += moveDelta;
				}
			} else {
				inputDelay -= Time.deltaTime;
			}

			transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(coordinate.x), Mathf.Floor(coordinate.y), 0f), Time.deltaTime*5f);


		} else {
			Debug.Log ("Finished moving!");
			StopForcedMove ();
			inputDelay = 0.3f;
		}

	}

	//Begin forcing movement. Called by CM_ForceMove to start the process.
	public void StartForcedMove(Vector2 spaces){
		intendedPosition = new Vector2(this.transform.position.x + spaces.x, this.transform.position.y + spaces.y);
		bForceMove = true;
		Debug.Log ("Forced movement starting!");
	}

	//stops forced movement process.
	private void StopForcedMove(){
		bForceMove = false;
		intendedPosition = new Vector2 (0, 0);
		forcedSender.GetComponent<CM_ForceMove> ().NextTarget ();

	}

	//passed by the instance of CM_ForceMove,so we can tell it we're done.
	public void GetForcedSender (GameObject sender){
		forcedSender = sender;
	}
}
