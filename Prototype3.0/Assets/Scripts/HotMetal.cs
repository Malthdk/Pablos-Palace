using UnityEngine;
using System.Collections;

public class HotMetal : MonoBehaviour {

	//Public
	public float waitTime;
	public bool deadly;
	public LayerMask collisionMask;
	public Collider2D[] colliders;
	public int numOfWaterParticles;
	public HeatAndCool heatAndCool;

	//Private and hidden
	private SpriteRenderer spriteRend;
	//public float heatAndCoolFraction;
	private BoxCollider2D bCollider;
	private Color color;
	private Bounds bounds;

	private float tempWaitTime;
	private Vector2 pointA;
	private Vector2 pointB;

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
		Hot,
		Heating,
		Cooling,
		Cold
	}

	void Update () 
	{
		colliders = Physics2D.OverlapAreaAll(pointA, pointB, collisionMask);

		if (colliders.Length < numOfWaterParticles && heatAndCool != HeatAndCool.Hot && heatAndCool != HeatAndCool.Cold)
		{
			heatAndCool = HeatAndCool.Heating;
		}

		switch (heatAndCool)
		{
		case HeatAndCool.Hot:
			gameObject.tag = "blackBox";
			//deadly = true;
			color = new Color(1f, 0.4f, 0f);
			spriteRend.color = color;

			Debug.Log("isHot");
			break;

		case HeatAndCool.Heating:
			MoreHot();
			Debug.Log("isHeating");
			break;

		case HeatAndCool.Cooling:
			gameObject.tag = "Untagged";
			MoreCold();
			Debug.Log("isCooling");
			break;

		case HeatAndCool.Cold:
			gameObject.tag = "Untagged";
			//deadly = false;
			tempWaitTime -= Time.deltaTime;
			color = new Color(0.7f, 0.9f, 1f);
			spriteRend.color = color;

			if (tempWaitTime <= 0)
			{
				heatAndCool = HeatAndCool.Heating;
				tempWaitTime = waitTime;
			}
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
	}

	void SetColor()
	{
		color = new Color(1,0,0);
		spriteRend.color = color;
	}

	void MoreHot()
	{
		//color.r += heatAndCoolFraction;
		//spriteRend.material.color = color;

		color = Color.Lerp(color, Color.red, Mathf.PingPong(Time.deltaTime * 0.5f, 1));
		spriteRend.color = color;

		if (color.b <= 0.1f && color.g <= 0.1f)
		{
			heatAndCool = HeatAndCool.Hot;
		}
	}

	void MoreCold()
	{
//		if(color.r > 0)
//		{
//			Debug.Log ("COOOLING");
//			color.r -= heatAndCoolFraction;
//			spriteRend.material.color = color;
//		}
		color = Color.Lerp(color, Color.white, Mathf.PingPong(Time.deltaTime * 0.5f, 1));
		spriteRend.color = color;

		if (color.b >= 0.90f && color.g >= 0.90f)
		{
			heatAndCool = HeatAndCool.Cold;
		}
	}
}