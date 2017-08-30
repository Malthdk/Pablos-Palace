using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmashBlock : RaycastController {

	public LayerMask collisionMask1;

	public bool isHorizontal, isVertical, constantSmash, Chandelier;
	public float riseSpeed;
	public float waitTimeConstSmash;

	public Vector2 smashForce;

	public float groundTime;

	private Vector2 rayOriginTargetRay;
	public float rayLength;

	private Vector3 startPosition;
	private bool shootingRay;
	private bool playerHit;
	private bool retreating;

	public BoxCollider2D bCollider;
	private Smashing smashing;
	private Rigidbody2D rgb;

	public override void Start()
	{
		base.Start();

		PlaceRayOrigin();
		startPosition = transform.position;
		rgb = gameObject.GetComponent<Rigidbody2D>();
		smashing = Smashing.Ready;

		if (constantSmash)
		{
			shootingRay = false;
			StartCoroutine(ConstantSmash());
		}
		else
		{
			shootingRay = true;
		}
	}

	public enum Smashing 
	{
		Ready,
		Smashing,
		Retreating,
		Destroyed
	}

	public override void Update () 
	{
		base.Update();
			
		if (shootingRay)
		{
			RaycastHit2D hit = Physics2D.Raycast(rayOriginTargetRay, -Vector2.up, rayLength, collisionMask1);
			Debug.DrawRay(rayOriginTargetRay, -Vector2.up *rayLength, Color.red);

			if (hit.collider != null) 
			{
				Debug.Log("hit player");
				smashing = Smashing.Smashing;

			}
		}
		if (retreating)
		{
			float step = riseSpeed * Time.deltaTime;
			rgb.transform.position = Vector3.MoveTowards(rgb.gameObject.transform.position, startPosition, step);

			if (rgb.transform.position == startPosition)
			{
				retreating = false;
				if (constantSmash)
				{
					shootingRay = false;

					StartCoroutine(ConstantSmash());
				}
				else
				{
					shootingRay = true;
				}
			}
		}

		switch(smashing)
		{
		case Smashing.Ready:
			break;

		case Smashing.Smashing:
			rgb.isKinematic = false;
			shootingRay = false;

			if(!Chandelier)
			{
				rgb.AddForce(smashForce, ForceMode2D.Impulse);
			}

			float rayLength = skinWidth * 40;		
			for (int i = 0; i < verticalRayCount; i ++)
			{
				Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i);		//Rayorigin allways on bottomleft.
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, rayLength, collisionMask1); 		//Allways casting ray.

				Debug.DrawRay(rayOrigin, -Vector2.up * rayLength, Color.blue);
				if (hit && hit.distance !=0)
				{
					Debug.Log("kill player");
					PlayerManager.instance.KillPlayer();

					//Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), hit.collider);

					BoxCollider2D boxCollider = hit.transform.gameObject.GetComponent<BoxCollider2D>();
					boxCollider.enabled = false;
				}
			}
			StartCoroutine(Wait());
			break;

		case Smashing.Retreating:
			rgb.isKinematic = true;
			retreating = true;
			break;
		case Smashing.Destroyed:
			rgb.isKinematic = true;
			break;
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(groundTime);
		if (Chandelier)
		{
			smashing = Smashing.Destroyed;
		}
		else
		{
			smashing = Smashing.Retreating;
		}
	}

	IEnumerator ConstantSmash()
	{
		smashing = Smashing.Ready;

		yield return new WaitForSeconds(waitTimeConstSmash);

		smashing = Smashing.Smashing;
	}

	void PlaceRayOrigin()
	{
		Bounds bounds = bCollider.bounds;
		bounds.Expand (0.015f * -2);

		if(isHorizontal)
		{
			rayOriginTargetRay = new Vector2(bounds.max.x, bounds.center.y);
		}

		if(isVertical)
		{
			rayOriginTargetRay = new Vector2(bounds.center.x, bounds.min.y);
		}
	}
}
