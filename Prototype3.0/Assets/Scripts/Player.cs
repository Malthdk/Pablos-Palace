using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
	
	public float maxJumpHeight = 3.5f;				//Max JumpHeight
	public float maxJumoHeightPainting;
	public float minJumpHeight = 1f;				//Minimum JumpHeight
	[HideInInspector]
	public float purpMinJumpVelocity = -9.3f;		//Minimum jump velocity when purple - have to be defined here cause of mathematical equation
	public float timeToJumpApex = .65f;				//Time to reach highest point (4875 was before)
	public float accelerationTimeAirborn = .2f;		//Acceleration while airborne
	public float accelerationTimeGrounded = .5f;	//Acceleration while grounded
	[HideInInspector]
	public float moveSpeed = 9;	
	public float airSpeed = 6.4f, groundSpeed = 9.4f, paintingSpeed;
	[HideInInspector]
	public Vector2 input;
	[HideInInspector]
	public int wallDirX;

	//FOR WALLSLIDING
	[HideInInspector]
	public float wallSlideSpeedMax = 0.01f;		//Maximum wall spide speed
	private float wallStickTime = 1f;		//Time before releasing from wall when input is away from wall
	float timeToWallUnstick;
	[HideInInspector]
	public bool wallSliding;
	[HideInInspector]
	public float wjXSmoothing, wjYSmoothing;
	private float wjAcceleration = 0.05f;

	public float gravity;					//gramaxJumpVelocity to player
	[HideInInspector]
	public float maxJumpVelocity, minJumpVelocity;			//Min jump velocity
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
	public float doubleJumpReduction;
	private float tripleJumpVelocity;
	public float tripleJumpReduction;
	public float gravityModifierFall;
	public float gravityModifierJump;
	public float gravityModifierPaintingJump;
	public float gravityModifierPaintingFall;

	// Has player landed?
	private bool landed = true;

	// FOR SOUND
	public AudioClip jumpSoundTakeOff;
	public AudioClip jumpSoundLanding;
	public AudioClip paintingSound;
	public AudioClip paintingSplashSound;

	[HideInInspector]
	public AudioSource paintingSoundSource;
	[HideInInspector]
	public AudioSource jumpSoundSource;

	private float volLowRange = .6f;
	private float volHighRange = .8f;
	private float volLanding;

	//ParticleSystems for dJump and tJump
	private ParticleSystem doubleJumpParticle;
	private ParticleSystem tripleJumpParticle;

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
		doubleJumpParticle = gameObject.transform.GetChild(6).GetChild(0).GetComponent<ParticleSystem>();
		tripleJumpParticle = gameObject.transform.GetChild(7).GetChild(0).GetComponent<ParticleSystem>();
		paintingSoundSource = gameObject.transform.GetChild(8).GetComponent<AudioSource>();
		jumpSoundSource = gameObject.transform.GetChild(9).GetComponent<AudioSource>();
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
			float vol = Random.Range(volLowRange, volHighRange);
			if (controller.collisions.below && !pullPush.isPulling && !Swimming.instance.isSwimming)
			{
				jumpSoundSource.PlayOneShot(jumpSoundTakeOff, vol);
				velocity.y = maxJumpVelocity;
				animator.SetBool("Ground", false);
			}

			if(!controller.collisions.below)
			{
				/*This is where double jump is handled*/
				if (!hasDoubleJumped)
				{	
					jumpSoundSource.PlayOneShot(jumpSoundTakeOff, (.1f + vol));
					velocity.y = 0;
					Debug.Log ("double jumped");
					doubleJumped = true; //This causes an animation bugg where we jump from a wall and then doublejump is is set to true so we canno transition into jump animation.
					hasDoubleJumped = true;
					//abilities.secondJump = true;
					velocity.y = doubleJumpVelocity;
					doubleJumpParticle.Play();
					
				}
				else if (!hasTripleJumped && hasDoubleJumped)
				{
					jumpSoundSource.PlayOneShot(jumpSoundTakeOff, (.2f + vol));
					velocity.y = 0;
					Debug.Log ("tripple jumped");
					tripleJumped = true; //animation
					hasTripleJumped = true; 
					velocity.y = tripleJumpVelocity;
					tripleJumpParticle.Play();
				}
			}
		}
			
		if (!controller.collisions.below && !pullPush.isPulling)
		{
			landed = false;
			moveSpeed = airSpeed;
			volLanding = (velocity.y / -30);
		}
		else if (controller.collisions.below && landed == false)
		{
			jumpSoundSource.PlayOneShot(jumpSoundLanding, volLanding);
			hasTripleJumped = false;
			hasDoubleJumped = false;
			tripleJumped = false; //animation
			doubleJumped = false; //animation
			moveSpeed = groundSpeed;
			landed = true;
		}
		if (controller.painting)
		{
			if (!paintingSoundSource.isPlaying) {
				StartCoroutine("PaintAudio");
				paintingSoundSource.PlayOneShot(paintingSplashSound, 0.8f);
			}
			moveSpeed = paintingSpeed;
		} else {
			paintingSoundSource.Pause();
		}
		if (Mathf.Sign(velocity.y) == -1)
		{
			if (controller.painting)
			{
				timeToJumpApex = gravityModifierPaintingFall;
			}
			else
			{
				timeToJumpApex = gravityModifierFall; //55 //0.75
			}
		}
		else 
		{
			if (controller.painting)
			{
				//maxJumpHeight = maxJumoHeightPainting;
				timeToJumpApex = gravityModifierPaintingJump;
				doubleJumpVelocity = maxJumpVelocity/doubleJumpReduction;
				tripleJumpVelocity = maxJumpVelocity/tripleJumpReduction;
			}
			else
			{
				timeToJumpApex = gravityModifierJump;//0.92f;	//72
				doubleJumpVelocity = maxJumpVelocity/doubleJumpReduction;
				tripleJumpVelocity = maxJumpVelocity/tripleJumpReduction;
			}
		}

		if (Input.GetButtonUp("Jump"))					//For variable jump
		{
			if (velocity.y > minJumpVelocity && (!hasDoubleJumped && !hasTripleJumped))
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

	IEnumerator PaintAudio() {
		paintingSoundSource.pitch = Mathf.Lerp(0.9f, 1.1f, (paintingSound.length*Time.deltaTime));
		//source.pitch = Random.Range (0.9f, 1.1f);
		float vol = Random.Range (0.9f, 1.0f);
		paintingSoundSource.PlayOneShot(paintingSound, vol);
		yield return new WaitForSeconds(paintingSound.length-0.15f);
		if (!controller.painting) {
			StopCoroutine("PaintAudio");
		} else {
			StartCoroutine("PaintAudio");
		}
	}

	//IEnumerator SpecialJump(float velocity)
	//{
	//	yield return new WaitForEndOfFrame();
	//}
}