using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour {

	//Publics
	public LayerMask collisionMask;

	//Privates
	private Vector2 rayOrigin;
	private bool playerClose;

	//Components
	BoxCollider2D myBoxCol;
	Bounds bounds;
	Transform player;

	void Start () 
	{
		bounds = myBoxCol.bounds;
		player = GameObject.Find("Player").transform;
	}

	void Update () 
	{
		if (playerClose)
		{
			Vector3 center = new Vector3(bounds.center.x, bounds.center.y, 0);
			Vector2 dir = player.position - center;

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, Mathf.Infinity, collisionMask);
			Debug.DrawRay(rayOrigin, dir * 15f, Color.red);
		}
	}

	void PlaceRayOrigin()
	{
		bounds.Expand (0.015f * -2);
		rayOrigin = new Vector2(bounds.center.x, bounds.center.y);

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			playerClose = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			playerClose = false;
		}
	}
}
