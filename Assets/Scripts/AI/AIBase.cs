using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour {

	//AI base class.

	protected Player _player; //ref to our Player script
	protected Movement _movement;

	private Vector2 originLocation;

	private bool bWander = true;

	[System.Serializable] //temporary for debugging purposes
	private struct WanderBoundaries
	{
		public float maxX;
		public float minX;
		public float maxY;
		public float minY;
	}

	private WanderBoundaries bounds;

	private Vector2 oldBounds; //For resuming the original boundaries if the player runs away

	private int wanderState = 0;
	private float waitTimer = 1f;
	private float waitTimerCurrent = 0f;

	private float moveTimer = 1.5f; //extra timer in case it's running straight into a wall or otherwise can't physically get to its location
	private float moveTimerCurrent = 0f;

//	private float hitTimer = 1f;
//	private float hitTimerCurrent = 0f;

	private bool bForceUp, bForceDown, bForceLeft, bForceRight = false; //to keep them off of borders

	private bool bHold = false; //used to completely freeze the bot


	private bool bWaitingOnCombatStart = false; //for when going to position and waiting on the combat to start. Prevents wandering overlapping

	// Use this for initialization
	void Start()
	{
		_player = GetComponent<Player>();
		_movement = GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (bHold) {
			return;
		}

//		if (_player.InCombat || !bWander) {
//			
//			CombatUpdate ();
//		} else {
//			Wander ();
//		}
		//To prevent wandering overlap
		if (bWaitingOnCombatStart) {
			return;
		}

		if (_player.InCombat) {
						
			CombatUpdate ();
		} else if (bWander) {
			Wander ();
		} else {
			//do nothing
		}
			
	}

	//Central point of battleground
	public void SetOrigin(Vector2 origin){
		originLocation = origin;
	}

	//The battleground's specific boundaries to wander in
	public void SetBoundries(Vector2 newBounds){
		bounds.maxX = originLocation.x + newBounds.x;
		bounds.minX = originLocation.x - newBounds.x;
		bounds.maxY = originLocation.y + newBounds.y;
		bounds.minY = originLocation.y - newBounds.y;

		//to keep the original input. Yes, this is needlessly recursive
		oldBounds = newBounds;
	}

	public void SetCombatBoundaries(bool bFaceLeft){

		if (bFaceLeft) {
			bounds.minX = originLocation.x;
		} else {
			bounds.maxX = originLocation.x;

		}

	}

	//Called by thhe player script to stop wandering
	public void Stun(){
		waitTimerCurrent = 0;
		wanderState = 2;
		GetComponent<Movement> ().StopForcedMove (false);
	}


	//Wandering state machine
	//0 decision making
	//1 waiting to move to position
	//2 wait timer

	public Vector2 wanderPos;
	public void Wander(){
		if (wanderState == 0) {
			int dir = Random.Range(0, 4); //4 is how far it can go? And it can't go to 4? Welp, okay
//			Debug.Log ("random int is " + dir.ToString ());
			wanderPos = this.transform.position;

			//Chose direction
			if (dir == 0 || bForceUp) {
				//go up
				//Make sure we're not about to go send it out of bounds
				if (wanderPos.y + 1 > bounds.maxY) {
					wanderPos.y = bounds.maxY;
					bForceDown = true;
				} else {
					wanderPos.y += 1;
				}
				bForceUp = false;

			} else if (dir == 1 || bForceRight) {
				//go right
				if (wanderPos.x + 1 > bounds.maxX) {
					wanderPos.x = bounds.maxX;
					bForceLeft = true;
				} else {
					wanderPos.x += 1;
				}
				bForceRight = false;

			} else if (dir == 2 || bForceDown) {
				//go down
				if (wanderPos.y - 1 < bounds.minY) {
					wanderPos.y = bounds.minY;
					bForceUp = true;
				} else {
					wanderPos.y -= 1;
				}
				bForceDown = false;

			} else if (dir == 3 || bForceLeft) {
				//go left
				if (wanderPos.x - 1 < bounds.minX) {
					wanderPos.x = bounds.minX;
					bForceRight = true;
				} else {
					wanderPos.x -= 1;
				}
				bForceLeft = false;
			} else { 
				Debug.Log ("AI wandering direction out of range, what the heck");
			}

			//send to movement script
			_movement.StartForcedMove (wanderPos);
			_movement.GetForcedSender (this.gameObject);

			wanderState = 1;

		} else if (wanderState == 1) {
			moveTimerCurrent += Time.deltaTime;
			if (moveTimerCurrent >= moveTimer) {
				//we're trying to move to an inaccessable location, so continue from here
				moveTimerCurrent = 0f;
	
				_movement.StopForcedMove (false);
				wanderState = 2;
			}

		} else if (wanderState == 2) {
			waitTimerCurrent += Time.deltaTime;
			if (waitTimerCurrent >= waitTimer) {
				waitTimerCurrent = 0f;
				wanderState = 0;
			}

		}


	}
	//Called by Movement to signify we hit the end of a position
	private void MoveComplete(){
//		Debug.Log ("Wandered into position!");
		wanderState = 2;
	}

	//Called by Battleground
	public void CombatStart(bool enemyFaceDirection){
		//Turn of wandering
		bWander = false;
		//stop forced movement
		if (_movement == null) { //in case we added this script after the fact, meaning Start didn't run
			_movement = GetComponent<Movement> ();
		}
		_movement.StopForcedMove(false); //if we set this to true it'll activate MoveComplete
		//update borders for combat
		SetCombatBoundaries (enemyFaceDirection);

	}

	//called to reset behavior if player runs from fight
	public void Reset(){

		bWander = true;
		SetBoundries (oldBounds);
		GetComponent<Movement> ().StopForcedMove (false);
	}

	//Overwritten by specific AI
	public virtual void CombatUpdate(){

	}

	public virtual void ResumeWander(){}

	public bool BHold {
		set {
			bHold = value;
		}
	}

	public bool BWander {
		set {
			bWander = value;
		}
	}

	public bool BWaitingOnCombatStart {
		set {
			bWaitingOnCombatStart = value;
		}
	}
}
