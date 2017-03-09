using UnityEngine;
using System.Collections;

public class ParticleColor : MonoBehaviour {

	//ParticleSystem pSystem;
	//Abilities abilities;
	GameObject player;
	ParticleSystemRenderer pColor;
	ColorStates colorStates;
	private Color playerColor;

	void Start () 
	{
		//pSystem = GetComponentInChildren<ParticleSystem>();
		//abilities = GetComponentInParent<Abilities>();
		//pSystem.gameObject.SetActive(false);
		player = GameObject.Find("Player");	
		pColor = GetComponent<ParticleSystemRenderer>();
		colorStates = GameObject.Find("Player").GetComponent<ColorStates>();

		//SetColor();
	}

	void Update () 
	{
		SetColor();
	}

	private void SetColor() 
	{
		playerColor = GameObject.Find("Player").transform.FindChild("Graphics").GetComponent<SpriteRenderer>().color;
		Color color1 = pColor.material.color;

		color1 = playerColor;
		pColor.material.color = color1;
		if (pColor.trailMaterial != null)
		{
			pColor.trailMaterial.color = color1;
		}
	}
}