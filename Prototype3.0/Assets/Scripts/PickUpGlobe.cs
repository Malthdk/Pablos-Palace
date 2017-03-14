using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGlobe : MonoBehaviour {

	public Color orbColor;

	ParticleSystem pSystemConstant;
	ParticleSystem pSystemExplode1;
	ParticleSystem pSystemExplode2;
	ParticleSystemRenderer pRenderConstant;
	ParticleSystemRenderer pRenderExplode1;
	ParticleSystemRenderer pRenderExplode2;
	ColorStates colorStates;
	CircleCollider2D myCollider;
	Color currentColor;

	void Start () 
	{
		pSystemConstant = gameObject.transform.FindChild("FX_ParticleBall").GetComponent<ParticleSystem>();
		pSystemExplode1 = gameObject.transform.FindChild("FX_ParticleExplode1").GetComponent<ParticleSystem>();
		pSystemExplode2 = gameObject.transform.FindChild("FX_ParticleExplode2").GetComponent<ParticleSystem>();

		pRenderConstant = pSystemConstant.GetComponent<ParticleSystemRenderer>();
		pRenderExplode1 = pSystemExplode1.GetComponent<ParticleSystemRenderer>();
		pRenderExplode2 = pSystemExplode2.GetComponent<ParticleSystemRenderer>();

		colorStates = GameObject.Find("Player").GetComponent<ColorStates>();
		myCollider = gameObject.GetComponent<CircleCollider2D>();

		SetColor(pRenderConstant, orbColor);
		SetColor(pRenderExplode1, orbColor);
		SetColor(pRenderExplode2, orbColor);

		pSystemExplode1.Stop();
		pSystemExplode2.Stop();
		pSystemConstant.Play();
	}

	void Update () 
	{
		
	}

	void SetColor(ParticleSystemRenderer pRenderer, Color setColor)
	{
		Color color = pRenderer.material.color;
		//setColor.a = 0.5f;
		color = setColor;
		pRenderer.material.color = color;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			LevelManager.instance.numberOrbs --;
			//gameObject.SetActive(false);
			myCollider.enabled = false;
			pSystemConstant.Stop();
			pSystemConstant.Clear();
			pSystemExplode1.Play();
			pSystemExplode2.Play();
		}
	}

}
