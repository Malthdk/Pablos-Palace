﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
	
	public float maxJumpHeight = 3.5f;				//Max JumpHeight
	public float minJumpHeight = 1f;				//Minimum JumpHeight
	public float purpMinJumpVelocity = -9.3f;		//Minimum jump velocity when purple - have to be defined here cause of mathematical equation
	public float timeToJumpApex = .65f;				//Time to reach highest point
	public float accelerationTimeAirborn = .2f;		//Acceleration while airborne
	public float accelerationTimeGrounded = .5f;	//Acceleration while grounded
	public float moveSpeed = 9;						//Player movement speed
	private float airSpeed = 8.4f;
	private float groundSpeed = 11.4f;
	[HideInInspector]
	public Vector2 input;
	[HideInInspector]
	public int wallDirX;

	//FOR WALLSLIDING
	public float wallSlideSpeedMax = 0.01f;		//Maximum wall spide speed
	private float wallStickTime = 1f;		//Time before releasing from wall when input is away from wall
	float timeToWallUnstick;
	public bool wallSliding;

	public bool hasJumped;
	public float gravity;					//gramaxJumpVelocity to player
	public float maxJumpVelocity;			//Max jump velocity
	public float minJumpVelocity;			//Min jump velocity
	public Vector3 velocity;				//velocity
	[HideInInspector]
	public float velocityXSmoothing;		//smoothing on velocity

	Controller2D controller;					//calling controller class
	Abilities abilities;


	private Animator animator;		//ANIMATION

	void Start () 
	{
		controller = GetComponent<Controller2D>();
		abilities = GetComponent<Abilities>();
		animator = GetComponent<Animator>();		//ANIMATION
	}

	void Update () 
	{

		//Debug.Log (abilities.isRed);

	
		gravity = -(2* maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);		//Gravity defined based of jumpheigmaxJumpVelocityto reach highest point

//		if (velocity.y < 0 && abilities.isGreen) 
//		{							// Sets gravity for red ability
//			gravity = -4.5f;
//			if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
//			{
//				gravity = -(2* maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);		
//			}
//		}

		/*if (velocity.y < 0 && abilities.isRed && !abilities.isPurple) 
		{							// Sets gravity for red ability
			gravity = -4.5f;
			if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			{
				gravity = -(2* maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);		
			}
		}
		if (velocity.y > 0 && abilities.isPurple) 
		{							// Sets gravity for red ability
			gravity = 6.5f;
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			{
				gravity = -(2* maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);		
			}
		}*/

	
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;				//Max jump velocity defined based on gravity and time to reach highest point
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);	//Min jump velocity defined based on gravity and min jump height

		input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));		//Input from player
		wallDirX = (controller.collisions.left)? -1:1;												//wall direction left or right

		float targetVelocityX = input.x * moveSpeed;							//velocity on x axis

		if (input.x == 1 || input.x == -1) //[BUG REPORT] Minor problem with changing direction and keeping same speed
		{
			accelerationTimeGrounded = 0.25f;
		}
		else
		{
			accelerationTimeGrounded = 0.05f;
		}

		if(!abilities.isDashing)
		{
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below || gameObject.tag == "purple" && controller.collisions.above)?accelerationTimeGrounded:accelerationTimeAirborn);		//Calculating velocity x both airborn and on ground with smoothing
		}

		animator.SetFloat("Speed", Mathf.Abs(velocity.x));		//ANIMATION
		animator.SetBool("Ground", controller.collisions.below && gameObject.tag != "purple" || controller.collisions.above && gameObject.tag == "purple");	//ANIMATION
		animator.SetFloat("vSpeed", velocity.y);				//ANIMATION
		animator.SetBool("OnWall", wallSliding);
		animator.SetBool("Dashing", abilities.isDashing);
		animator.SetBool ("Soaring", abilities.inAir);

		//Debug.Log(controller.collisions.above);


//		//WALLSLIDING WITH YELLOW
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && abilities.notJumping && abilities.isYellow)		//havde && velocity.y <0 efter && og før gameObject.tag =="yellow"	//setting wether player is on wall
		{
			abilities.inAir = false;
			wallSliding = true;
			if (velocity.y < -wallSlideSpeedMax)	
			{
				velocity.y = -wallSlideSpeedMax;		// Making it so we can never surpass wall slide speed max
			}
			if (timeToWallUnstick > 0)
			{
				velocityXSmoothing = 0;
				velocity.x = 0;								
				if ( input.x != wallDirX && input.x != 0)   //If input is away from wall and not equal to zero
				{
					timeToWallUnstick -= Time.deltaTime;	//Then we start counting down timeToWallUnstick
				}
				else 
				{
					timeToWallUnstick = wallStickTime;		
				}
			}
			else 
			{
				timeToWallUnstick = wallStickTime;

			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && animator.GetBool("Ground"))
		{
			if (controller.collisions.below)
			{
				velocity.y = maxJumpVelocity;
				animator.SetBool("Ground", false);
			}
			if (controller.collisions.above && gameObject.tag == "purple")
			{
				velocity.y = maxJumpVelocity;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Space) && !controller.collisions.below && (abilities.isBlue || abilities.isGreen) )
		{
			abilities.secondJump = true;
			if (!abilities.hasDoubleJumped && !wallSliding && !abilities.isPurple)
			{
				Debug.Log ("double jumped");
				abilities.inAir = false;
				abilities.hasDoubleJumped = true;
				velocity.y = maxJumpVelocity/1.3f;
				abilities.startRotation();
				
			}
			else if (abilities.isPurple && !controller.collisions.above && !abilities.hasDoubleJumped)
			{
				abilities.hasDoubleJumped = true;
				velocity.y = maxJumpVelocity/1.3f;
				abilities.startRotation();
				
			}
		}

		if (!controller.collisions.below)
		{
			moveSpeed = airSpeed;
			Debug.Log ("No collisions below");
		}
		else
		{
			abilities.secondJump = false;
			abilities.inAir = false;
			moveSpeed = groundSpeed;
		}

		if (Input.GetKeyUp(KeyCode.Space))					//For variable jump
		{
			if (velocity.y > minJumpVelocity && gameObject.tag != "purple" && !abilities.secondJump)
			{
				velocity.y = minJumpVelocity;									//When space is released set velocity y to minimum jump velocity
			}
			if (velocity.y < purpMinJumpVelocity && gameObject.tag == "purple")  //For variable jump when purple
			{
				velocity.y = purpMinJumpVelocity;
			}

		}

		//SPRINTING
//		if (Input.GetKeyDown(KeyCode.LeftShift))
//		{
//			moveSpeed = 17f;
//		}
//		if (Input.GetKeyUp(KeyCode.LeftShift))
//		{
//			moveSpeed = 11.5f;
//		}
		if(!abilities.isDashing && !abilities.inAir)
		{
			velocity.y += gravity * Time.deltaTime;							//Applies velocity to gravity
		}

		controller.Move(velocity * Time.deltaTime, input);				//Moving character

		if (controller.collisions.above  || controller.collisions.below || abilities.isDashing == true )		//If raycasts hit above or below, velocity on y axis stops
		{
			velocity.y = 0;
		}
	}
}
