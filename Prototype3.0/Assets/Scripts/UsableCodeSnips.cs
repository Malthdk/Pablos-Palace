using UnityEngine;
using System.Collections;

public class UsableCodeSnips : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//FREEZE ABILITY
		//			if (Input.GetKeyDown(KeyCode.Space))
		//			{
		//				if (!controller.collisions.below)
		//				{
		//					inAir = true;
		//					storedVel = player.velocity;
		//					Debug.Log(storedVel);
		//
		//					if(inAir)
		//					{
		//						player.velocity = zeroVel;
		//						Debug.Log (player.velocity);
		//					}
		//				}
		//			}
		//			if (Input.GetKeyUp(KeyCode.Space) && inAir)
		//			{
		//				inAir = false;
		//				player.velocity = storedVel;
		//				Debug.Log ("applying:" + storedVel);
		//			}


		//Old DASH
		//DASH MECHANIC (WORKING)
		//			switch (dashState) 
		//			{
		//			case DashState.Ready:
		//				var isDashKeyDown = Input.GetKeyDown (KeyCode.Space);
		//				isdashing = false;
		//				if(isDashKeyDown && !controller.collisions.below)
		//				{
		//					savedVelocity = new Vector2(player.velocity.x, 0);
		//					player.velocity =  new Vector2(player.velocity.x * 5f, 4);
		//					dashState = DashState.Dashing;
		//				}
		//				break;
		//			case DashState.Dashing:
		//				dashTimer += Time.deltaTime * 3;
		//				isdashing = true;
		//				if(dashTimer >= maxDash)
		//				{
		//					dashTimer = maxDash;
		//					player.velocity = savedVelocity;
		//					dashState = DashState.Cooldown;
		//				}
		//				break;
		//			case DashState.Cooldown:
		//				dashTimer -= Time.deltaTime;
		//				isdashing = false;
		//				if(dashTimer <= 0)
		//				{
		//					dashTimer = 0;
		//					dashState = DashState.Ready;
		//				}
		//				break;
		//			}

		//New DASH
		//			if(isDashing)
		//			{
		//				dashTime -= Time.deltaTime;
		//				if ( dashTime < 0 )
		//				{
		//					isDashing = false;
		//				}
		//			}
		//
		//			if (controller.collisions.below && hasDoubleJumped)
		//			{
		//				hasDoubleJumped = false;
		//			}
		//			if (Input.GetKeyDown(KeyCode.Space))
		//			{
		//				if (!controller.collisions.below && !hasDoubleJumped)
		//				{
		//					isDashing = true;
		//					if(isDashing)
		//					{
		//						if(controller.collisions.left || controller.collisions.right)
		//						{
		//							isDashing = false;
		//							dashTime = 0.5f;
		//						}
		//						hasDoubleJumped = true;
		//						if(controller.collisions.faceDir == 1)
		//						{
		//							player.velocity.x = maxDashVelocity;
		//						}
		//						else if(controller.collisions.faceDir == -1)
		//						{
		//							player.velocity.x = -maxDashVelocity;
		//						}
		//
		//					}
		//				}
		//			}
		//
		//			if (Input.GetKeyUp(KeyCode.Space))					//For variable jump
		//			{
		//				isDashing = false;
		//				dashTime = 0.5f;
		//			}

		//Orange DASH
//		if(isDashing)
//		{
//			orangeDashTime -= Time.deltaTime;
//			if ( orangeDashTime < 0 )
//			{
//				isDashing = false;
//			}
//		}
//		
//		if (controller.collisions.below && hasDoubleJumped)
//		{
//			hasDoubleJumped = false;
//		}
//		if (Input.GetKeyDown(KeyCode.Space))
//		{
//			if (!controller.collisions.below && !hasDoubleJumped)
//			{
//				isDashing = true;
//				if(isDashing)
//				{
//					if(controller.collisions.left || controller.collisions.right)
//					{
//						isDashing = false;
//						orangeDashTime = 0.25f;
//					}
//					hasDoubleJumped = true;
//					if(controller.collisions.faceDir == 1)
//					{
//						player.velocity.x = orangeMaxDashVelocity;
//					}
//					else if(controller.collisions.faceDir == -1)
//					{
//						player.velocity.x = -orangeMaxDashVelocity;
//					}
//					
//				}
//			}
//		}
//		if (Input.GetKeyUp(KeyCode.Space))					//For variable jump
//		{
//			isDashing = false;
//			orangeDashTime = 0.25f;
//		}
	}
}
