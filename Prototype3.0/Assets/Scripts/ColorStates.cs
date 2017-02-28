using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStates : MonoBehaviour {

	//Publics


	//Privates
	public Color red = new Color(0.82F, 0F, 0.2F);			//Red farve
	public Color yellow = new Color(1F, 0.8F, 0.2F);		//Yellow farve
	public Color blue = new Color(0.2F, 0.18F, 1F);		//Blue farve (0.2F, 0.18F, 1F);
	public Color green = new Color(0F, 0.6F, 0.05F);		//Green farve
	public Color purple = new Color(0.55F, 0.15F, 0.7F);	//Purple farve
	public Color orange = new Color(0.96F, 0.55F, 0F);		//Orange farve

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
		myRenderer = gameObject.transform.FindChild("Graphics").GetComponent<SpriteRenderer>();
	}

	void Update () 
	{

		if (gameObject.tag == "red")
		{
			//myRenderer.color = red;
			ChangeColor(red);
		}
		else if (gameObject.tag == "blue")
		{
			//myRenderer.color = blue;
			ChangeColor(blue);
		}
		else if (gameObject.tag == "yellow")
		{
			//myRenderer.color = yellow;
			ChangeColor(yellow);
		}
		else if (gameObject.tag == "green")
		{
			//myRenderer.color = green;
			ChangeColor(green);
		}
		else if (gameObject.tag == "orange")
		{
			//myRenderer.color = orange;
			ChangeColor(orange);
		}
		else if (gameObject.tag == "purple")
		{
			//myRenderer.color = purple;
			ChangeColor(purple);
		}
	}

	public void ChangeColor(Color newColor)
	{
		Color color = myRenderer.color;
		color = Color.Lerp(color, newColor, Mathf.PingPong(Time.deltaTime * 4f, 1));
		myRenderer.color = color;
	}

	public static Vector4 hexColor(float r, float g, float b, float a)
	{
		Vector4 color = new Vector4(r/255, g/255, b/255, a/255);
		return color;
	}
}
