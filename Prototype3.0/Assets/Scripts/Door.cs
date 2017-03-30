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
			if (isOpen)
			{
				LevelManager.instance.NextLevel();
			}
		}
	}
}
