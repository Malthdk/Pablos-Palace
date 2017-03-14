using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	SpriteRenderer myRenderer;
	private bool isOpen = false;

	void Start () 
	{
		myRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	

	void Update () 
	{
		if (LevelManager.instance.numberOrbs == 0)
		{
			myRenderer.color = Color.green;
			isOpen = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (isOpen)
		{
			LevelManager.instance.NextLevel();
		}
	}
}
