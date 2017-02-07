using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrolling : MonoBehaviour {

	public bool floor, leftwall, rightwall, cieling, aroundEdge;
	public LayerMask enemyMask;
	public int speed = 1;
	public float raylength;

	private bool isGrounded, isBlocked;

	Rigidbody2D myBody;
	Transform myTrans;
	float myWidth, myHeight;

	void Start ()
	{
		myTrans = this.transform;
		myBody = this.GetComponent<Rigidbody2D>();
		SpriteRenderer mySprite = this.GetComponent<SpriteRenderer>();

		if (rightwall || leftwall)
		{
			myWidth = mySprite.bounds.extents.y;
			myHeight = mySprite.bounds.extents.x;	
		}
		else if (floor || cieling)
		{
			myWidth = mySprite.bounds.extents.x;
			myHeight = mySprite.bounds.extents.y;	
		}
	}

	void FixedUpdate ()
	{

		if (floor)
		{
			Vector2 startPos = Vector2.up;
			Vector2 groundCheck = Vector2.down;
			Vector2 wallCheck = myTrans.right.toVector2();

			ShootLines(startPos, groundCheck, wallCheck);

			float direction = -myTrans.right.x;
			Move(direction, true);
			Debug.Log("(floor)My Width: " + myWidth);
			Debug.Log("(floor)My height: " + myHeight);

			if(!isGrounded || isBlocked)
			{
				Flip(false);
			}

		}
		if (cieling)
		{
			Vector2 startPos = Vector2.down;
			Vector2 groundCheck = Vector2.up;
			Vector2 wallCheck = myTrans.right.toVector2();

			ShootLines(startPos, groundCheck, wallCheck);

			float direction = -myTrans.right.x;
			Move(direction, true);

			if(!isGrounded || isBlocked)
			{
				Flip(false);
			}
		}

		if (leftwall)
		{
			Vector2 startPos = Vector2.right;
			Vector2 groundCheck = Vector2.left;
			Vector2 wallCheck = myTrans.right.toVector2();

			ShootLines(startPos, groundCheck, wallCheck);

			float direction = -myTrans.right.y;
			Move(direction, false);

			if(!isGrounded || isBlocked)
			{
				Flip(true);
			}
		}

		if (rightwall)
		{
			Vector2 startPos = Vector2.left;
			Vector2 groundCheck = Vector2.right;
			Vector2 wallCheck = myTrans.right.toVector2();

			ShootLines(startPos, groundCheck, wallCheck);

			float direction = -myTrans.right.y;
			Move(direction, false);

			Debug.Log("(rightwall)My Width: " + myWidth);
			Debug.Log("(rightwall)My height: " + myHeight);

			if(!isGrounded || isBlocked)
			{
				Flip(true);
			}
		}
	}

	void ShootLines(Vector2 startPos, Vector2 groundCheck, Vector2 wallCheck)
	{



		Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * myWidth + startPos * myHeight;

		isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + (groundCheck * raylength), enemyMask);
		Debug.DrawLine(lineCastPos, lineCastPos + (groundCheck * raylength), Color.blue);

		//Check to see if there's a wall in front of us before moving forward

		isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - wallCheck * .05f, enemyMask);
		Debug.DrawLine(lineCastPos, lineCastPos - wallCheck * raylength, Color.red);
	}

	void Move(float direction, bool horizontal)
	{
		Vector2 myVel = myBody.velocity;

		if (horizontal)
		{
			Vector2 vel = new Vector2(direction * speed, 0f);
			//myVel.x = direction * speed;
			//myBody.velocity = myVel;
			myBody.velocity = vel;
			

		}
		else if (!horizontal)
		{
			myVel.y = direction * speed;
			myBody.velocity = myVel;
		}
	}

	void Flip(bool walls)
	{
		if (!walls)
		{
			Vector3 currRot = myTrans.eulerAngles;
			currRot.y += 180;
			myTrans.eulerAngles = currRot;
		}
		else if (walls)
		{
			Vector3 currRot = myTrans.eulerAngles;
			currRot.x += 180;
			myTrans.eulerAngles = currRot;

		}
	}
}
