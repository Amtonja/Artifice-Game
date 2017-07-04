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
//    private Vector2 coordinate;

    /// <summary>
    /// Where should another player/NPC following this player be?
    /// </summary>
    private Vector2 followPos;

    /// <summary>
    /// Handles input delay, so that the player doesn't move too fast
    /// </summary>
//    private float inputDelay;

    /// <summary>
    /// If this player is meant to follow another player, set this variable
    /// </summary>
    public Movement followTarget;

    /// <summary>
    /// Used by the cutscene manager; overrides player input and forces character to move in a direction.
    /// </summary>
    private bool bForceMove = false;

    /// <summary>
    /// Used by the cutscene manager; overrides player input and forces character to face a direction.
    /// </summary>
    private bool bForceFace = false;

    /// <summary>
    /// A reference to the CM_ForceMove object that told us to do this, so we can tell it we're done when the process ends.
    /// </summary>
    private GameObject forcedSender;

    /// <summary>
    /// Forced movement. inputed Vector2 added to current position to tell use where we're trying to be.
    /// </summary>
    private Vector2 intendedPosition;

    //private SortableSprite sp;
    private Animator _animator;

    //Used to tell what direction we're facin' for idle sprites
    public enum directions { Up, Down, Left, Right };
    private directions moveDir;

    private float moveSpeed = 3.0f;


    //Temporary measure to stop following animation from constantly going into an idle state every time it pops into place
    //    private float waitTimerMax = 0.22f;
    //    private float waitTimer = 0.22f;


    private bool bForceLock = false; //Locks controls. Used by AreaTransition, etc?

	private bool bAnimating = false; //If this character is being forced to animate



    /// <summary>
    /// Check this if it doesn't use player controls.
    /// </summary>
    public bool bNPC = false;

    private bool bFollowing = false; //used for new following code.
    private float followDist = 1.5f;


	private Transform basePoint; //The base of the feet. Used for collision checks

    void Start()
    {
        player = GetComponent<Player>();
        //sp = GetComponentInChildren<SortableSprite>();
        //        coordinate = new Vector2(this.transform.position.x, this.transform.position.y);
        //        followPos = coordinate;

        _animator = GetComponent<Animator>(); //Might need to be in Awake

		basePoint = transform.FindChild ("Base");
    }

    void Update()
    {

        //If we're in combat or a cutscene, skip everything
		if (bForceLock || bAnimating) { return; }

        //		if (PlayManager.instance.ExploreMode) {
        //			return;
        //		}

        //		if (!PlayManager.instance.ExploreMode ()) {
        //			return;
        //		}
        //skip everything except forced movement if that's what we're using
        if (bForceMove)
        {
            bForceFace = false;
            ForceMove();
            return;
        }
        if (player.InCombat) { return; }

        //skip everything if we're just forced to face a direction
        if (bForceFace)
        {
            return;
        }

        if (followTarget == null && !player.InCombat && !bNPC)
        {
            HandleMove();

        }
        else if (followTarget != null && !player.InCombat && !bNPC && !bForceMove)
        {

            FollowMove();
        }


    }

    //Gets input, moves character
    private void HandleMove()
    {
        Vector2 moveDelta = Vector2.zero;

        if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
        {
            if (moveDir == directions.Up)
            {
                _animator.Play(Animator.StringToHash("IdleUp"));
            }
            else if (moveDir == directions.Down)
            {
                _animator.Play(Animator.StringToHash("IdleDown"));
            }
            else if (moveDir == directions.Right)
            {
                _animator.Play(Animator.StringToHash("IdleRight"));
            }
            else if (moveDir == directions.Left)
            {
//                _animator.Play(Animator.StringToHash("IdleLeft"));
				_animator.Play(Animator.StringToHash("IdleRight"));
            }

            return;
        }

        if (Input.GetAxis("Vertical") > 0.5f)
        {
            moveDelta += Vector2.up;
            //To avoid overlapping animations
            if (Input.GetAxis("Horizontal") == 0f)
                _animator.Play(Animator.StringToHash("GoUp"));
            moveDir = directions.Up;
        }
        else if (Input.GetAxis("Vertical") < -0.5f)
        {
            moveDelta += Vector2.down;
            if (Input.GetAxis("Horizontal") == 0f)
                _animator.Play(Animator.StringToHash("GoDown"));
            moveDir = directions.Down;

        }
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            moveDelta += Vector2.right;
            _animator.Play(Animator.StringToHash("GoRight"));
			this.transform.localScale = new Vector3(1, 1, 1);
            moveDir = directions.Right;
        }
        else if (Input.GetAxis("Horizontal") < -0.5f)
        {
            moveDelta += Vector2.left;
//            _animator.Play(Animator.StringToHash("GoLeft"));
			_animator.Play(Animator.StringToHash("GoRight"));
			this.transform.localScale = new Vector3(-1, 1, 1);

            moveDir = directions.Left;
        }

		//If we detect collision in a direction we want to move, nuke momentum on that axis
		if(Mathf.Abs(Input.GetAxis("Vertical")) > 0f && CollideCheck(0f, moveDelta.y)){
			moveDelta.y = 0;
		}

		if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0f && CollideCheck(moveDelta.x, 0f)){
			moveDelta.x = 0;
		}

