using UnityEngine;
using System.Collections;

public class Swimming : MonoBehaviour {

	//Public
	public float swimSpeed;
	public float sprintSwim;
	public bool isSwimming;
	public float accelerationTimeWater;

	//Private and hidden
	[HideInInspector]
	public bool notSurface;
	private float refSpeed;
	private Player player;
	[HideInInspector]
	public float velocityYSmoothing;

	[HideInInspector]
	public static Swimming _instance;
	public static Swimming instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<Swimming>();
			}
			return _instance;
		}
	}

	void Start () 
	{
		refSpeed = swimSpeed;
		player = GetComponent<Player>();
	}
	

	void Update () 
	{
		if (isSwimming)
		{
			player.maxJumpHeight = 5f;			//-5.6f;
			player.timeToJumpApex = 2.5f;		//-0.4875f;	
			player.minJumpHeight = -0.5f; 		//-0.5f; UNNECESSARY?

			float targetVelocityX = player.input.x * swimSpeed;
			float targetVelocityY = player.input.y * swimSpeed;

			player.velocity.x = Mathf.SmoothDamp(player.velocity.x, targetVelocityX, ref player.velocityXSmoothing, accelerationTimeWater);		//Calculating velocity x 
			player.velocity.y = Mathf.SmoothDamp(player.velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeWater);		//Calculating velocity y

			if (Input.GetButton("Jump"))
			{
				swimSpeed = sprintSwim;
			}
			if (Input.GetButtonUp("Jump"))
			{
				swimSpeed = refSpeed;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "waterPool")
		{
			Debug.Log("Swimming");
			isSwimming = true;
			//Abilities.instance.Reset();  //NEEDS TO RESET TAG ASWELL 
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "waterPool")
		{
			Debug.Log("No Swimming");
			isSwimming = false;
			Abilities.instance.Reset();
		}
	}
		
}
