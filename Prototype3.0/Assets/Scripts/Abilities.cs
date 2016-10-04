using UnityEngine;
using System.Collections;

public class Abilities : MonoBehaviour {

	private Color red = new Color(0.82F, 0F, 0.2F);			//Red farve
	private Color yellow = new Color(1F, 0.8F, 0.2F);		//Yellow farve
	private Color blue = new Color(0.2F, 0.18F, 1F);		//Blue farve
	private Color green = new Color(0.2F, 0.8F, 0.15F);		//Green farve
	private Color purple = new Color(0.55F, 0.15F, 0.7F);	//Purple farve
	private Color orange = new Color(1F, 0.6F, 0.2F);		//Orange farve

	//COLOR BOOLS
	public bool isBlue = false;
	public bool isRed = false;
	public bool isYellow = false;
	public bool isGreen = false;
	public bool isOrange = false;
	public bool isPurple = false;

	//FOR DOUBLE JUMO
	public bool hasDoubleJumped;
	public float duration = 0.5f;
	float t = 0;
	public float time = 0;
	public bool ifRotating = false;
	public bool notRotating;
	public float start = 0f;
	public bool secondJump;

	//FOR OlD DASH
	public DashState dashState;
	public float dashTimer;
	public float maxDash = 20f;
	public float minDash = 0.5f;
	private float tempMinDash;
	public Vector2 savedVelocity;
	private TrailRenderer trailRenderer;
	public bool isDashing;
	public bool canDestroy;
	private bool isDashKeyDown;
	private float maxDashVelocity = 30f;

	public bool singleDash; //For testing dash
	public bool longDash;	//For testing dash
	//FOR DOWN DASH
	public DownDashState downDashState;
	public float waitTime = 0.5f;
	private float tempWaitTime;
	private float maxDownDashVelocity = 40f;

	//NEW DASH ORANGE

	//public float dashTime = 0.5f;

	//NEW DASH ORANGE
	public bool isOrangeDashing;
	private float orangeMaxDashVelocity = 45f;
	public float orangeDashTime;
	public bool isdowndashing;

	//FOR GREEN
	private Vector3 zeroVel = new Vector3(0,0,0);
	private Vector3 storedVel;
	private Vector3 freezePos;
	public bool inAir = false;
	public float floatingVelocity;
	[HideInInspector]
	public float currentVelocityY;
	
	//FOR WALLJUMP
	private Vector2 wallJumpClimb = new Vector2(12f, 25.6f); 			//1. iteration of walljump (input towards wall + space) 7.5f, 16f. 2nd iteration: 12f, 25.6f 3rd. 10.5f, 22.4
	private Vector2 wallLeap = new Vector2(26.4f, 24.9f);					//3. iteration of walljump (input away from wall + space) 18f 17f. 2nd 26.4f, 24.9f 3rd. 23.1f,21.8f
	public bool notJumping;

	private SpriteRenderer renderer;
	Player player;						//Calling player class
	Controller2D controller;			//Calling controller class
	private Transform graphicsTransform;

	private bool hasBeenReset = true;
	//[HideInInspector]
	//public bool isdashing = false;

	private bool gravityReversed = false;
	private bool normalGravity = true;		//For flipping character when purple	

	//RESIZING
	private Vector3 orangeSize = new Vector3(0.45f, 0.45f, 0.45f);
	private Vector3 greenSize = new Vector3(0.2f, 0.2f, 0.2f);

	void Start () 
	{
		hasDoubleJumped = false;
		controller = GetComponent<Controller2D>();
		player = GetComponent<Player>();
		renderer = gameObject.transform.FindChild("Graphics").GetComponent<SpriteRenderer>();
		graphicsTransform = gameObject.transform.FindChild("Graphics").GetComponent<Transform>();

		//For dash
		trailRenderer = gameObject.GetComponent<TrailRenderer>();
		tempWaitTime = waitTime;
		tempMinDash = minDash;
	}

