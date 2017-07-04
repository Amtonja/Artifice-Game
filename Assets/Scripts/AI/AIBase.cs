using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour {

	//AI base class.

	protected Player _player; //ref to our Player script
	protected Movement _movement;

	private Vector2 originLocation;

	private bool bWander = true;

	private struct WanderBoundaries
	{
		public float maxX;
		public float minX;
		public float maxY;
		public float minY;
	}

	private WanderBoundaries bounds;

	private int wanderState = 0;
	private float waitTimer = 1f;
	private float waitTimerCurrent = 0f;

	private float moveTimer = 1.5f; //extra timer in case it's running straight into a wall or otherwise can't physically get to its location
	private float moveTimerCurrent = 0f;

	// Use this for initialization
	void Start()
	{
		_player = GetComponent<Player>();
		_movement = GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_player.InCombat || !bWander) {
			waitTimer = 0;
			CombatUpdate ();
		} else {
			Wander ();
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

	}


	//Wandering state machine
	//0 decision making
	//1 waiting to move to position
	//2 wait timer

	public Vector2 wanderPos;
	private void Wander(){
		if (wanderState == 0) {
			int dir = Random.Range(0, 4); //4 is how far it can go? And it can't go to 4? Welp, okay
//			Debug.Log ("random int is " + dir.ToString ());
			wanderPos = this.transform.position;

			//Chose direction
			if (dir == 0) {
				//go up
				//Make sure we're not about to go send it out of bounds
				if (wanderPos.y + 1 > bounds.maxY) {
					wanderPos.y = bounds.maxY;
				} else {
					wanderPos.y += 1;
				}

			} else if (dir == 1) {
				//go right
				if (wanderPos.x + 1 > bounds.maxX) {
					wanderPos.x = bounds.maxX;
				} else {
					wanderPos.x += 1;
				}

			} else if (dir == 2) {
				//go down
				if (wanderPos.y - 1 < bounds.minY) {
					wanderPos.y = bounds.minY;
				} else {
					wanderPos.y -= 1;
				}

			} else if (dir == 3) {
				//go left
				if (wanderPos.x - 1 < bounds.minX) {
					wanderPos.x = bounds.minX;
				} else {
					wanderPos.x -= 1;
				}
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
				waitTimerCurrent = 0;
				wanderState = 0;
			}

		}


	}
	//Called by Movement to signify we hit the end of a position
	private void MoveComplete(){
		Debug.Log ("Wandered into position!");
		wanderState = 2;
	}

	public void CombatStart(){
		//Turn of wandering
		bWander = false;
		//stop forced movement
		_movement.StopForcedMove(false); //if we set this to true it'll activate MoveComplete
	}

	//Overwritten by specific AI
	public virtual void CombatUpdate(){

	}
}
