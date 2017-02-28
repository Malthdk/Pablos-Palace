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
	[HideInInspector]
	public bool isBlue = false, isRed = false, isYellow = false, isGreen = false, isOrange = false, isPurple = false;

	//DOUBLE JUMP
	//[HideInInspector]
	public bool hasDoubleJumped, ifRotating = false, secondJump, notRotating;
	private float duration = 0.5f, t = 0, time = 0, start = 0f;

	//DASH
	[HideInInspector]
	public float maxDash = 0.15f, minDash = 0.06f, maxDashVelocity = 30f;
	public bool singleDash; //For testing dash
	public bool longDash;	//For testing dash
	private DashState dashState;
//	private TrailRenderer trailRenderer;
	private ParticleSystem dashParticle;
	private float dashTimer, tempMinDash;
	[HideInInspector]
	public bool isDashing, hasDashed;
	private bool isDashKeyDown;

	//DOWN DASH
	private float maxDownDash = 0.15f;
	private float waitTime = 0.1f, maxDownDashVelocity = 30f;
	[HideInInspector]
	public DownDashState downDashState;
	private float tempWaitTime;
	private bool isDownDashKeyDown;
	[HideInInspector]
	public bool isDownDashing;
	private bool facingDirection;

	//DASH ORANGE
	[HideInInspector]
	public bool canDestroy;

	//FOR GREEN
	[HideInInspector]
	public bool soaring = false;
	public float floatingVelocity;
	
	//FOR WALLJUMP
	public Vector2 wallJumpClimb = new Vector2(12f, 25.6f), wallLeap = new Vector2(0f, 0f); 			//1. iteration of wallJumpClimb (input towards wall + space) 7.5f, 16f. 2nd iteration: 12f, 25.6f 3rd. 10.5f, 22.4
																												//4. iteration (12f, 25.6f) (26.4f, 24.9f) //5 12,21 og 17,21
	[HideInInspector]																										//3. iteration of wallLeap (input away from wall + space) 18f 17f. 2nd 26.4f, 24.9f 3rd. 23.1f,21.8f
	public bool notJumping;

	private SpriteRenderer renderer;
	private Player player;						//Calling player class
	private Controller2D controller;			//Calling controller class
	private Transform graphicsTransform;

	private bool hasBeenReset = true, normalGravity = true;		//normalGravity is for flipping character when purple	
	public  bool gravityReversed = false;
	[HideInInspector]
    public float special = 0;

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
//		trailRenderer = GetComponentInChildren<TrailRenderer>();
		dashParticle = transform.GetChild(3).GetComponent<ParticleSystem>();
		StopEmitParticle(dashParticle);

		tempWaitTime = waitTime;
		tempMinDash = minDash;
	}

	public enum DashState 
	{
		Ready,
		Waiting,
		Dashing,
		EndDash,
		Cooldown
	}
	public enum DownDashState 
	{
		Ready,
		Waiting,
		Dashing,
		EndDash,
		Cooldown
	}

	void Update () 
	{

        special = Input.GetAxis("Special");
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
			//dashParticle.main.startColor = new Color(0.85f, 0f ,0.22f);

			if (controller.collisions.below && hasDashed || player.wallSliding || controller.collisions.above && isPurple && hasDashed)
			{
				hasDashed = false;
			}
			if (isDashing)
			{
				if (controller.collisions.left || controller.collisions.right)
				{
					dashState = DashState.Cooldown;
				}
 			}

            if (Input.GetButtonDown("Special") || special == 1)
            {
				isDashKeyDown = true;
			}
			else {
				isDashKeyDown = false;
			}

            if (player.input.y == -1)
            {
				isDownDashKeyDown = true;
			}
			else {
				isDownDashKeyDown = false;
			}

			//For dash
			switch (dashState) 
			{
			case DashState.Ready:
				if(isDashKeyDown && !isDownDashKeyDown && !player.wallSliding && !hasDashed)
				{
					isDashing = true;
					canDestroy = true;
//					EnableTrailRenderer();
					EmitParticle(dashParticle);
					dashState = DashState.Waiting;
				}
				break;
			case DashState.Waiting:
				tempWaitTime -= Time.deltaTime;
				player.velocity.x = 0f;
				if (tempWaitTime <= 0)
				{
					Debug.Log ("Switching state");
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
							dashState = DashState.EndDash;
						}
					}
					//One dash when pressing dashbutton
					if (singleDash)
					{
						dashState = DashState.EndDash;
					}
				}
				break;
			case DashState.EndDash:
				dashTimer -= Time.deltaTime;
				if(dashTimer <= 0)
				{
					//Bad fix for having the player retain some velocity after a dash. This could also be 0 for instant stop.
					if (controller.collisions.faceDir == 1)
					{
						player.velocity.x = 10f;
					}
					else if (controller.collisions.faceDir == -1)
					{
						player.velocity.x = -10f;
					}
					dashState = DashState.Cooldown;
				}
				break;
			case DashState.Cooldown:
					isDashing = false;
					canDestroy = false;
					dashTimer = 0;
					minDash = tempMinDash;
					tempWaitTime = waitTime;
					dashState = DashState.Ready;
