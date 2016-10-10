using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformController : RaycastController {

	// Should it function as an elevator
	public bool elevator = false;
	public bool stoppingPlatform = false;
	public bool platformActivator = false;
	public bool smashPlatform;
	public LayerMask passengerMask;


	public Vector3[] localWaypoints;
	public Vector3[] globalWaypoints;

	public float speed;
	public bool cyclic;
	public float waitTime;

	[Range(0, 2)]				//Actual value is 1-3
	public float easeAmount;

	[HideInInspector]
	public Vector3 pos;

	private int fromWaypointIndex;
	private float percentBetweenWaypoints; 
	private float nextMoveTime; 

	List<PassengerMovement> passengerMovement;
	Dictionary<Transform,Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();   //Passangers dictionary to reduce get component calls

	public override void Start () 
	{
		base.Start();

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i=0; i < localWaypoints.Length; i++)
		{
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}
	

	void Update () 
	{
		pos = gameObject.transform.position;

		//Debug.Log(platformActivator);

		if ( platformActivator == false && elevator == false ) 
		{
			UpdateRaycastOrigins ();
			Vector3 velocity = CalculatePlatformMovement();
			CalculatePassengerMovement(velocity);
			MovePassengers (true);
			transform.Translate(velocity);
			MovePassengers (false);
		} 
	}

	float Ease(float x) 
	{
		float a = easeAmount + 1;
		return Mathf.Pow (x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1-x,a));
	}

	Vector3 CalculatePlatformMovement()
	{
		if (Time.time < nextMoveTime)
		{
			return Vector3.zero;
		}
		fromWaypointIndex %= globalWaypoints.Length; 
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * speed/distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
		float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1)
		{
			percentBetweenWaypoints = 0;
			fromWaypointIndex ++;
			if (!cyclic)
			{
				if (fromWaypointIndex >= globalWaypoints.Length-1)
				{
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
				}
			}
			nextMoveTime = Time.time + waitTime;
		}
		return newPos - transform.position;
	}

	void MovePassengers ( bool beforeMovePlatform)
	{
		foreach (PassengerMovement passenger in passengerMovement)
		{
			if (!passengerDictionary.ContainsKey(passenger.transform))
			{
				passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
			}

			if (passenger.moveBeforePlatform == beforeMovePlatform)
			{
				passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform, passenger.slidingOnPlatformLeft, passenger.slidingOnPlatformRight);
			}
		}
	}


	void CalculatePassengerMovement(Vector3 velocity)
	{
		HashSet<Transform> movedPassengers = new HashSet<Transform>();
		passengerMovement = new List<PassengerMovement>();

		float directionX = Mathf.Sign(velocity.x);
		float directionY = Mathf.Sign(velocity.y);


		//VERTICALLY MOVING PLATFORM
		if (velocity.y != 0)										//If velocty on y axis is not equal to zero then we have a vertically moving platform
		{
			float rayLength = Mathf.Abs (velocity.y) + skinWidth;	//Ray lenght is equal to the absolute velocity of y + skinwidth
			
			for (int i = 0; i < verticalRayCount; i ++)				
			{
				Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomRight:raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				if (hit && hit.distance != 0) 			//The && is for maiking player capable of jumping through "Through" platforms that are moving. Its makes it so that if we are inside of platform no collision is detected.
				{
					if (!movedPassengers.Contains(hit.transform))
					{
						movedPassengers.Add(hit.transform);
						float pushX = (directionY ==1)?velocity.x:0;
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
				
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), directionY == 1, false, false, true));
					}
				}
			}
		}

		//HORIZONTALLY MOVING PLATFORM
		if (velocity.x != 0)						//If velocty on x axis is not equal to zero then we have a horizontally moving platform
		{
			float rayLength = Mathf.Abs (velocity.x) + skinWidth;

			for (int i = 0; i < horizontalRayCount; i ++)
			{
				Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

				if (hit && hit.distance != 0 && hit.collider.gameObject.tag != "yellow")
				{
					if (!movedPassengers.Contains(hit.transform))
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
						float pushY = -skinWidth;
						
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), false, false, false, true));
					}
				}
			}
			for (int i = 0; i < horizontalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.up * (horizontalRaySpacing * i);	//Rayorigin allways on bottomleft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, passengerMask); 		//Allways casting ray.
				
				Debug.DrawRay(rayOrigin, Vector2.left * rayLength, Color.red);
				
				if (hit && hit.distance != 0 && hit.collider.gameObject.tag == "yellow")
				{
					if (!movedPassengers.Contains(hit.transform))  //Makes it so player will only be moved once per frame. 
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), false, true, false, false));
					}
				}
			}
			for (int i = 0; i < horizontalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.bottomRight + Vector2.up * (horizontalRaySpacing * i);	//Rayorigin allways on bottomleft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, passengerMask); 		//Allways casting ray.
				
				Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.red);
				
				if (hit && hit.distance != 0 && hit.collider.gameObject.tag == "yellow")
				{
					if (!movedPassengers.Contains(hit.transform))  //Makes it so player will only be moved once per frame. 
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), false, false, true, false));
					}
				}
			}
		}
		//PASSANGER ON TOP OF A HORIZONTALLY OR DOWNWARD MOVING PLATFORM
		if (directionY == -1 || velocity.y == 0 && velocity.x != 0)        //If platform moving downward or horizontally
		{
			float rayLength = skinWidth * 2;			//Short rayLength
			
			for (int i = 0; i < verticalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);		//Rayorigin allways on topLeft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask); 		//Allways casting ray upwards.

				Debug.DrawRay(rayOrigin, Vector2.up * rayLength * 4, Color.red);

				if (hit && hit.distance != 0)
				{
					if (!movedPassengers.Contains(hit.transform))
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), true, false, false, false));
					}
				}
			}
		}
		//PASSANGER BELOW OF A HORIZONTALLY OR DOWNWARD/UPWARDS MOVING PLATFORM AS PURPLE
		if (directionY == -1 || directionY == 1 || velocity.y == 0 && velocity.x != 0) 
		{
			float rayLength = skinWidth * 4;			//Short rayLength
			
			for (int i = 0; i < verticalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i);		//Rayorigin allways on bottomleft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, passengerMask); 		//Allways casting ray.

				Debug.DrawRay(rayOrigin, -Vector2.up * rayLength, Color.red);

				if (hit && hit.distance != 0 && hit.collider.gameObject.tag == "purple")
				{
					if (!movedPassengers.Contains(hit.transform))  //Makes it so player will only be moved once per frame. 
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), true, false, false, false));
					}
				}
			}
		}

		if (directionY == -1 && smashPlatform == true)
		{
			float rayLength = skinWidth * 30;			//Short rayLength

			for (int i = 0; i < verticalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i);		//Rayorigin allways on bottomleft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, passengerMask); 		//Allways casting ray.

				Debug.DrawRay(rayOrigin, -Vector2.up * rayLength, Color.green);

				if (hit && hit.distance !=0)
				{
					Debug.Log("kill player");
					PlayerManager.pManager.KillPlayer();
				}
			}
		}

		//PASSENGER STICKING TO A DOWN/UP MOVING PLATFORM
		if (directionY == -1 && velocity.x == 0|| directionY == 1 && velocity.x == 0 ) 
		{
			float rayLength = skinWidth * 4;			//Short rayLength
			
			for (int i = 0; i < horizontalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.up * (horizontalRaySpacing * i);	//Rayorigin allways on bottomleft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, passengerMask); 		//Allways casting ray.
				
				Debug.DrawRay(rayOrigin, Vector2.left * rayLength, Color.red);
				
				if (hit && hit.distance != 0 && hit.collider.gameObject.tag == "yellow")
				{
					if (!movedPassengers.Contains(hit.transform))  //Makes it so player will only be moved once per frame. 
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), false, true, false, false));
					}
				}
			}
			for (int i = 0; i < horizontalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.bottomRight + Vector2.up * (horizontalRaySpacing * i);	//Rayorigin allways on bottomleft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, passengerMask); 		//Allways casting ray.
				
				Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.red);
				
				if (hit && hit.distance != 0 && hit.collider.gameObject.tag == "yellow")
				{
					if (!movedPassengers.Contains(hit.transform))  //Makes it so player will only be moved once per frame. 
					{
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add (new PassengerMovement(hit.transform, new Vector3(pushX,pushY), false, false, true, false));
					}
				}
			}
		}
	}
	struct PassengerMovement
	{
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool slidingOnPlatformLeft;
		public bool slidingOnPlatformRight;
		public bool moveBeforePlatform;

		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _slidingOnPlatformLeft, bool _slidingOnPlatformRight, bool _moveBeforePlatform)
		{
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
			slidingOnPlatformLeft = _slidingOnPlatformLeft;
			slidingOnPlatformRight = _slidingOnPlatformRight;

		}
	}

	void OnDrawGizmos()
	{
		if (localWaypoints != null)
		{
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i = 0; i < localWaypoints.Length; i ++)
			{
				Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i]:localWaypoints[i] + transform.position;
				Gizmos.DrawLine (globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine (globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			elevator = false;
			Debug.Log("player on platform");
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Player" && stoppingPlatform == true) {
			elevator = true;
		}
		else
		{
			elevator = false;
		}
	}
}
