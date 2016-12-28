using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	private Player player;
	private Vector2 coordinate;
	private Vector2 followPos;

	public Movement followTarget;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(followTarget == null) {
			Vector2 moveVector = Vector2.zero;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				moveVector += Vector2.up;
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				moveVector += Vector2.left;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				moveVector += Vector2.down;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				moveVector += Vector2.right;
			}

			if (moveVector != Vector2.zero) {
				transform.Translate(moveVector*Time.deltaTime*3f);
				followPos = (Vector2)transform.position - moveVector/2f;
			}
	//		else {
	//			transform.position = Vector3.MoveTowards(transform.position,
	//				new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0f),
	//				Time.deltaTime);
	//		}
		} else {
			Vector3 newPos = Vector3.MoveTowards(transform.position, followTarget.followPos, Time.deltaTime*(1+Vector3.Distance(transform.position, followTarget.followPos)*2f));
			if (newPos - transform.position != Vector3.zero) {
				followPos = transform.position - (newPos - transform.position).normalized/2f;
				transform.position = newPos;
			}
		}
	}
}
