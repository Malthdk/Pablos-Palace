using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
	
	public float maxJumpHeight = 3.5f;				//Max JumpHeight
	public float minJumpHeight = 1f;				//Minimum JumpHeight
	public float purpMinJumpVelocity = -9.3f;		//Minimum jump velocity when purple - have to be defined here cause of mathematical equation
	public float timeToJumpApex = .65f;				//Time to reach highest point
	public float accelerationTimeAirborn = .2f;		//Acceleration while airborne
	public float accelerationTimeGrounded = .5f;	//Acceleration while grounded
	public float moveSpeed = 9;	
	[HideInInspector]
	public float airSpeed = 8.4f, groundSpeed = 11.4f;
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
	private float targetX;
	private float targetY;

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

		targetX = -wallDirX * abilities.wallJumpClimb.x;
		targetY = abilities.wallJumpClimb.y;

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
			accelerationTimeGrounded = 0.05f;
		}

		if(!abilities.isDashing && !abilities.isDownDashing && !Swimming.instance.isSwimming )
		{
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below || gameObject.tag == "purple" && controller.collisions.above)?accelerationTimeGrounded:accelerationTimeAirborn);		//Calculating velocity x both airborn and on ground with smoothing
		}

		animator.SetFloat("Speed", Mathf.Abs(velocity.x));		//ANIMATION
		animator.SetBool("Ground", controller.collisions.below && gameObject.tag != "purple" || controller.collisions.above && gameObject.tag == "purple");	//ANIMATION
		animator.SetFloat("vSpeed", velocity.y);				//ANIMATION
		animator.SetBool("OnWall", wallSliding);
		animator.SetBool("Dashing", abilities.isDashing);
		animator.SetBool ("Soaring", abilities.soaring);

		//WALLSLIDING WITH YELLOW
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && abilities.notJumping && abilities.isYellow)		//havde && velocity.y <0 efter && og før gameObject.tag =="yellow"	//setting wether player is on wall
		{
			abilities.soaring = false;
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

		if (Input.GetButtonDown("Jump"))
		{
			if (controller.collisions.below && !pullPush.isPulling && !Swimming.instance.isSwimming)
			{
				velocity.y = maxJumpVelocity;
				animator.SetBool("Ground", false);
			}
			if (controller.collisions.above && gameObject.tag == "purple")
			{
				velocity.y = maxJumpVelocity;
			}

			if(!controller.collisions.below && (abilities.isBlue || abilities.isGreen) )
			{
				/*This is where double jump is handled*/
				if (!abilities.hasDoubleJumped && !wallSliding && !abilities.isPurple)
				{
					abilities.secondJump = true;
					Debug.Log ("double jumped");
					abilities.soaring = false;
					abilities.hasDoubleJumped = true;
					velocity.y = maxJumpVelocity/1.3f;
					abilities.startRotation();
					
				}
				else if (abilities.isPurple && !controller.collisions.above)
				{
					abilities.secondJump = true;

					if (!abilities.hasDoubleJumped)
					{
						abilities.hasDoubleJumped = true;
						velocity.y = maxJumpVelocity/1.3f;
						abilities.startRotation();
					}
				}
			}
			if (wallSliding)
			{
				if (wallDirX == input.x)						//If input is towards the wall
				{
					abilities.notJumping = false;

					//Tried to add smoothness to the ability by SmoothDamp - didnt work. Tried afterwards with maxJumpVelocity - worked but not apparent difference. 
					//The reason it looks wierd could simply be due to the animations state machine. 
					velocity.x = -wallDirX * abilities.wallJumpClimb.x;
					velocity.y = abilities.wallJumpClimb.y;
					if (controller.collisions.above  || controller.collisions.below)		//If raycasts hit above or below, velocity on y axis stops
					{
						velocity.y = 0;
					}
				}
				if (input.x == 0)								
				{
					abilities.notJumping = false;
					velocity.x = -wallDirX * abilities.wallLeap.x;
					velocity.y = abilities.wallLeap.y;
				}
				else if (wallDirX != input.x)					//If input is away from wall
				{
					abilities.notJumping = false;
					velocity.x = -wallDirX * abilities.wallLeap.x;
					velocity.y = abilities.wallLeap.y;
				}
			}
		}

		if (!controller.collisions.below && !pullPush.isPulling)
		{
			moveSpeed = airSpeed;
		}
		else if (!pullPush.isPulling)
		{
			abilities.secondJump = false;
			abilities.soaring = false;
			moveSpeed = groundSpeed;
		}
		if (abilities.isPurple && controller.collisions.above)
		{
			abilities.secondJump = false;
		}

		if (Input.GetButtonUp("Jump"))					//For variable jump
		{
			if (velocity.y > minJumpVelocity && gameObject.tag != "purple" && !abilities.secondJump)
			{
				velocity.y = minJumpVelocity;									//When space is released set velocity y to minimum jump velocity
			}
			if (velocity.y < purpMinJumpVelocity && gameObject.tag == "purple" && !abilities.secondJump)  //For variable jump when purple
			{
				velocity.y = purpMinJumpVelocity;
			}
		}
			
		if(!abilities.isDashing && !abilities.soaring && !abilities.isDownDashing && !WaterTop.instance.onSurface)
		{
			velocity.y += gravity * Time.deltaTime;							//Applies velocity to gravity
		}

		controller.Move(velocity * Time.deltaTime, input);				//Moving character

		if (controller.collisions.above  || controller.collisions.below || abilities.isDashing == true || WaterTop.instance.onSurface)		//If raycasts hit above or below, velocity on y axis stops
		{
			velocity.y = 0;
		}
	}
}
