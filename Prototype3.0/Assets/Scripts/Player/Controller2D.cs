﻿using UnityEngine;
using System.Collections;

public class Controller2D : RaycastController {

	float maxClimbAngle = 80;					//Highest angle for climbing slopes
	float maxDescendAngle = 75;					//Highest angle for controlling descension on sloeps

	public Collisioninfo collisions;			//Calling collisions info from struct
	public LevelManager levelmanager;
	public Splatter splatter;

	[HideInInspector]
	public Vector2 playerInput;
	public Checkpoint checkpoint;
	public UIManager uiManager;

	[HideInInspector]
	public bool facingRight = true;		//For switching face direction

	//Graphics and splat
	private Transform graphicsTransform;
	public float splatTime = 0.0005f;
	public bool painting;
	private PullPush pullPush;
	[HideInInspector]
	public static Controller2D _instance;
	private float newZPos;
	private CreateSplat createSplat;

	//PaintParticles
	private ParticleSystem paintParticles;
	private GameObject paintParticleGO;
	private bool isEmitting;

	public static Controller2D instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<Controller2D>();
			}
			return _instance;
		}
	}

	public override void Start()
	{
		base.Start();
		collisions.faceDir = 1;					//Face direction set to 1
		levelmanager = FindObjectOfType<LevelManager>();
		checkpoint = FindObjectOfType<Checkpoint>();
		pullPush = GetComponent<PullPush>();
		createSplat = GetComponent<CreateSplat>();
		//dashParticle = transform.GetChild(3).gameObject;
		paintParticleGO = transform.GetChild(5).gameObject;
		paintParticles = paintParticleGO.transform.GetChild(0).GetComponent<ParticleSystem>();
		graphicsTransform = gameObject.transform.FindChild("Graphics").transform;
	}

	public override void Update()
	{
		base.Update();

		if (Input.GetButton("Special") && !ColorStates.instance.isWhite)
		{
			if (createSplat.onMiddleGround)
			{
				StartSplat();
				EmitParticle(paintParticles);
			}
			else
			{
				StopSplat();
				StopEmitParticle(paintParticles);
			}
		}
		else if (Input.GetButtonUp("Special"))
		{
			painting = false;
			StopSplat();
			StopEmitParticle(paintParticles);
		}
		if (!createSplat.onMiddleGround)
		{
			painting = false;
			StopEmitParticle(paintParticles);
		}

	}
		

	public void Move(Vector3 velocity, bool standingOnPlatform, bool slidingOnplatformLeft, bool slidingOnPlatformRight)		//Small overload function for the platformcontroller to use without any player input. Ergo Vector2.zero. 
	{
		Move (velocity, Vector2.zero, standingOnPlatform, slidingOnplatformLeft, slidingOnPlatformRight);
	}

	public void Move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false, bool slidingOnPlatformLeft = false, bool slidingOnPlatformRight = false)		//Move function
	{
		UpdateRaycastOrigins ();
		collisions.Reset();							//Resetting all collisions
		collisions.velocityOld = velocity;			
		playerInput = input;						//The player input

		if (velocity.x != 0)						
		{
			collisions.faceDir = (int)Mathf.Sign (velocity.x);

			if (collisions.faceDir == -1 && facingRight)
			{
				if (!Player.instance.wallSliding)
				{
					//Debug.Log("Flip1");
					Flip ();
				}
			}
			if (collisions.faceDir == 1 && !facingRight)
			{
				if (!Player.instance.wallSliding)
				{
					//Debug.Log("Flip2");
					Flip ();
				}
			}
		}

		if (velocity.y <0)							
		{
			DescendSlope(ref velocity);
		}

		HorizontalCollisions( ref velocity);

		if (velocity.y != 0)
		{
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);

		if (standingOnPlatform)						//Tracing collisions below player when on platform
		{
			collisions.below = true;
		}
		if (slidingOnPlatformLeft)
		{
			collisions.right = true;
			collisions.below = false;
		}
		if (slidingOnPlatformRight)
		{
			collisions.left = true;
			collisions.below = false;
		}
	}

	//HORIZONTAL COLLISIONS
	void HorizontalCollisions(ref Vector3 velocity) 				
	{
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		if (Mathf.Abs(velocity.x) < skinWidth)  			 //So we can jump even without horizontal input
		{
			rayLength = 2*skinWidth;						//Setting raylenght to be skinwidth + 1 skinwidth outsite of player for wall detection without horizontal input
		}

		for (int i = 0; i < horizontalRayCount; i ++)
		{
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;				//Raycast origin either bottom left or bottom right
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);														
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);		//Raycast hit - from origin in directionX with rayLength and only on collision layer

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red); 								//Drawing red rays

			if (hit)
			{
				if (hit.distance == 0)
				{
					continue;
				}

				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= maxClimbAngle)
				{
					if (collisions.descendingSlope)							// Makes it so we can go from descending to ascending slope. 
					{
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld; 
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld)
					{
						distanceToSlopeStart = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}
				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisions.climbingSlope)
					{
						velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}
					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
				//Horizontal Collision with Paint
