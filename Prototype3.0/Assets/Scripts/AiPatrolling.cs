using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrolling : MonoBehaviour {

	//Publics
	public bool floor, leftwall, rightwall, cieling, aroundEdge, flipAtStart, isPatrolling;
	public LayerMask enemyMask;
	public int speed = 1;
	public float raylength;

	//Privates
	private bool isGrounded, isBlocked;
	public bool faceingLeft = true;
	float myWidth, myHeight;

	//Components
	Rigidbody2D myBody;
	Transform myTrans;
	SpriteRenderer mySprite;
	BoxCollider2D myBoxCol;

	void Start ()
	{
		myTrans = this.transform;
		myBody = this.GetComponent<Rigidbody2D>();
		mySprite = this.GetComponent<SpriteRenderer>();
		myBoxCol = this.GetComponent<BoxCollider2D>();

		if (rightwall || leftwall)
		{
			myWidth = myBoxCol.bounds.extents.y;
			myHeight = myBoxCol.bounds.extents.x;

			//This is so we can have the AI move in different direction from beginning
			if (flipAtStart)
			{
				Flip(true);
			}
		}
		else if (floor || cieling)
		{
			myWidth = myBoxCol.bounds.extents.x;
			myHeight = myBoxCol.bounds.extents.y;	
			if (flipAtStart)
			{
				Flip(false);
			}
		}
	}

	void FixedUpdate ()
	{
		
		Move();

		if (isPatrolling)
		{
			//When the AI is walking on the floor
			if (floor)
			{
				//This is the collision detection and lineshooting
				Vector2 startPos = Vector2.up;
				Vector2 groundCheck = Vector2.down;
				Vector2 wallCheck = myTrans.right.toVector2();

				ShootLines(startPos, groundCheck, wallCheck);

				//If the AI has reached an edge or is blocked by a wall
				if(!isGrounded || isBlocked)
				{
					//This is for the AI to go around an edge when blocked
					if (aroundEdge && isBlocked)
					{
						AroundEdge(0, new Vector3(0f,0f, -90));
						floor = false;
						if(faceingLeft)
						{
							leftwall = true;
						}
						else if (!faceingLeft)
						{
							rightwall = true;
						}

					}
					//This is for the AI to go around an edge when egde
					else if (aroundEdge && !isGrounded)
					{
						floor = false;
						AroundEdge(1, new Vector3(0f,0f, 90));	
						if(faceingLeft)
						{
							rightwall = true;
						}
						else if (!faceingLeft)
						{
							leftwall = true;
						}
					}
					//This flips the AI in the opposite direction - for back and forth patrolling.
					else
					{
						Flip(false);
					}
				}
			}
			//When the AI is walking on the cieling
			if (cieling)
			{
				Vector2 startPos = Vector2.down;
				Vector2 groundCheck = Vector2.up;
				Vector2 wallCheck = myTrans.right.toVector2();

				ShootLines(startPos, groundCheck, wallCheck);

				if(!isGrounded || isBlocked)
				{
					if (aroundEdge && isBlocked)
					{
						AroundEdge(0, new Vector3(0f,0f, -90));
						cieling = false;
						if(faceingLeft)
						{
							rightwall = true;
						}
						else if (!faceingLeft)
						{
							leftwall = true;
						}
					}
					else if (aroundEdge && !isGrounded)
					{
						AroundEdge(1, new Vector3(0f,0f, 90));
						cieling = false;
						if(faceingLeft)
						{
							leftwall = true;
						}
						else if (!faceingLeft)
						{
							rightwall = true;
						}
					}
					else 
					{
						Flip(false);	
					}
				}
			}
			//When the AI is walking on a left wall
			if (leftwall)
			{
				Vector2 startPos = Vector2.right;
				Vector2 groundCheck = Vector2.left;
				Vector2 wallCheck = myTrans.right.toVector2();

				ShootLines(startPos, groundCheck, wallCheck);

				if(!isGrounded || isBlocked)
				{
					if (aroundEdge && isBlocked)
					{
						AroundEdge(0, new Vector3(0f,0f, -90));
						leftwall = false;
						if(faceingLeft)
						{
							cieling = true;
						}
						else if (!faceingLeft)
						{
							floor = true;
						}
					}
					else if (aroundEdge && !isGrounded)
					{
						AroundEdge(1, new Vector3(0f,0f, 90));
						leftwall = false;
						if(faceingLeft)
						{
							floor = true;
						}
						else if (!faceingLeft)
						{
							cieling = true;
						}
					}
					else
					{
						Flip(true);	
					}
				}
			}
			//When the AI is walking on a right wall
			if (rightwall)
			{
				Vector2 startPos = Vector2.left;
				Vector2 groundCheck = Vector2.right;
				Vector2 wallCheck = myTrans.right.toVector2();

				ShootLines(startPos, groundCheck, wallCheck);

				if(!isGrounded || isBlocked)
				{
					if (aroundEdge && isBlocked)
					{
						AroundEdge(0, new Vector3(0f,0f, -90));
						rightwall = false;
						if(faceingLeft)
						{
							floor = true;
						}
						else if (!faceingLeft)
						{
							cieling = true;
						}
					}
					else if (aroundEdge && !isGrounded)
					{
						AroundEdge(1, new Vector3(0f,0f, 90));
						rightwall = false;
						if(faceingLeft)
						{
							cieling = true;
						}
						else if (!faceingLeft)
						{
							floor = true;
						}
					}
					else
					{
						Flip(true);	
					}
				}
			}
		}
	}

	//This function shoots lines for collision detection
	void ShootLines(Vector2 startPos, Vector2 groundCheck, Vector2 wallCheck)
	{
		Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * myWidth + startPos * myHeight;

		//Shooting towards ground
		isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + (groundCheck * raylength), enemyMask);
		Debug.DrawLine(lineCastPos, lineCastPos + (groundCheck * raylength), Color.blue);

		//Shooting in front of the AI
		isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - wallCheck * .05f, enemyMask);
		Debug.DrawLine(lineCastPos, lineCastPos - wallCheck * raylength, Color.red);
	}

	//This moves the AI
	void Move()
	{
		myTrans.position -= myTrans.right * speed * Time.deltaTime;
	}

	//This makes the AI turn around a corner
	void AroundEdge(int childInt, Vector3 rotation)
	{
		Vector3 pivot = transform.GetChild(childInt).position;
		transform.Rotate(rotation, Space.Self);
		pivot -= transform.GetChild(childInt).position;
		transform.position += pivot;
	}

	//This flips the AI for patrolling back and forth
	void Flip(bool walls)
	{
		if (faceingLeft)
		{
			faceingLeft = false;
		}
		else if (!faceingLeft)
		{
			faceingLeft = true;
		}
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