//		if(!CollideCheck(moveDelta)){
        	transform.Translate((moveDelta * moveSpeed) * Time.deltaTime);
//		}


    }


	private RaycastHit2D _raycastHit;
	//

	bool CollideCheck(float x, float y){

		var ray = new Vector2 (0, 0);
		if (basePoint != null) {
			ray = new Vector2 (basePoint.transform.position.x, basePoint.transform.position.y + 0.1f); //the base transform is the bottom of feet, need offset
		} else {
			ray = new Vector2 (this.transform.position.x, this.transform.position.y + 0.1f); //the base transform is the bottom of feet, need offset
		}

		Vector2 delta = new Vector2 (x, y);
	
		Debug.DrawRay( ray, delta * 0.2f, Color.red );
		_raycastHit = Physics2D.Raycast( ray, delta, 0.2f, ~9); //9 is collision layer, ~ means only focus on that
		if (_raycastHit) {
//			Debug.Log ("We hit " + _raycastHit.collider.name.ToString () + " and its layer is " + _raycastHit.collider.gameObject.layer.ToString());
			return true;
		} else {
			return false;
		}

	}

    //New following function.
    private void FollowMove()
    {
        //For now, other characters just try to stay near the player.
        if (!bFollowing)
        {
            //Check to see if we need to be following
            if (Vector2.Distance(this.transform.position, followTarget.transform.position) > followDist)
            {
                bFollowing = true;

            }
            else {
                //We're idle, so set idle pose
                if (moveDir == directions.Up)
                {
                    _animator.Play(Animator.StringToHash("IdleUp"));
                }
                else if (moveDir == directions.Down)
                {
                    _animator.Play(Animator.StringToHash("IdleDown"));
                }
                else if (moveDir == directions.Right)
                {
                    _animator.Play(Animator.StringToHash("IdleRight"));
                }
                else if (moveDir == directions.Left)
                {
//                    _animator.Play(Animator.StringToHash("IdleLeft"));
					_animator.Play(Animator.StringToHash("IdleRight"));
                }
            }

        }
        else {
            //We need to follow
            if (Vector2.Distance(this.transform.position, followTarget.transform.position) < followDist - 0.5f)
            {
                bFollowing = false;
                return;
            }
            //			float step = moveSpeed * Time.deltaTime;
            //			Vector2 followDir = Vector2.MoveTowards (this.transform.position, followTarget.transform.position, step);
            //			Vector2 facingDir = this.transform.position - followTarget.transform.position; //For animation

            //Facing is backwards because it's a direction
            //			if (facingDir.y < 0f) {
            //				//To avoid overlapping animations
            //				if(facingDir.x == 0f)
            //					_animator.Play (Animator.StringToHash ("GoUp"));
            //				moveDir = directions.Up;
            //			} else if (facingDir.y > 0f) {
            //				if(facingDir.x == 0f)
            //					_animator.Play (Animator.StringToHash ("GoDown"));
            //				moveDir = directions.Down;
            //			} 
            //			if (facingDir.x < 0f) {
            //				_animator.Play(Animator.StringToHash("GoRight"));
            //				moveDir = directions.Right;
            //			} else if (facingDir.x > 0f) {
            //				_animator.Play (Animator.StringToHash ("GoLeft"));
            //				moveDir = directions.Left;
            //			}
            //
            //
            //			this.transform.position = followDir;// Transform.translate with facingDir for easier collision detection?

            Vector3 moveDelta = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, 0f) - this.transform.position;

            if (moveDelta.y > 0f && Mathf.Abs(moveDelta.y) > Mathf.Abs(moveDelta.x))
            {
                //				moveDelta += Vector2.up;
                //To avoid overlapping animations
                //				if(Mathf.Abs(moveDelta.y) > Mathf.Abs(moveDelta.x))
                _animator.Play(Animator.StringToHash("GoUp"));
                moveDir = directions.Up;
            }
            else if (moveDelta.y < 0f && Mathf.Abs(moveDelta.y) > Mathf.Abs(moveDelta.x))
            {
                //				moveDelta += Vector2.down;
                //				if(moveDelta.x == 0f)
                _animator.Play(Animator.StringToHash("GoDown"));
                moveDir = directions.Down;

            }
            else if (moveDelta.x > 0f)
            {// && Mathf.Abs(moveDelta.x) > Mathf.Abs(moveDelta.y)) {
             //				moveDelta += Vector2.right;
                _animator.Play(Animator.StringToHash("GoRight"));
				this.transform.localScale = new Vector3(1, 1, 1);
                moveDir = directions.Right;
            }
            else if (moveDelta.x < -0f)
            {// && Mathf.Abs(moveDelta.x) > Mathf.Abs(moveDelta.y)) {
             //				moveDelta += Vector2.left;
//                _animator.Play(Animator.StringToHash("GoLeft"));
				_animator.Play(Animator.StringToHash("GoRight"));
				this.transform.localScale = new Vector3(-1, 1, 1);
                moveDir = directions.Left;
            }

			//If we detect collision in a direction we want to move, nuke momentum on that axis
			if(Mathf.Abs(moveDelta.y) > 0f && CollideCheck(0f, moveDelta.y)){
				moveDelta.y = 0;
			}

			if(Mathf.Abs(moveDelta.x) > 0f && CollideCheck(moveDelta.x, 0f)){
				moveDelta.x = 0;
			}

            transform.Translate((moveDelta.normalized * moveSpeed) * Time.deltaTime);
        }
    }

    //Forces a character to move to a location.
    private void ForceMove()
    {
        if (Vector3.Distance(this.transform.position, new Vector3(intendedPosition.x, intendedPosition.y, 0)) > 0.1f)
        {

            //			float step = moveSpeed * Time.deltaTime;
            //			Vector2 moveDelta = Vector2.MoveTowards (this.transform.position, intendedPosition, step);
            Vector3 moveDelta = new Vector3(intendedPosition.x, intendedPosition.y, 0f) - this.transform.position;

            if (moveDelta.y > 0f && Mathf.Abs(moveDelta.y) > Mathf.Abs(moveDelta.x))
            {
                //				moveDelta += Vector2.up;
                //To avoid overlapping animations
                //				if(Mathf.Abs(moveDelta.y) > Mathf.Abs(moveDelta.x))
                _animator.Play(Animator.StringToHash("GoUp"));
                moveDir = directions.Up;
            }
            else if (moveDelta.y < 0f && Mathf.Abs(moveDelta.y) > Mathf.Abs(moveDelta.x))
            {
                //				moveDelta += Vector2.down;
                //				if(moveDelta.x == 0f)
                _animator.Play(Animator.StringToHash("GoDown"));
                moveDir = directions.Down;

            }
            else if (moveDelta.x > 0f)
            {// && Mathf.Abs(moveDelta.x) > Mathf.Abs(moveDelta.y)) {
             //				moveDelta += Vector2.right;
                _animator.Play(Animator.StringToHash("GoRight"));
				this.transform.localScale = new Vector3(1, 1, 1);
                moveDir = directions.Right;
            }
            else if (moveDelta.x < -0f)
            {// && Mathf.Abs(moveDelta.x) > Mathf.Abs(moveDelta.y)) {
             //				moveDelta += Vector2.left;
//                _animator.Play(Animator.StringToHash("GoLeft"));
				_animator.Play(Animator.StringToHash("GoRight"));
				this.transform.localScale = new Vector3(-1, 1, 1);
                moveDir = directions.Left;
            }

            transform.Translate((moveDelta.normalized * moveSpeed) * Time.deltaTime);
            //			transform.position = moveDelta;


            //            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(coordinate.x), Mathf.Floor(coordinate.y), 0f), Time.deltaTime * 5f);

            //If we're at the end point, go to their idle
            if (this.transform.position.x == intendedPosition.x && this.transform.position.y == intendedPosition.y)
            {
                if (moveDir == directions.Up)
                {
                    _animator.Play(Animator.StringToHash("IdleUp"));
                }
                else if (moveDir == directions.Down)
                {
                    _animator.Play(Animator.StringToHash("IdleDown"));
                }
                else if (moveDir == directions.Right)
                {
                    _animator.Play(Animator.StringToHash("IdleRight"));
					this.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (moveDir == directions.Left)
                {
//                    _animator.Play(Animator.StringToHash("IdleLeft"));
					_animator.Play(Animator.StringToHash("IdleRight"));
					this.transform.localScale = new Vector3(-1, 1, 1);
                }
            }



        }
        else {
            Debug.Log("Finished moving!");
            this.transform.position = intendedPosition;
			StopForcedMove(true);
            //            inputDelay = 0.3f;
        }

    }

    //Begin forcing movement. Called by CM_ForceMove to start the process.
    public void StartForcedMove(Vector2 spaces)
    {
        //Kill follow target if we have one because it messes with things
        //        followTarget = null;
        //        coordinate = this.transform.position;
        //        intendedPosition = new Vector2(this.transform.position.x + spaces.x, this.transform.position.y + spaces.y);
        intendedPosition = spaces;
        bForceMove = true;
        Debug.Log("Forced movement starting!");
    }

    //stops forced movement process. Callback sends... well, a callback towards whatever stopped it. 
	public void StopForcedMove( bool bCallback)
    {
        bForceMove = false;
        intendedPosition = new Vector2(0, 0);
        //        forcedSender.GetComponent<CM_ForceMove>().NextTarget();
		if (bCallback) {
			forcedSender.SendMessage ("MoveComplete");
		}


    }

    //passed by the instance of CM_ForceMove,so we can tell it we're done.
    public void GetForcedSender(GameObject sender)
    {
        forcedSender = sender;
    }

    //passed by CM_FaceDir
    //	public void FaceDir(string fDir){

    public void FaceDir(Vector2 fDir)
    {
        bForceFace = true;
        if (fDir.y == 1)
        {
            _animator.Play(Animator.StringToHash("IdleUp"));
        }
        else if (fDir.y == -1)
        {
            _animator.Play(Animator.StringToHash("IdleDown"));
        }
        else if (fDir.x == 1)
        {
            _animator.Play(Animator.StringToHash("IdleRight"));
			this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (fDir.x == -1)
        {
//            _animator.Play(Animator.StringToHash("IdleLeft"));
			_animator.Play(Animator.StringToHash("IdleRight"));
			this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    //passed by CM_StopFaceDir
    public void StopFaceDir()
    {
        bForceFace = false;
    }


    public void ForceLock(bool setting)
    {
        bForceLock = setting;
        if (moveDir == directions.Up)
        {
            _animator.Play(Animator.StringToHash("IdleUp"));
        }
        else if (moveDir == directions.Down)
        {
            _animator.Play(Animator.StringToHash("IdleDown"));
        }
        else if (moveDir == directions.Right)
        {
            _animator.Play(Animator.StringToHash("IdleRight"));
        }
        else if (moveDir == directions.Left)
        {
            _animator.Play(Animator.StringToHash("IdleLeft"));
        }
    }

    public void ClearFollowTarget()
    {
        FollowTarget = null;
    }

    /// <summary>
    /// Can set the follow target by passing a string rather than a GameObject.
    /// </summary>
    /// <param name="target">The name of the object to follow.</param>
    public void SetFollowTarget(string target)
    {
        FollowTarget = GameObject.Find(target).GetComponent<Movement>();
    }

    //Old, tile-based movement. Unused, and just here for reference.
    //	private void OldMove(){
    //		if (followTarget == null && !player.InCombat && !bNPC)
    //		{
    //			//I use this variable to tell if any input was made
    //			Vector2 moveVector = Vector2.zero;
    //
    //			if (inputDelay <= 0f)
    //			{
    //				inputDelay = 0.3f;
    //				Vector2 moveDelta = Vector2.zero;
    //				if (Input.GetAxis("Vertical") > 0f)
    //				{
    //					moveDelta += Vector2.up;
    //					/*
    //                    sp.UpdateSortingOrder();
    //                    int next = Artifice.Data.GameManager.Instance.PlayerChunk + 1;
    //                    if(transform.position.y > Artifice.Data.GameManager.PLAYER_RESET_THRESHOLD * next) {
    //                        Artifice.Data.GameManager.Instance.PlayerChunk++;
    //                    }
    //                    */
    //					_animator.Play(Animator.StringToHash("GoUp"));
    //					moveDir = directions.Up;
    //				}
    //				if (Input.GetAxis("Horizontal") < 0f)
    //				{
    //					moveDelta += Vector2.left;
    //					_animator.Play(Animator.StringToHash("GoLeft"));
    //					moveDir = directions.Left;
    //				}
    //				if (Input.GetAxis("Vertical") < 0f)
    //				{
    //					moveDelta += Vector2.down;
    //					/*
    //                    sp.UpdateSortingOrder();
    //                    int next = Artifice.Data.GameManager.Instance.PlayerChunk;
    //                    if (transform.position.y < Artifice.Data.GameManager.PLAYER_RESET_THRESHOLD * next) {
    //                        Artifice.Data.GameManager.Instance.PlayerChunk--;
    //                    }
    //                    */
    //					_animator.Play(Animator.StringToHash("GoDown"));
    //					moveDir = directions.Down;
    //				}
    //				if (Input.GetAxis("Horizontal") > 0f)
    //				{
    //					moveDelta += Vector2.right;
    //					_animator.Play(Animator.StringToHash("GoRight"));
    //					moveDir = directions.Right;
    //				}
    //				if (moveDelta == Vector2.zero)
    //				{
    //					inputDelay = 0f;
    //					//Set the correct idle direction
    //					if (moveDir == directions.Up)
    //					{
    //						_animator.Play(Animator.StringToHash("IdleUp"));
    //					}
    //					else if (moveDir == directions.Down)
    //					{
    //						_animator.Play(Animator.StringToHash("IdleDown"));
    //					}
    //					else if (moveDir == directions.Right)
    //					{
    //						_animator.Play(Animator.StringToHash("IdleRight"));
    //					}
    //					else if (moveDir == directions.Left)
    //					{
    //						_animator.Play(Animator.StringToHash("IdleLeft"));
    //					}
    //				}
    //				else
    //				{
    //					followPos = coordinate;
    //					coordinate += moveDelta;
    //				}
    //			}
    //			else
    //			{
    //				inputDelay -= Time.deltaTime;
    //			}
    //
    //			transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(coordinate.x), Mathf.Floor(coordinate.y), 0f), Time.deltaTime * 5f);
    //		}
    //		else if (followTarget != null && !player.InCombat)
    //		{
    //			//Following target code
    //			transform.position = Vector3.MoveTowards(transform.position, new Vector3(Mathf.Floor(followTarget.followPos.x), Mathf.Floor(followTarget.followPos.y), 0f), Time.deltaTime * 5f);
    //
    //			//Sprite stuff
    //			if (followTarget.followPos.x < this.transform.position.x)
    //			{
    //				_animator.Play(Animator.StringToHash("GoLeft"));
    //				moveDir = directions.Left;
    //				waitTimer = waitTimerMax;
    //			}
    //			else if (followTarget.followPos.x > this.transform.position.x)
    //			{
    //				_animator.Play(Animator.StringToHash("GoRight"));
    //				moveDir = directions.Right;
    //				waitTimer = waitTimerMax;
    //			}
    //			else if (followTarget.followPos.y < this.transform.position.y)
    //			{
    //				_animator.Play(Animator.StringToHash("GoDown"));
    //				moveDir = directions.Down;
    //				waitTimer = waitTimerMax;
    //			}
    //			else if (followTarget.followPos.y > this.transform.position.y)
    //			{
    //				_animator.Play(Animator.StringToHash("GoUp"));
    //				moveDir = directions.Up;
    //				waitTimer = waitTimerMax;
    //			}
    //
    //			//Short wait timer so we don't keep hitting idle every time it pops into place
    //			waitTimer -= Time.deltaTime;
    //
    //			if (this.transform.position.x == followTarget.followPos.x && this.transform.position.y == followTarget.followPos.y && waitTimer <= 0f)
    //			{
    //				//				Debug.Log ("We did the thing!");
    //				if (moveDir == directions.Up)
    //				{
    //					_animator.Play(Animator.StringToHash("IdleUp"));
    //				}
    //				else if (moveDir == directions.Down)
    //				{
    //					_animator.Play(Animator.StringToHash("IdleDown"));
    //				}
    //				else if (moveDir == directions.Right)
    //				{
    //					_animator.Play(Animator.StringToHash("IdleRight"));
    //				}
    //				else if (moveDir == directions.Left)
    //				{
    //					_animator.Play(Animator.StringToHash("IdleLeft"));
    //				}
    //			}
    //		}
    //
    //	}


	//Forces a character to play a specific animation. Currently called by CM_PlayAnimation
	public void PlayAnimation(string str){
		_animator.Play(Animator.StringToHash(str));
		bAnimating = true;
	}

	public void EndAnimation(){
		bAnimating = false;
	}


    //properties
    public Movement FollowTarget
    {
        get
        {
            return followTarget;
        }
        set
        {
            followTarget = value;            
        }
    }

    public bool CheckForceLock
    {
        get
        {
            return bForceLock;
        }
    }

	//Lets other scripts know what vector we're pointing. MoveDir really should have been a vector2 rather than an enum
	public Vector2 DirVector
	{
		get {
			if (moveDir == directions.Up) {
				return Vector2.up;
			} else if (moveDir == directions.Left) {
				return Vector2.left;
			} else if (moveDir == directions.Right) {
				return  Vector2.right;
			} else if (moveDir == directions.Down) {
				return Vector2.down;
			} else {
				return Vector2.zero;
				Debug.Log ("DirVector not found!");
			}

		}


	}
}
