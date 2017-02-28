using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGlobe : MonoBehaviour {

	ParticleSystem pSystemConstant;
	ParticleSystem pSystemExplode;
	ParticleSystemRenderer pRenderConstant;
	ParticleSystemRenderer pRenderExplode;
	ColorStates colorStates;

	void Start () 
	{
		pSystemConstant = gameObject.transform.FindChild("FX_ParticleBall").GetComponent<ParticleSystem>();
		//pSystemExplode = gameObject.transform.FindChild("FX_ParticleExplode").GetComponent<ParticleSystem>();
		pRenderConstant = pSystemConstant.GetComponent<ParticleSystemRenderer>();
		//pRenderExplode = pSystemExplode.GetComponent<ParticleSystemRenderer>();

		colorStates = GameObject.Find("Player").GetComponent<ColorStates>();

		SetColor();
		//pSystemExplode.Clear();
		//pSystemExplode.Stop();
		pSystemConstant.Play();
	}
	

	void Update () 
	{
		
	}

	void SetColor()
	{
		Color color1 = pRenderConstant.material.color;
		//Color color2 = pRenderExplode.trailMaterial.color;

		if (gameObject.tag == "blueBox")
		{
			color1 = colorStates.blue;
			//color2 = colorStates.blue;
			pRenderConstant.material.color = color1;
			//pRenderExplode.trailMaterial.color = color2;
		}
		else if (gameObject.tag == "yellowBox")
		{
			color1 = colorStates.yellow;
			//color2 = colorStates.yellow;
			pRenderConstant.material.color = color1;
			//pRenderExplode.trailMaterial.color = color2;
		}
		else if (gameObject.tag == "redBox")
		{
			color1 = colorStates.red;
			//color2 = colorStates.red;
			pRenderConstant.material.color = color1;
			//pRenderExplode.trailMaterial.color = color2;
		}
		else if (gameObject.tag == "greenBox")
		{
			color1 = colorStates.green;
			//color2 = colorStates.green;
			pRenderConstant.material.color = color1;
			//pRenderExplode.trailMaterial.color = color2;
		}
		else if (gameObject.tag == "orangeBox")
		{
			color1 = colorStates.orange;
			//color2 = colorStates.orange;
			pRenderConstant.material.color = color1;
			//pRenderExplode.trailMaterial.color = color2;
		}
		else if (gameObject.tag == "purpleBox")
		{
			color1 = colorStates.purple;
			//color2 = colorStates.purple;
			pRenderConstant.material.color = color1;
			//pRenderExplode.trailMaterial.color = color2;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			gameObject.SetActive(false);
			pSystemConstant.Stop();
			pSystemConstant.Clear();
			//pSystemExplode.Play();
			PickedUpGlobe(other.gameObject, gameObject.tag);
		}
	}

	void PickedUpGlobe(GameObject gameObj, string tag)
	{
		if (tag == "blueBox")
		{
			gameObj.tag = "blue";
		}
		else if (tag == "yellowBox")
		{
			gameObj.tag = "yellow";
		}
		else if (tag == "redBox")
		{
			gameObj.tag = "red";
		}
		else if (tag == "greenBox")
		{
			gameObj.tag = "green";
		}
		else if (tag == "orangeBox")
		{
			gameObj.tag = "orange";
		}
		else if (tag == "purpleBox")
		{
			gameObj.tag = "purple";
		}
	}
}