	public enum DashState 
	{
		Ready,
		Dashing,
		Cooldown
	}
	public enum DownDashState 
	{
		Ready,
		Waiting,
		Dashing,
		Cooldown
	}

	void Update () 
	{
																	////////////////////////
																	////PLAYER IS BLUE//////
																	////////////////////////
		if (isBlue)
		{
			if (controller.collisions.below && hasDoubleJumped || isGreen && player.wallSliding)
			{
				hasDoubleJumped = false;
			}
			else if (gameObject.tag == "purple" && controller.collisions.above && hasDoubleJumped)
			{
				hasDoubleJumped = false;
			}
			
//			if (Input.GetKeyDown(KeyCode.Space) && !controller.collisions.below)
//			{
//				if (!hasDoubleJumped && !player.wallSliding && !isPurple)
//				{
//					Debug.Log ("double jumped");
//					inAir = false;
//					hasDoubleJumped = true;
//					player.velocity.y = player.maxJumpVelocity/1.3f;
//					startRotation();
//					
//				}
//				else if (isPurple && !controller.collisions.above && !hasDoubleJumped)
//				{
//					hasDoubleJumped = true;
//					player.velocity.y = player.maxJumpVelocity/1.3f;
//					startRotation();
//					
//				}
//			}
			
			if (ifRotating)
			{
				
				time += Time.deltaTime;
				t = time / duration;
				Vector3 angle = graphicsTransform.eulerAngles;
				
				graphicsTransform.rotation = Quaternion.Euler(angle.x, angle.y, Mathf.LerpAngle(start, start + 180, t));
				if (time >= duration)
				{
					float zrot = graphicsTransform.rotation.eulerAngles.z;
					graphicsTransform.rotation = Quaternion.Euler(angle.x, angle.y, Mathf.LerpAngle(zrot, zrot - 180, t));
					ifRotating = false;
					notRotating = true;
				}
			}
			
//			if (Input.GetKeyUp(KeyCode.Space))					//For variable jump
//			{
//				if (player.velocity.y > player.minJumpVelocity)
//				{
//					player.velocity.y = player.minJumpVelocity;									//When space is released set velocity y to minimum jump velocity
//				}
//			}
		}
																////////////////////////
																////PLAYER IS RED///////
																////////////////////////
		if (isRed)
		{
			//player.gravity = -(2* player.maxJumpHeight) / Mathf.Pow (player.timeToJumpApex, 2);		//Gravity defined based of jumpheigmaxJumpVelocityto reach highest point
			//if (player.velocity.y < 0 && isRed) {
			//	player.gravity = -5f;
			//	if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			//	{
			//		player.gravity = -(2* player.maxJumpHeight) / Mathf.Pow (player.timeToJumpApex, 2);		
			//	}
			//}

			if (controller.collisions.below && hasDoubleJumped || player.wallSliding)
			{
				hasDoubleJumped = false;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				isDashKeyDown = true;
			}
			else
			{
				isDashKeyDown = false;
			}
			//For dash
			switch (dashState) 
			{
			case DashState.Ready:
				if(isDashKeyDown && !player.wallSliding && !hasDoubleJumped)
				{
					isDashing = true;
					canDestroy = true;
					EnableTrailRenderer();
					dashState = DashState.Dashing;
				}
				break;
			case DashState.Dashing:
				dashTimer += Time.deltaTime * 3;
				if(dashTimer >= maxDash)
				{
					hasDoubleJumped = true;
					dashTimer = maxDash;
					if(controller.collisions.faceDir == 1)
					{
						player.velocity.x = maxDashVelocity;
					}
					else if(controller.collisions.faceDir == -1)
					{
						player.velocity.x = -maxDashVelocity;
					}

					//Eternal dashing as long as shift is held down
					if (longDash)
					{
						minDash -= Time.deltaTime;
						if (minDash <= 0f)
						{
							minDash = 0f;
						}
						if (!isDashKeyDown && minDash == 0) //Seems to be 0.5 sec delay on release of shift which causes bad control.
						{
							dashState = DashState.Cooldown;
						}
					}
					//One dash when pressing dashbutton
					if (singleDash)
					{
						dashState = DashState.Cooldown;
					}
				}
				break;
			case DashState.Cooldown:
				dashTimer -= Time.deltaTime;
				if(dashTimer <= 0)
				{
					isDashing = false;
					canDestroy = false;
					dashTimer = 0;
					minDash = tempMinDash;
					dashState = DashState.Ready;
					DisableTrailRenderer();

					//Bad fix for having the player retain some velocity after a dash. This could also be 0 for instant stop.
					if (controller.collisions.faceDir == 1)
					{
						player.velocity.x = 10f;
					}
					else if (controller.collisions.faceDir == -1)
					{
						player.velocity.x = -10f;
					}
				}
				break;
			}

			//For downward burst
			switch (downDashState) 
			{
			case DownDashState.Ready:
				var isDashKeyDown = Input.GetKeyDown (KeyCode.DownArrow);
				
				if(isDashKeyDown && !controller.collisions.below && !player.wallSliding)
				{
					//isDashing = true;
					EnableTrailRenderer();
					isDashing = true;
					player.velocity.x = 0;
					downDashState = DownDashState.Waiting;
				}
				break;
			case DownDashState.Waiting:
				tempWaitTime -= Time.deltaTime;
				if (tempWaitTime <= 0)
				{
					Debug.Log ("Switching state");
					downDashState = DownDashState.Dashing;
				}
				break;
			case DownDashState.Dashing:
				dashTimer += Time.deltaTime * 3;
				isDashing = false;
				canDestroy = true;
				if(dashTimer >= maxDash)
				{
					//isDashing = true;
					dashTimer = maxDash;
					player.velocity.y = -maxDownDashVelocity;
					if (controller.collisions.below)
					{
						downDashState = DownDashState.Cooldown;
					}
				}
				break;
			case DownDashState.Cooldown:
				dashTimer -= Time.deltaTime;
				//isDashing = false;
				if(dashTimer <= 0)
				{
					//isDashing = false;
					canDestroy = false;
					tempWaitTime = waitTime;
					dashTimer = 0;
					downDashState = DownDashState.Ready;
					DisableTrailRenderer();
				}
				break;
			}
//			if (!Input.GetKey(KeyCode.LeftShift))
//			{
//				player.moveSpeed = 15f;
//			}
//			else
//			{
//				player.moveSpeed = 22f;
//			}
		}
																////////////////////////
																////PLAYER IS YELLOW////
																////////////////////////
		if (isYellow)
		{
			notJumping = true;
			
			//WALLjumping WITH YELLOW END		
			if (Input.GetKeyDown(KeyCode.Space))  					//Jumping
			{
				if (player.wallSliding)
				{
					if (player.wallDirX == player.input.x)						//If input is towards the wall
					{
						notJumping = false;
						player.velocity.x = -player.wallDirX * wallJumpClimb.x;
						player.velocity.y = wallJumpClimb.y;
						if (controller.collisions.above  || controller.collisions.below)		//If raycasts hit above or below, velocity on y axis stops
						{
							player.velocity.y = 0;
						}
					}
					if (player.input.x == 0)								
					{
						notJumping = false;
						player.velocity.x = -player.wallDirX * wallLeap.x;
						player.velocity.y = wallLeap.y;
					}
					else if (player.wallDirX != player.input.x)					//If input is away from wall
					{
						notJumping = false;
						player.velocity.x = -player.wallDirX * wallLeap.x;
						player.velocity.y = wallLeap.y;
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))			//For increasing slidespeed on wall
			{
				if (player.wallSliding)				
				{
					player.wallSlideSpeedMax = 8f;
				}
			}
			if (Input.GetKeyUp(KeyCode.DownArrow))				//For decreasing slidespeed on wall
			{
				player.wallSlideSpeedMax = 0f;
			}	
		}
																////////////////////////
																////PLAYER IS GREEN/////
																////////////////////////
		if (isGreen)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				if (!controller.collisions.below && !player.wallSliding)
				{	
					inAir = true;
					player.velocity.y = Mathf.Lerp(player.velocity.y, floatingVelocity, 1f);
				}
			}

			else if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				inAir = false;
			}
		}
																////////////////////////
																////PLAYER IS ORANGE////
																////////////////////////
		if (isOrange)
		{
			//Is handled further down in: public void OnTriggerEnter2D.
		}
																////////////////////////
																////PLAYER IS PURPLE////
																////////////////////////
		if (isPurple)
		{
			if(!gravityReversed)
			{
				FlipPurple ();
				gravityReversed = true;
			}
			player.maxJumpHeight = -8f;		//Negative for reverse gravity
			player.timeToJumpApex = -.8f;		//Negative for reverse gravity	
			player.minJumpHeight = -0.5f; 		//Need to fix for variable jump
		}


		//BLUE TAG
		if(gameObject.tag == "blue")
		{
			hasBeenReset = false;
			renderer.material.color = blue;
			isRed = isGreen = isPurple = isOrange = isYellow = false;
			isBlue = true;
		}
		//RED TAG//
		else if(gameObject.tag == "red")
		{
			hasBeenReset = false;

			//Primary
			renderer.material.color = red;
			isBlue = isGreen = isPurple = isOrange = isYellow = false;
			isRed = true;
		}

		//YELLOW TAG//
		else if(gameObject.tag == "yellow")
		{
			hasBeenReset = false;
			//Primary
			renderer.material.color = yellow;
			isBlue = isGreen = isPurple = isOrange = isRed = false;
			isYellow = true;
		}

		//PURPLE TAG//
		else if(gameObject.tag == "purple")
		{
			hasBeenReset = false;
			renderer.material.color = purple;
			isYellow = isGreen = isOrange = false;
			isRed = isBlue = isPurple = true;
		}

		//ORANGE TAG//
		else if(gameObject.tag == "orange")
		{
			hasBeenReset = false;
			//Primary
			//Resize (orangeSize);
			renderer.material.color = orange;
			isBlue = isGreen = isPurple = false;
			isRed = isYellow = isOrange = true;
		}

		//GREEN TAG//

		else if(gameObject.tag == "green")
		{
			hasBeenReset = false;

			//Primary
			renderer.material.color = green;
			isRed = isPurple = isOrange = false;
			isYellow = isBlue = isGreen = true;

		}

		//WHITE TAG//
		else if(gameObject.tag == "white")
		{
			if(!hasBeenReset)
			{
				Reset();
				hasBeenReset = true;
			}
		}
	}

	private void FlipPurple()
	{
		// Switch the way the player is labelled as facing.
		normalGravity = !normalGravity;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}

	private void Resize(Vector3 newScale)
	{
			gameObject.transform.localScale = newScale;
	}

	public void startRotation()
	{
		start = graphicsTransform.rotation.eulerAngles.z;
		ifRotating = true;
		notRotating = false;
		t = 0;
		time = 0;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "orangeDestroy" && isOrange && canDestroy)
		{
			Destroy(other.gameObject);
		}
	}

	public void EnableTrailRenderer()
	{
		trailRenderer.enabled = true;
	}
	public void DisableTrailRenderer()
	{
		//trailRenderer.Clear(); //Should be here, but only works for Unity 5.3 and above.
		trailRenderer.enabled = false;
	}

	public void Reset()
	{
		renderer.material.color = Color.white;
		player.maxJumpHeight = 6f;
		player.minJumpHeight = 0.5f;
		player.timeToJumpApex = 0.4875f;
		player.moveSpeed = 11.5f;

		player.accelerationTimeGrounded = .09f; 
		player.accelerationTimeAirborn = 0.125f; //Was 0.25f
		gravityReversed = false;
		//isdashing = false;
		isDashing = false;

		if(controller.collisions.faceDir == 1)
		{
			gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
		}

		else if(controller.collisions.faceDir == -1)
		{
			gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
		}

	}
}