using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStates : MonoBehaviour {

	//Publics
	[HideInInspector]
	public Color activeColor;
	public bool isWhite;
	public float colorChangeTime = 1.5f;
	//Privates
	private SpriteRenderer myRenderer;
	[HideInInspector]
	public static ColorStates _instance;
	public static ColorStates instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<ColorStates>();
			}
			return _instance;
		}
	}

	void Start () 
	{
		activeColor = Color.white;
		myRenderer = gameObject.transform.FindChild("Graphics").GetComponent<SpriteRenderer>();
	}

	void Update () 
	{
		if (activeColor == Color.white)
		{
			isWhite = true;
		}
		else 
		{
			isWhite = false;
		}
	}

	//Detects the collision with an orb
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "orb")
		{
			PickUpGlobe pickUpGlobe = other.gameObject.GetComponent<PickUpGlobe>();
			StartCoroutine(ChangeColor(pickUpGlobe.orbColor, colorChangeTime));
		}
	}

	//Changes the color on the player
	public IEnumerator ChangeColor(Color newColor, float time)
	{
		float elapsedTime = 0;
		while (elapsedTime < time)
		{
			FadeColor(newColor, time, elapsedTime);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	//Fades the material of a color to a new color based on a specific time interval
	public void FadeColor(Color newColor, float time, float elapsedTime)
	{
		Color color = myRenderer.color;
		color = Color.Lerp(color, newColor, (elapsedTime / time));
		myRenderer.color = color;
		activeColor = color;
	}

	//Gets the current active color
	public Color GetColor() {
		return activeColor;
	} 
}
