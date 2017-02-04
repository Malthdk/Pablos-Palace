using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleTile : MonoBehaviour {

	public Destroy destroy;


	private float destroyFraction = 0.035f;
	private bool destroying;
	private GameObject particles;
	private Color color;
	private SpriteRenderer spriteRend;
	private BoxCollider2D boxCol;

	void Start () 
	{
		particles = this.transform.GetChild(0).gameObject;
		particles.SetActive(false);
		spriteRend = gameObject.GetComponent<SpriteRenderer>();
		color = spriteRend.material.color;
		color.a = 1;
		spriteRend.material.color = color;
	
	}
	
	public enum Destroy
	{
		Awake,
		Crumbling,
		Destroyed
	}


	void Update () 
	{

		switch (destroy)
		{
		case Destroy.Awake:

			break;
		case Destroy.Crumbling:

			particles.SetActive(true);
			Crumble();
			if (color.a <= 0)
			{
				destroy = Destroy.Destroyed;
			}
			break;
		case Destroy.Destroyed:
			gameObject.SetActive(false);
			break;
		}
	}


	void Crumble()
	{
		color.a -= destroyFraction;
		spriteRend.material.color = color;

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			destroy = Destroy.Crumbling;
		}	
	}

}
