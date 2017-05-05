using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateSplat : MonoBehaviour {

	public bool constantSplat;

	private GameObject splatterParent;
	private Vector3 lastPosition;
	private Vector3 currentPosition;
	private Vector3 dirToCurrentPos;
	[HideInInspector]
	public static CreateSplat _instance;
	[HideInInspector]
	public static GameObject splatterPrefab;

	Vector2 rayOrigin;
	float rayLength = 5f;
	public LayerMask middleGroundMask;
	public bool onMiddleGround;

	public bool isSplatting;

	//For splat instantiation
	private LinkedList<Vector3> centerPositions;
	public float splatDistanceMin = 0.50f;
	private float newZvalue;

	public static CreateSplat instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<CreateSplat>();
			}
			return _instance;
		}
	}

	void Awake () 
	{
		//if(pManager == null)
		//{
		//	DontDestroyOnLoad(gameObject);
		//	pManager = this;
		//}
		//else if(pManager != this)
		//{
		//	Destroy(gameObject);
		//}
	}

	void Start()
	{
		splatterParent = new GameObject("Splatter Parent");
		lastPosition = transform.position;
		splatterPrefab = PoolManager.instance.splatterPrefab;

		//For splat instantiation
		centerPositions = new LinkedList<Vector3>();
		centerPositions.AddFirst(transform.position);
	}

	void Update()
	{

		//Ray checks for middleground
		RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector3.forward, rayLength, middleGroundMask);
		Debug.DrawRay(transform.position, Vector3.forward * rayLength, Color.red);
		if (hit2) 
		{
			onMiddleGround = true;
		}
		else 
		{
			onMiddleGround = false;
		}

		//This is where all splatting happens
		if(isSplatting)
		{
			if (constantSplat && onMiddleGround)
			{
				Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - newZvalue);
				SpawnSplat(pos, Color.black, true);
			}
			else if (onMiddleGround)
			{
				Color color = ColorStates.instance.GetColor();
				Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - newZvalue);
				SpawnSplat(pos, color, false); 
			}
		}
		//This meassures a direction vector
		currentPosition = transform.position;
		dirToCurrentPos = (currentPosition - lastPosition).normalized;
		lastPosition = transform.position;
	}

	//Spawns the player splat
	public void SpawnSplat(Vector3 position, Color color, bool isBlack)
	{
		float angle = Mathf.Atan2(dirToCurrentPos.y, dirToCurrentPos.x) * Mathf.Rad2Deg;
		if (Controller2D.instance.collisions.faceDir == -1 && !isBlack)
		{
			angle += 180;
		}

		//Currently to make sure new track is on top of old track
		newZvalue -= -0.001f;
		//Debug.Log(newZvalue);
		//Makes it so object has to have travelled a certain distance before a new splat can be created
		if ( (centerPositions.First.Value - transform.position).sqrMagnitude > splatDistanceMin * splatDistanceMin) 
		{
			Vector3 scale = transform.localScale;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			//color = ColorStates.instance.GetColor();
			PoolManager.instance.ReuseSplatter (splatterPrefab, position, q, color, scale);

			centerPositions.AddFirst(transform.position);
		}
	}
}