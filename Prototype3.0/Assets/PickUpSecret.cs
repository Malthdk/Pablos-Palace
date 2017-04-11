using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSecret : MonoBehaviour {

	ParticleSystem pSystem;
	SpriteRenderer sRenderer;
	CircleCollider2D cCollider;

	// FOR SOUND
	public AudioClip pickUpSound;
	private AudioSource source;


	void Start () 
	{
		cCollider = gameObject.GetComponent<CircleCollider2D>();
		sRenderer = gameObject.GetComponent<SpriteRenderer>();
		pSystem = transform.GetComponentInChildren<ParticleSystem>();
		source = GetComponent<AudioSource>();
	}

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			source.PlayOneShot(pickUpSound, 0.8f);
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
