using UnityEngine;
using System.Collections;

public class FleshPlant : RaycastController {

	public SpriteRenderer spriteRend;
	public float killTimeFraction;
	public float releaseTimeFraction;
	public Eat eat;
	public LayerMask passengerMask;

	public bool killLeft;
	public bool killRight;
	public bool killUp;

	private Color color;

	public override void Start () 
	{
		base.Start();

		color = spriteRend.material.color;
		color.a = 0;
		spriteRend.material.color = color;
	}

	public enum Eat
	{
		Opening,
		Closing,
		Kill
	}


	public override void Update () 
	{
		base.Update();

		switch(eat)
		{
		case Eat.Opening:
			Opening();
			break;

		case Eat.Closing:

			Closing();

			if (color.a >= 1)
			{
				eat = Eat.Kill;
			}

			break;

		case Eat.Kill:
			PlayerManager.pManager.KillPlayer();
			eat = Eat.Opening;
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			Debug.Log("entered kill zone");
			if (killUp)
			{
				ShootRaysUp();
			}
			if (killLeft)
			{
				ShootRaysLeft();
			}
			if (killRight)
			{
				ShootRaysRight();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			Debug.Log("exited kill zone");
			eat = Eat.Opening;
		}
	}

	void Closing()
	{
		color.a += killTimeFraction;
		spriteRend.material.color = color;
	}

	void Opening()
	{
		if(color.a > 0)
		{
			color.a -= releaseTimeFraction;
			spriteRend.material.color = color;
		}
	}

	void ShootRaysUp()
	{
		for (int i = 0; i < verticalRayCount; i ++)
		{
			float rayLength = 0.015f * 2f;			//Short rayLength

			Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * verticalRaySpacing * i;		//Rayorigin allways on topLeft.
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask); 		//Allways casting ray upwards.

			Debug.DrawRay(rayOrigin, Vector2.up * rayLength * 4, Color.red);

			if (hit)
			{
				Debug.Log ("you've hit it");
				eat = Eat.Closing;
			}
		}
	}

	void ShootRaysLeft()
	{
		for (int i = 0; i < horizontalRayCount; i ++)
		{
			float rayLength = 0.015f * 2f;			//Short rayLength

			Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.down * horizontalRaySpacing * i;		//Rayorigin allways on topLeft.
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, passengerMask); 		//Allways casting ray upwards.

			Debug.DrawRay(rayOrigin, Vector2.up * rayLength * 4, Color.red);

			if (hit)
			{
				Debug.Log ("you've hit it");
				eat = Eat.Closing;
			}
		}
	}

	void ShootRaysRight()
	{
		for (int i = 0; i < horizontalRayCount; i ++)
		{
			float rayLength = 0.015f * 2f;			//Short rayLength

			Vector2 rayOrigin = raycastOrigins.topRight + Vector2.down * horizontalRaySpacing * i;		//Rayorigin allways on topLeft.
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, passengerMask); 		//Allways casting ray upwards.

			Debug.DrawRay(rayOrigin, Vector2.up * rayLength * 4, Color.red);

			if (hit)
			{
				Debug.Log ("you've hit it");
				eat = Eat.Closing;
			}
		}
	}

}
