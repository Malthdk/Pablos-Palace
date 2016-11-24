﻿using UnityEngine;
using System.Collections;

public class Abilities : MonoBehaviour {

	private Color red = new Color(0.82F, 0F, 0.2F);			//Red farve
	private Color yellow = new Color(1F, 0.8F, 0.2F);		//Yellow farve
	private Color blue = new Color(0.2F, 0.18F, 1F);		//Blue farve
	private Color green = new Color(0.2F, 0.8F, 0.15F);		//Green farve
	private Color purple = new Color(0.55F, 0.15F, 0.7F);	//Purple farve
	private Color orange = new Color(1F, 0.6F, 0.2F);		//Orange farve

	//COLOR BOOLS
	[HideInInspector]
	public bool isBlue = false, isRed = false, isYellow = false, isGreen = false, isOrange = false, isPurple = false;

	//DOUBLE JUMP
	//[HideInInspector]
	public bool hasDoubleJumped, ifRotating = false, secondJump, notRotating;
	private float duration = 0.5f, t = 0, time = 0, start = 0f;

	//DASH
	public float maxDash = 0.15f, minDash = 0.06f, maxDashVelocity = 30f;
	public bool singleDash; //For testing dash
	public bool longDash;	//For testing dash
	private DashState dashState;
	private TrailRenderer trailRenderer;
	private float dashTimer, tempMinDash;
	[HideInInspector]
	public bool isDashing, hasDashed;
	private bool isDashKeyDown;

	//DOWN DASH
	public float waitTime = 0.5f, maxDownDashVelocity = 40f;
	[HideInInspector]
	public DownDashState downDashState;
	private float tempWaitTime;
	[HideInInspector]
	public bool isdowndashing = false;

	//DASH ORANGE
	[HideInInspector]
	public bool canDestroy;

	//FOR GREEN
	[HideInInspector]
	public bool soaring = false;
	public float floatingVelocity;
	
	//FOR WALLJUMP
	public Vector2 wallJumpClimb = new Vector2(12f, 25.6f), wallLeap = new Vector2(0f, 0f); 			//1. iteration of wallJumpClimb (input towards wall + space) 7.5f, 16f. 2nd iteration: 12f, 25.6f 3rd. 10.5f, 22.4
																												//4. iteration (12f, 25.6f) (26.4f, 24.9f)
	[HideInInspector]																										//3. iteration of wallLeap (input away from wall + space) 18f 17f. 2nd 26.4f, 24.9f 3rd. 23.1f,21.8f
	public bool notJumping;

	private SpriteRenderer renderer;
	private Player player;						//Calling player class
	private Controller2D controller;			//Calling controller class
	private Transform graphicsTransform;

	private bool hasBeenReset = true, gravityReversed = false, normalGravity = true;		//normalGravity is for flipping character when purple	

	[HideInInspector]
	public static Abilities _instance;

	public static Abilities instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<Abilities>();
			}
			return _instance;
		}
	}

	void Start () 
	{
		hasDoubleJumped = false;
		hasDashed = false;
		controller = GetComponent<Controller2D>();
		player = GetComponent<Player>();
		renderer = gameObject.transform.FindChild("Graphics").GetComponent<SpriteRenderer>();
		graphicsTransform = gameObject.transform.FindChild("Graphics").GetComponent<Transform>();

		//FOR DASH
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
		}
																////////////////////////
																////PLAYER IS RED///////
																////////////////////////
		if (isRed)
		{

			if (controller.collisions.below && hasDashed || player.wallSliding || controller.collisions.above && isPurple && hasDashed)
			{
				hasDashed = false;
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
				if(isDashKeyDown && !player.wallSliding && !hasDashed)
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
					hasDashed = true;
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
				isdowndashing = true;
				isDashing = false;
				canDestroy = true;
				if(dashTimer >= maxDash)
				{
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
				if (dashTimer <= 0)
				{
					canDestroy = false;
					tempWaitTime = waitTime;
					dashTimer = 0;
					downDashState = DownDashState.Ready;
					isdowndashing = false;
					DisableTrailRenderer();
				}
				break;
			}
		}
																////////////////////////
																////PLAYER IS YELLOW////
																////////////////////////
		if (isYellow)
		{
			notJumping = true;
			
			//WALLSLIDING WITH YELLOW		
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
					soaring = true;
					player.velocity.y = Mathf.Lerp(player.velocity.y, floatingVelocity, 1f);
				}
			}

			else if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				soaring = false;
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
			player.maxJumpHeight = -5.6f;		//Negative for reverse gravity
			player.timeToJumpApex = -0.4875f;		//Negative for reverse gravity	
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
			isBlue = isRed = isYellow = isGreen = isOrange = isPurple = false;
		}
	}

	private void FlipPurple()
	{
		// Switch the way the player is labelled as facing.
		normalGravity = !normalGravity;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = graphicsTransform.localScale;
		theScale.y *= -1;
		graphicsTransform.localScale = theScale;
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
			other.gameObject.SetActive(false);
		}
	}

	public void EnableTrailRenderer()
	{
		trailRenderer.enabled = true;
	}
	public void DisableTrailRenderer()
	{
		trailRenderer.Clear();
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
		isDashing = false;
		soaring = false;
//		if(controller.collisions.faceDir == 1)
//		{
//			gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
//		}
//
//		else if(controller.collisions.faceDir == -1)
//		{
//			gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
//		}

	}
}