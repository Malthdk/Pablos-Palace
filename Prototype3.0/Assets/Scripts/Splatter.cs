using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
	//Publics
	public static Splatter splatManager; //Hvad er det her?
	public float splatStayTime = 3f;
	public float blackTurnTime = 1f;
	public float correctionFactor = 0.5f;

	//Privates
	private int randomRoll;
	private float scale, speed, endPosition;
	private bool scaling;
	private string myColor;
	private Vector3 Anchor_Position;
	private Material material;
	private GameObject player;
	private Color playerColor;
	private BoxCollider2D boxCollider;
	private Color black = new Color(0f, 0f, 0f);
	private bool isActive;

	private bool isActionPerformed;
	public LayerMask collisionMask;
	private Bounds bounds;
	public Collider2D[] colliders;
	private Vector2 pointA;
	private Vector2 pointB;

	private void Awake()
    {
		boxCollider = GetComponent<BoxCollider2D>();
		material = GetComponent<MeshRenderer>().material;
		player = GameObject.Find("Player");	
		CompareColors();
    }

    private void Start()
    {
		isActive = true;
		randomRoll = Random.Range(1, 10);
		endPosition = Random.Range(0.3f, 0.5f);
		speed = Random.Range(0.3f, 1f);
		scale = transform.localScale.y;
		Anchor_Position = transform.localPosition;
		StartCoroutine(DestroySplat(playerColor));
		Anchor_Position = new Vector3(transform.localPosition.x, transform.localPosition.y - endPosition, transform.localPosition.z);

		bounds = GetComponent<BoxCollider2D>().bounds;
		pointA = new Vector2(bounds.min.x, bounds.min.y);
		pointB = new Vector2(bounds.max.x, bounds.max.y);
    }

	void Update ()
	{
		if (isActive)
		{
			colliders = Physics2D.OverlapAreaAll(pointA, pointB, collisionMask);
			
			if (colliders.Length > 0)
			{
				StartCoroutine(PlayOnce());
			}

			if (randomRoll>8)
			{
				scaling = true;
			}
			if (scaling)
			{
				gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.position, Anchor_Position, Time.deltaTime * speed);
				transform.localScale = new Vector3(scale * (0.3f + endPosition), scale * (0.5f + endPosition), transform.localScale.z);
				material.SetFloat("_Warp", 0.3f);
			}
		}
	}

	private void CompareColors() {
		playerColor = GameObject.Find("Player").transform.FindChild("Graphics").GetComponent<SpriteRenderer>().color;

		Color colorStart = playerColor;
				//new Color (0.15f, 0.15f, 0.82f);
		material.color = colorStart; //0.17 0.17 0.85

	}

	private IEnumerator PlayOnce(){

		while(!isActionPerformed){
			yield return new WaitForEndOfFrame ();

			StopAllCoroutines();
			StartCoroutine(BlackSplat());
			isActionPerformed = true;
		}
	}

	public IEnumerator DestroySplat(Color brightColor)
	{
		yield return new WaitForSeconds(splatStayTime);

		boxCollider.enabled = false;
		scaling = false;
		material.renderQueue = 2998;
		Color color = material.color;

		color = ChangeBrightness(brightColor, 0.5f);
		material.color = color;
		isActive = false;
	}

	public IEnumerator BlackSplat()
	{
		yield return new WaitForSeconds(blackTurnTime);

		gameObject.tag = "killTag";
		gameObject.layer = 15;
		Color color = material.color;
		color = black;
		//color = Color.Lerp(color, black, Mathf.PingPong(Time.deltaTime * 1f, 1));
		material.color = color;
		StartCoroutine(DestroySplat(black));
	}


	public Color ChangeBrightness( Color color, float correctionFactor)
	{
		float red = color.r * 255;
		float green = color.g * 255;
		float blue = color.b * 255;

		if (correctionFactor < 0)
		{
			correctionFactor = 1 + correctionFactor;
			red *= correctionFactor;
			green *= correctionFactor;
			blue *= correctionFactor;
		}
		else
		{
			red = (255 - red) * correctionFactor + red;
			green = (255 - green) * correctionFactor + green;
			blue = (255 - blue) * correctionFactor + blue;
		}
		return new Color(red/255, green/255, blue/255);
	}		
}