//				if (hit.collider.gameObject.CompareTag("orangeDestroy") && gameObject.tag == "orange" )
//				{
//					Destroy(hit.collider.gameObject);
//				}
				if (hit.collider.gameObject.CompareTag("blackBox"))
				{
					//gameObject.tag = checkpoint.tempTag;
					PlayerManager.instance.KillPlayer();
				}
				if (hit.collider.gameObject.CompareTag("doctorDark"))
				{
				}
				if (hit.collider.gameObject.CompareTag("coin"))
				{
					LevelManager.instance.coinCount++;
					Destroy(hit.collider.gameObject);
				}
			}
		}
	}

	//VERTICAL COLLISIONS
	public void VerticalCollisions(ref Vector3 velocity) 
	{
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++)
		{
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit)
			{
				// Instantiates splatter
				//if (this.gameObject.tag != "white") {
				//	StartCoroutine(SplatterControl());
				//}
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
				{
					PlayerManager.instance.KillPlayer();
				}
				if (hit.collider.tag == "Through")					//To go through "Through" platforms
				{
					if (directionY ==1 || hit.distance == 0)		//Makes sure we have to go all the way through platform to land on it
					{
						continue;									//continues direction
					}
					if (collisions.fallingThroughPlatform)
					{
						continue;
					}
					if (playerInput.y == -1)						//if player input is down
					{
						collisions.fallingThroughPlatform = true;  //setting falling through platforms to true
						Invoke("ResetFallingThroughPlatform", .2f); //After half a second this method will be called
						continue;
					}
				}
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if ( collisions.climbingSlope)
				{
					velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;

				if (hit.collider.gameObject.CompareTag("blackBox"))
				{
					//gameObject.tag = checkpoint.tempTag;
					PlayerManager.instance.KillPlayer();
				}
				if (hit.collider.gameObject.CompareTag("coin"))
				{
					UIManager.uiManager.score++;
					Destroy(hit.collider.gameObject);
				}
			}
		}
		if (collisions.climbingSlope)
		{
			float directionX = Mathf.Sign (velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask); 
			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX; 
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	//CLIMBING SLOPES
	void ClimbSlope (ref Vector3 velocity, float slopeAngle)
	{
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) 
		{
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}

	}

	//DESCENDING SLOPES
	void DescendSlope(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomRight : raycastOrigins.bottomLeft;  //If moving left cast ray from bottomrightcorner otherwise bottomleft.
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

		if (hit)
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) 
			{
				if (Mathf.Sign(hit.normal.x) == directionX)
				{
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) //If we are close enough for the slope to come into effect.
					{
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true; 
					}
				}
			}
		}
	}

	void ResetFallingThroughPlatform()
	{
		collisions.fallingThroughPlatform = false;
	}

	// FLIPS PLAYER
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
				
		// Multiply the player's x local scale by -1.
		Vector3 theScale = graphicsTransform.localScale;
		theScale.x *= -1;
		graphicsTransform.localScale = theScale;

		//Flip the dashParticleSystem aswell
		//Vector3 particleScale = dashParticle.transform.localScale;
		//particleScale.x *= -1;
		//dashParticle.transform.localScale = particleScale;

		//Flip the paint particle aswell
		Vector3 particleScale = paintParticleGO.transform.localPosition;
		particleScale.x *= -1;
		paintParticleGO.transform.localPosition = particleScale;
	}

	// METHOD FOR SPLATTING
	private void StartSplat() {
		painting = true;
		createSplat.isSplatting = true;
	}
	private void StopSplat()
	{
		createSplat.isSplatting = false;
	}

	//STRUCT WITH COLLISION INFOS
	public struct Collisioninfo 
	{
		public bool above, below;						//Collision on above or below?
		public bool left, right;						//Collision on right or left?

		public bool descendingSlope;					//Is player descending slope
		public bool climbingSlope;						//Is player climbing slope
		public float slopeAngle, slopeAngleOld;			//Slope angles and slope angle on previou slope
		public Vector3 velocityOld;
		public int faceDir;								//Player face direction
		public bool fallingThroughPlatform;

		public void Reset() 							//Function for resetting collisiondetection
		{
			above = below = false;
			left = right = false; 
			climbingSlope = false;
			descendingSlope = false; 

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "chaseBoss" || other.tag == "killTag")
		{
			PlayerManager.instance.KillPlayer();
		}
	}

	public void EmitParticle(ParticleSystem particle)
	{
		if (!particle.isPlaying)
		{
			particle.Play();
		}
	}
	public void StopEmitParticle(ParticleSystem particle)
	{
		particle.Stop();
	}

}