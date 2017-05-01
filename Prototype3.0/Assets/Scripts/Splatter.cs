using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
	//Publics
	public static Splatter splatManager; //Hvad er det her?
	public float splatStayTime = 3f;
	public float blackSplatStayTime = 1.5f;
	public float turnedBlackSplatStayTime = 1.5f;
	public float blackTurnTime = 1f;
	public float correctionFactor = 0.5f;
	public float colorChangeTime = 1f;
	public LayerMask collisionMask;

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
	private bool isBlackSplat = false;

	private bool isActionPerformed;
	private Bounds bounds;
	public Collider2D[] colliders;
	private Vector2 pointA;
	private Vector2 pointB;
	private ParticleSystem changeColorParticles;
	private ParticleSystem dissapearParticles;

	private void Awake()
    {
		boxCollider = GetComponent<BoxCollider2D>();
		material = GetComponent<MeshRenderer>().material;
		player = GameObject.Find("Player");
    }

    private void Start()
    {
		changeColorParticles = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
		dissapearParticles = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
    }

	void Update ()
	{
		if (isActive)
		{
			colliders = Physics2D.OverlapAreaAll(pointA, pointB, collisionMask);
			
			if (colliders.Length > 0 && gameObject.tag != "killTag")
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
		if (isBlackSplat)
		{
			splatStayTime = blackSplatStayTime;
		}
	}

	public void OnObjectReuse () {
		gameObject.SetActive(true);
		isActive = true;

		if (material.color == black)
		{
			isBlackSplat = true;
			gameObject.tag = "killTag";
			gameObject.layer = 15;
			splatStayTime = blackSplatStayTime;
		}

		//This handles the dripping and variations in trail (MAYBE DELETE THIS IN THIS FUNCTION?)
		randomRoll = Random.Range(1, 10);
		endPosition = Random.Range(0.3f, 0.5f);
		speed = Random.Range(0.3f, 1f);
		scale = transform.localScale.y;
		Anchor_Position = transform.localPosition;
		Anchor_Position = new Vector3(transform.localPosition.x, transform.localPosition.y - endPosition, transform.localPosition.z);

		//Starting the Destroy corotine with correct color
		playerColor = ColorStates.instance.GetColor();
		if (isBlackSplat)
		{
			StartCoroutine(DestroySplat(black, colorChangeTime));
		}
		else
		{
			StartCoroutine(DestroySplat(playerColor, colorChangeTime));
		}
		//Bounds are used to check for collision with other splat prefabs
		bounds = GetComponent<BoxCollider2D>().bounds;
		pointA = new Vector2(bounds.min.x, bounds.min.y);
		pointB = new Vector2(bounds.max.x, bounds.max.y);
	}

	private IEnumerator PlayOnce(){

		while(!isActionPerformed){
			splatStayTime = turnedBlackSplatStayTime;
			yield return new WaitForEndOfFrame ();
			StopAllCoroutines();
			StartCoroutine(TurnBlack(colorChangeTime));
			isActionPerformed = true;
		}
	}

	public IEnumerator DestroySplat(Color brightColor, float time)
	{
		yield return new WaitForSeconds(splatStayTime);

		boxCollider.enabled = false;
		scaling = false;
		//dissapearParticles.Play(); //If we want a particle effect when it starts to dissapear - this also has to take into account if the particle is black etc. which is why it is not working correctly atm. 

		//material.renderQueue = 2999;

		float elapsedTime = 0;
		Color color2 = ChangeBrightness(brightColor, correctionFactor);

		while (elapsedTime < time)
		{
			FadeColor(color2, time, elapsedTime);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		isActive = false;
	}

	//Sepereate IEnumerator needed because i cannot restart the original DestroySplat IEnumerator.. it wont start with a different splatStayTime.
//	public IEnumerator DestroyBlackSplat(Color brightColor, float time)
//	{
//		yield return new WaitForSeconds(blackSplatStayTime);
//
//		boxCollider.enabled = false;
//		scaling = false;
//		//material.renderQueue = 2999;
//
//		float elapsedTime = 0;
//		Color color2 = ChangeBrightness(brightColor, correctionFactor);
//
//		while (elapsedTime < time)
//		{
//			FadeColor(color2, time, elapsedTime);
//			elapsedTime += Time.deltaTime;
//			yield return new WaitForEndOfFrame();
//		}
//		isActive = false;
//	}

	//Handles the splat turning black
	public IEnumerator TurnBlack(float time)
	{
		isBlackSplat = true;
		yield return new WaitForSeconds(blackTurnTime);

		gameObject.tag = "killTag";
		gameObject.layer = 15;
		changeColorParticles.Play();
		float elapsedTime = 0;
		while (elapsedTime < time)
		{
			FadeColor(black, time, elapsedTime);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		StartCoroutine(DestroySplat(black, colorChangeTime));
	}

	//Fades the material of a color to a new color based on a specific time interval
	public void FadeColor(Color newColor, float time, float elapsedTime)
	{
		Color color = material.color;
		color = Color.Lerp(color, newColor, (elapsedTime / time));
		material.color = color;
	}

	//Changes the brightness of a color
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
