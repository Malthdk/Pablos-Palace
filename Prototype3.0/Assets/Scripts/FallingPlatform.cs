﻿using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

	// Should it be a falling platform?
	public bool fallingPlatform = true;
	
	// How long time till platform falls
	private float fallTimer = 0.01f;
	
	public LayerMask passengerMask; 	//Passenger layer
	private Rigidbody2D platformrb;

	BoxCollider2D collider;				//collider
	Vector2 topLeft;					//top left corner of platform
	public float verticalRaySpacing;	//space between rays
	public int verticalRayCount = 4;	//number of rays

	//For flashing
	private float timer = 1.5f;
	private float flashTimer = 0.04f;
	private float flashDuration = 0.09f;
	public bool pltformActivated = false;

	Transform transform;
	MeshRenderer renderer;
//	public Vector2 fallSpeed = new Vector3(0f, -5f, 0);

	void Start () 
	{
		renderer = GetComponent<MeshRenderer>();
		platformrb = GetComponent<Rigidbody2D>();
		collider = GetComponent<BoxCollider2D>();
		transform = GetComponent<Transform>();

		UpdateRaycastSpace();
		UpdateRaycastOrigin();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (fallingPlatform)
		{

		}

		if (pltformActivated)
		{
			Falls ();
		}
	}
	void Falls()
	{
		timer -= Time.deltaTime;
		if(timer >5f)
		{
			
		}
		else if(timer >0)
		{
			flashTimer -= Time.deltaTime;
			if(flashTimer <=0)
			{
				flashTimer = flashDuration;
				Flash ();
			}
		}
		else
		{
			renderer.enabled = true;
			timer = 1.5f;
			flashTimer = 0.04f;
			flashDuration = 0.09f;
			pltformActivated = false;
			gameObject.SetActive(false);
		}
	}

	//KEEP THIS SECTION IN CASE WE WANT ACTUAL FALLING PLATFORMS
//	IEnumerator Fall()
//	{
//		yield return new WaitForSeconds(fallTimer);
//		//platformrb.isKinematic = false;
//
//		GetComponent<Collider2D>().isTrigger = true;
//		yield return 0;
//
//	}

	void UpdateRaycastSpace()
	{
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);	
		Bounds bounds = collider.bounds;									//taking the bounds of the platform
		verticalRaySpacing = bounds.size.x / (verticalRayCount -1);			//calculating the space between rays
	}

	void UpdateRaycastOrigin()
	{
		Bounds bounds = collider.bounds;
		topLeft = new Vector2(bounds.min.x, bounds.max.y);					//defining top left corner of platform
	}

	void Flash()
	{
		renderer.enabled = !renderer.enabled;
		//Light light = GetComponent<Light>();
		//light.enabled = !light.enabled;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			ShootRay();	
			Debug.Log("Shoot ray");
		}
	}

	void ShootRay()
	{
		for (int i = 0; i < verticalRayCount; i ++)
		{
			float rayLength = 0.015f * 2f;			//Short rayLength

			Vector2 rayOrigin = topLeft + Vector2.right * verticalRaySpacing * i;		//Rayorigin allways on topLeft.
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask); 		//Allways casting ray upwards.

			Debug.DrawRay(rayOrigin, Vector2.up * rayLength * 4, Color.red);

			if (hit)
			{
				Debug.Log ("you've hit it");
				pltformActivated = true;
			}
		}
	}
	public void Reset()
	{
		timer = 1.5f;
		pltformActivated = false;
		renderer.enabled = true;
		gameObject.SetActive(true);
	}
}
