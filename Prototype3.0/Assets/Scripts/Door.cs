﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	SpriteRenderer myRenderer;
	private bool isOpen = false;

	public string nextLevelName;

	// FOR SOUND
	public AudioClip completeSound;
	private AudioSource source;

	void Start () 
	{
		myRenderer = gameObject.GetComponent<SpriteRenderer>();
		source = this.gameObject.GetComponent<AudioSource>();
	}
	

	void Update () 
	{
		if (LevelManager.instance.numberOrbs == 0)
		{
			myRenderer.color = Color.green;
			isOpen = true;
		}
		else
		{
			myRenderer.color = Color.red;
			isOpen = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			StartCoroutine("CompletedLevel");
			/*if (isOpen)
			{
				source.PlayOneShot(completeSound, 0.8f);
				LevelManager.instance.NextLevel();
			}*/
		}
	}

	IEnumerator CompletedLevel() {
		if (isOpen)
		{
			source.PlayOneShot(completeSound, 0.8f);
			yield return new WaitForSeconds(completeSound.length);
			LevelManager.instance.NextLevel(nextLevelName);	
		}
	}
}
