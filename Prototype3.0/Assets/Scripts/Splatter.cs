using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
	//Publics
	public static Splatter splatManager; //Hvad er det her?
	public float splatStayTime = 3f;
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

	private void Awake()
    {
		material = GetComponent<MeshRenderer>().material;
		player = GameObject.Find("Player");	
		CompareColors();
    }

    private void Start()
    {
		randomRoll = Random.Range(1, 10);
		endPosition = Random.Range(0.3f, 0.5f);
		speed = Random.Range(0.3f, 1f);
		scale = transform.localScale.y;
		Anchor_Position = transform.localPosition;
		StartCoroutine(DestroySplat());
		Anchor_Position = new Vector3(transform.localPosition.x, transform.localPosition.y - endPosition, transform.localPosition.z);
    }

	void Update ()
	{
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

	private void CompareColors() {
		playerColor = GameObject.Find("Player").transform.FindChild("Graphics").GetComponent<SpriteRenderer>().color;
		if (player.gameObject.tag == "blue") 
		{
			myColor = "blue";
			Color colorStart = playerColor;
				//new Color (0.15f, 0.15f, 0.82f);
			material.color = colorStart; //0.17 0.17 0.85

		} else if (player.gameObject.tag == "purple") 
		{
			myColor = "purple";
			Color colorStart = playerColor;
				//new Color (0.55f,0.21f,0.7f);
			material.color = colorStart; //0.55f,0.21f,0.7f
		} 
		else if (player.gameObject.tag == "red") 
		{
			myColor = "red";
			Color colorStart = playerColor;
				//new Color (0.85f,0f,0.22f);
			material.color = colorStart; //0.85f,0f,0.22f
		} 
		else if (player.gameObject.tag == "green") 
		{
			myColor = "green";
			Color colorStart = playerColor;;
				//new Color (0.185f,0.55f,0.175f);
			material.color = colorStart; //0.185f,0.55f,0.175f
		} 
		else if (player.gameObject.tag == "yellow") 
		{
			myColor = "yellow";
			Color colorStart = playerColor;
				//new Color (1f,0.82f,0.22f);
			material.color = colorStart; //1f,0.82f,0.22f
		} 
		else if (player.gameObject.tag == "orange") 
		{
			myColor = "orange";
			Color colorStart = playerColor;
				//new Color (1f,0.42f,0.0f);
			material.color = colorStart; //1f,0.42f,0.0f
		}
	}
		
	public IEnumerator DestroySplat()
	{
		yield return new WaitForSeconds(splatStayTime);

		scaling = false;
		material.renderQueue = 2998;
		Color color = material.color;

		color = ChangeBrightness(playerColor, 0.5f);
		material.color = color;
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
