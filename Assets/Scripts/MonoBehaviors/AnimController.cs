using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {

	//Handles all character animation

	//By default, movement 

	//Used to tell what direction we're facin' for idle sprites
	public enum directions { Up, Down, Left, Right };
	private directions moveDir;

	private Animator _animator;
	/*
		Animation states:
		0 = default movement. Animation is based on movement vectors sent to it.
		1 = combat. 
		?2 = manual movement. Used for enemies that shuffle around rather than moving in the direction they're facing. 
		99 = full manual. Used for full manual control - plays whatever animation is set to it and waits. Used by cutscenes.
	*/
	private int animState = 0;


	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>(); //Might need to be in Awake
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	public Vector3 tempDelta;
	//called by Movement to animate walking around. Takes the vector3 input and determines what direction to walk and what direction to face to stop.
	public void Move(Vector3 moveDelta){
//		tempDelta = moveDelta;
		if (moveDelta.x == 0f && moveDelta.y == 0f)
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

		if (moveDelta.y > 0.5f)
		{
//			moveDelta.y = 1f;
			//To avoid overlapping animations
			if (moveDelta.x == 0f)
				_animator.Play(Animator.StringToHash("GoUp"));
			moveDir = directions.Up;
		}
		else if (moveDelta.y < -0.5f)
		{
//			moveDelta = Vector2.down;
			if (Input.GetAxis("Horizontal") == 0f)
				_animator.Play(Animator.StringToHash("GoDown"));
			moveDir = directions.Down;

		}
		if (moveDelta.x > 0.5f)
		{
//			moveDelta += Vector2.right;
			_animator.Play(Animator.StringToHash("GoRight"));
			this.transform.localScale = new Vector3(1, 1, 1);
			moveDir = directions.Right;
		}
		else if (moveDelta.x < -0.5f)
		{
//			moveDelta += Vector2.left;
			//            _animator.Play(Animator.StringToHash("GoLeft"));
			_animator.Play(Animator.StringToHash("GoRight"));
			this.transform.localScale = new Vector3(-1, 1, 1);

			moveDir = directions.Left;
		}


	}
}