//					DisableTrailRenderer();
					StopEmitParticle(dashParticle);
				break;
			}

			//For downward burst
			switch (downDashState) 
			{
			case DownDashState.Ready:
				
				if(isDownDashKeyDown && isDashKeyDown && !controller.collisions.below && !player.wallSliding)
				{
					if (controller.collisions.faceDir == 1)
					{
						facingDirection = true;	
					}
					else
					{
						facingDirection = false;
					}
//					EnableTrailRenderer();
					isDownDashing = true;
					player.velocity.x = 0f;
					player.velocity.y = 0f;
					downDashState = DownDashState.Waiting;
				}
				break;
			case DownDashState.Waiting:
				tempWaitTime -= Time.deltaTime;
				if (tempWaitTime <= 0)
				{
					if (controller.collisions.faceDir == 1)
					{
						bool facingRight = true;
					}
					Debug.Log ("Switching state");
					downDashState = DownDashState.Dashing;
				}
				break;
			case DownDashState.Dashing:
				dashTimer += Time.deltaTime * 3;
				canDestroy = true;
				if(dashTimer >= maxDownDash)
				{
					dashTimer = maxDownDash;
					if(!isPurple)
					{
						player.velocity.y = -maxDownDashVelocity;
					}

					if (isPurple)
					{
						player.velocity.y = maxDashVelocity;
					}
					downDashState = DownDashState.EndDash;
					
				}
				break;
			case DownDashState.EndDash:
				dashTimer -= Time.deltaTime;
				if (dashTimer <= 0)
				{
					if(!isPurple)
					{
						player.velocity.y = -10f;
					}

					if (isPurple)
					{
						player.velocity.y = 10f;
					}
					downDashState = DownDashState.Cooldown;
				}
				break;
			case DownDashState.Cooldown:
				if (facingDirection)
				{
					controller.collisions.faceDir = 1;
				}
				else 
				{
					controller.collisions.faceDir = -1;
				}
					canDestroy = false;
					isDownDashing = false;
					tempWaitTime = waitTime;
					dashTimer = 0;
//					DisableTrailRenderer();
					downDashState = DownDashState.Ready;
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
			if (player.input.y == -1)			//For increasing slidespeed on wall
			{
				if (player.wallSliding)				
				{
					player.wallSlideSpeedMax = 8f;
				}
			}
			if (player.input.y != -1)				//For decreasing slidespeed on wall
			{
				player.wallSlideSpeedMax = 0f;
			}	
		}
																////////////////////////
																////PLAYER IS GREEN/////
																////////////////////////
		if (isGreen)
		{
			if (Input.GetButtonDown("Special")  || special == 1 )
			{
				if (!controller.collisions.below && !player.wallSliding)
				{	
					player.doubleJumped = false;
					soaring = true;
					player.velocity.y = Mathf.Lerp(player.velocity.y, floatingVelocity, 1f);
				}
			}

			else if (Input.GetButtonUp("Special")  || special == 0)
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
			//dashParticle.main.startColor = new Color(1f, 0.42f, 0.0f);
		}
																////////////////////////
																////PLAYER IS PURPLE////
																////////////////////////
		if (isPurple)
		{
			Debug.Log(gravityReversed);
			if(!gravityReversed)
			{
				FlipPurple ();
			}
			player.maxJumpHeight = -5.6f;		//Negative for reverse gravity
			player.timeToJumpApex = -0.58f;		//Negative for reverse gravity	 was -0.4875f
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
			}
			renderer.material.color = Color.white;
			isBlue = isRed = isYellow = isGreen = isOrange = isPurple = false;
		}
	}

	private void FlipPurple()
	{
		Debug.Log("Flipped");
		// Switch the way the player is labelled as facing.
		normalGravity = !normalGravity;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = graphicsTransform.localScale;
		theScale.y *= -1;
		graphicsTransform.localScale = theScale;
		gravityReversed = true;
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

//	public void EnableTrailRenderer()
//	{
//		trailRenderer.enabled = true;
//	}
//	public void DisableTrailRenderer()
//	{
//		trailRenderer.Clear();
//		trailRenderer.enabled = false;
//	}

	public void EmitParticle(ParticleSystem particle)
	{
		particle.Play();
	}
	public void StopEmitParticle(ParticleSystem particle)
	{
		particle.Stop();
	}
	public void Reset()
	{
		StartCoroutine("Resetting");
	}

	IEnumerator Resetting()
	{
		//Debug.Log("PlayerState has been Reset");
		gameObject.tag = "white";
//		DisableTrailRenderer();
		StopEmitParticle(dashParticle);
		if (isPurple)
		{
			gravityReversed = false;
		}
		yield return null;

		player.moveSpeed = 11.5f;
		player.accelerationTimeGrounded = .05f; 
		player.accelerationTimeAirborn = 0.125f;
		isDashing = false;
		soaring = false;
		hasBeenReset = true;
		gravityReversed = false;

		yield return null;

		player.maxJumpHeight = 5.6f;
		player.minJumpHeight = 0.5f;
		player.timeToJumpApex = 0.58f; //0.4875f
	}
}