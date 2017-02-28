using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
	
	public float maxJumpHeight = 3.5f;				//Max JumpHeight
	public float minJumpHeight = 1f;				//Minimum JumpHeight
	public float purpMinJumpVelocity = -9.3f;		//Minimum jump velocity when purple - have to be defined here cause of mathematical equation
	public float timeToJumpApex = .65f;				//Time to reach highest point (4875 was before)
	public float accelerationTimeAirborn = .2f;		//Acceleration while airborne
	public float accelerationTimeGrounded = .5f;	//Acceleration while grounded
	public float moveSpeed = 9;	
	[HideInInspector]
	public float airSpeed = 6.4f, groundSpeed = 9.4f;
	[HideInInspector]
	public Vector2 input;
	[HideInInspector]
	public int wallDirX;

	//FOR WALLSLIDING
	public float wallSlideSpeedMax = 0.01f;		//Maximum wall spide speed
	private float wallStickTime = 1f;		//Time before releasing from wall when input is away from wall
	float timeToWallUnstick;
	public bool wallSliding;

	public float wjXSmoothing;
	public float wjYSmoothing;
	private float wjAcceleration = 0.05f;

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
	private PullPush pullPush;
	[HideInInspector]
	public bool doubleJumped, tripleJumped, hasTripleJumped = false, hasDoubleJumped = false;
	private float doubleJumpVelocity;
	private float tripleJumpVelocity;

	[HideInInspector]
	public static Player _instance;

	public static Player instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<Player>();
			}
			return _instance;
		}
	}

	void Start () 
	{
		pullPush = GetComponent<PullPush>();
		controller = GetComponent<Controller2D>();
		abilities = GetComponent<Abilities>();
		animator = GetComponent<Animator>();		//ANIMATION
	}

	void Update () 
	{

		gravity = -(2* maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);		//Gravity defined based of jumpheigmaxJumpVelocityto reach highest point
	
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;				//Max jump velocity defined based on gravity and time to reach highest point
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);	//Min jump velocity defined based on gravity and min jump height

		input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));		//Input from player
		wallDirX = (controller.collisions.left)? -1:1;											//wall direction left or right

		float targetVelocityX = input.x * moveSpeed;							//velocity on x axis

		if (input.x == 1 || input.x == -1) //[BUG REPORT] Minor problem with changing direction and keeping same speed
		{
			accelerationTimeGrounded = 0.25f;
		}
		else
		{
			accelerationTimeGrounded = 0.05f; //0.05
		}
			
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborn);		//Calculating velocity x both airborn and on ground with smoothing

		animator.SetFloat("Speed", Mathf.Abs(velocity.x));		//ANIMATION
		animator.SetBool("Ground", controller.collisions.below);	//ANIMATION
		animator.SetFloat("vSpeed", velocity.y);				//ANIMATION
		//animator.SetBool("OnWall", wallSliding);
		//animator.SetBool("Dashing", abilities.isDashing);
		//animator.SetBool ("Soaring", abilities.soaring);
		animator.SetBool("DoubleJump", doubleJumped);
		animator.SetBool("TripleJump", tripleJumped);

		if (Input.GetButtonDown("Jump"))
		{
			if (controller.collisions.below && !pullPush.isPulling && !Swimming.instance.isSwimming)
			{
				velocity.y = maxJumpVelocity;
				animator.SetBool("Ground", false);
			}

			if(!controller.collisions.below)
			{
				/*This is where double jump is handled*/
				if (!hasDoubleJumped)
				{
					Debug.Log ("double jumped");
					doubleJumped = true; //This causes an animation bugg where we jump from a wall and then doublejump is is set to true so we canno transition into jump animation.
					hasDoubleJumped = true;
					//abilities.secondJump = true;
					velocity.y = doubleJumpVelocity;

					
				}
				else if (!hasTripleJumped && hasDoubleJumped)
				{
					Debug.Log ("tripple jumped");
					tripleJumped = true; //animation
					hasTripleJumped = true; 
					velocity.y = tripleJumpVelocity;
				}
			}
		}

		if (!controller.collisions.below && !pullPush.isPulling)
		{
			moveSpeed = airSpeed;
		}
		else if (controller.collisions.below)
		{
			hasTripleJumped = false;
			hasDoubleJumped = false;
			tripleJumped = false; //animation
			doubleJumped = false; //animation
			moveSpeed = groundSpeed;
		}
		if (Mathf.Sign(velocity.y) == -1)
		{
			timeToJumpApex = 0.75f; //55
		}
		else 
		{
			timeToJumpApex = 0.92f;	//72
			doubleJumpVelocity = maxJumpVelocity/1.4f;
			tripleJumpVelocity = maxJumpVelocity/1.2f;
		}

		if (Input.GetButtonUp("Jump"))					//For variable jump
		{
			if (velocity.y > minJumpVelocity)
			{
				velocity.y = minJumpVelocity;									//When space is released set velocity y to minimum jump velocity
			}
		}

		velocity.y += gravity * Time.deltaTime;							//Applies velocity to gravity

		controller.Move(velocity * Time.deltaTime, input);				//Moving character

		if (controller.collisions.above  || controller.collisions.below)		//If raycasts hit above or below, velocity on y axis stops
		{
			velocity.y = 0;
		}
	}
}