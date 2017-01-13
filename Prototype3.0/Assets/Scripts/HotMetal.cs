using UnityEngine;
using System.Collections;

public class HotMetal : MonoBehaviour {

	private SpriteRenderer spriteRend;
	//public float heatAndCoolFraction;
	public LayerMask collisionMask;
	public Collider2D[] colliders;
	public int numOfWaterParticles;

	private BoxCollider2D bCollider;
	public HeatAndCool heatAndCool;
	public Color color;
	private Bounds bounds;

	private Vector2 pointA;
	private Vector2 pointB;

	public bool deadly;

	void Start () 
	{
		bounds = GetComponent<BoxCollider2D>().bounds;
		pointA = new Vector2(bounds.min.x, bounds.min.y);
		pointB = new Vector2(bounds.max.x, bounds.max.y);

		spriteRend = GetComponent<SpriteRenderer>();
		color = spriteRend.material.color;

		SetColor();
	}
	
	public enum HeatAndCool
	{
		Hawt,
		Heating,
		Cooling,
		Cold
	}

	void Update () 
	{
		colliders = Physics2D.OverlapAreaAll(pointA, pointB, collisionMask);

		if (colliders.Length <5)
		{
			heatAndCool = HeatAndCool.Heating;
		}

		switch (heatAndCool)
		{
		case HeatAndCool.Hawt:
			deadly = true;
			Debug.Log("ishawt");
			break;

		case HeatAndCool.Heating:
			Heating();
			Debug.Log("isHeating");
			break;

		case HeatAndCool.Cooling:
			Cooling();
			Debug.Log("isCooling");
			break;

		case HeatAndCool.Cold:
			gameObject.tag = "Untagged";
			deadly = false;
			Debug.Log("isCold");
			break;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.tag == "DynamicParticle")
		{
			DynamicParticle dynPart = other.collider.GetComponent<DynamicParticle>();

			if(dynPart.currentState == DynamicParticle.STATES.WATER && colliders.Length >= numOfWaterParticles && heatAndCool != HeatAndCool.Cold)
			{
				heatAndCool = HeatAndCool.Cooling;
			}
		}
		if (other.collider.name == "Player")
		{
			PlayerManager.pManager.KillPlayer();
		}
	}

	void SetColor()
	{
		color = new Color(1,0,0);
		spriteRend.color = color;
	}

	void Heating()
	{
		//color.r += heatAndCoolFraction;
		//spriteRend.material.color = color;
		color = Color.Lerp(color, Color.red, Mathf.PingPong(Time.deltaTime * 0.5f, 1));
		spriteRend.color = color;

		if (color.b <= 0.1f && color.g <= 0.1f)
		{
			heatAndCool = HeatAndCool.Hawt;
		}
	}

	void Cooling()
	{
//		if(color.r > 0)
//		{
//			Debug.Log ("COOOLING");
//			color.r -= heatAndCoolFraction;
//			spriteRend.material.color = color;
//		}
		color = Color.Lerp(color, Color.white, Mathf.PingPong(Time.deltaTime * 0.5f, 1));
		spriteRend.color = color;

		if (color.b >= 0.90f & color.g >= 0.90f)
		{
			heatAndCool = HeatAndCool.Cold;
		}
	}
}