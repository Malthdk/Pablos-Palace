using UnityEngine;
using System.Collections;

public class PullPush : MonoBehaviour {

	//Publics
	public float pullSpeed;

	//Privates and hidden
	//[HideInInspector]
	public bool canGrab;
	public bool isPulling;
	[HideInInspector]
	public GameObject pushBlock;
	private Rigidbody2D rgb;
	private Player player;
	private Controller2D controller;

	void Start () 
	{
		player = GetComponent<Player>();
		controller = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (canGrab)
		{
			if (Input.GetButton("Special"))
			{
				Grab();
			}
		}

		if (isPulling)
		{
			if (Input.GetButtonUp("Special"))
			{
				canGrab = false;
				Release();
			}	
		}
		else if (!canGrab)
		{
			Release();
		}
	}

//	void OnTriggerEnter2D( Collider2D other)
//	{
//		if (other.tag == "pushBox" && (controller.collisions.right || controller.collisions.left) && controller.collisions.below)
//		{
//			canGrab = true;
//			pushBlock = other.transform.parent.gameObject;
//			rgb = other.GetComponentInParent<Rigidbody2D>();
//		}
//	}
//	void OnTriggerExit2D(Collider2D other)
//	{
//		Release();
//	}

	void Grab()
	{
		isPulling = true;
		pushBlock.transform.parent = gameObject.transform;
		pushBlock.layer = 0;
		player.moveSpeed = pullSpeed;
	}

	void Release()
	{
		isPulling = false;
		pushBlock.transform.parent = null;
		pushBlock.layer = 8;
		if (controller.collisions.below)
		{
			player.moveSpeed = player.groundSpeed;
		}
		else
		{
			player.moveSpeed = player.airSpeed;
		}
	}
}
