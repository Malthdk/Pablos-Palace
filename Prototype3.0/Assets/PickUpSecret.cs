using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSecret : MonoBehaviour {

	ParticleSystem pSystem;
	SpriteRenderer sRenderer;
	CircleCollider2D cCollider;
	void Start () 
	{
		cCollider = gameObject.GetComponent<CircleCollider2D>();
		sRenderer = gameObject.GetComponent<SpriteRenderer>();
		pSystem = transform.GetComponentInChildren<ParticleSystem>();
	}

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			pSystem.Play();
			PickedUp();
		}
	}

	void PickedUp()
	{
		UIManager.uiManager.score++;
		sRenderer.enabled = false;
		cCollider.enabled = false;
	}

	public void ResetSeOrb()
	{
		if (!sRenderer.enabled && !cCollider.enabled)
		{
			UIManager.uiManager.score--;
			sRenderer.enabled = true;
			cCollider.enabled = true;	
		}
	}
}
