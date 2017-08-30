using UnityEngine;
using System.Collections;

public class SecretArea : MonoBehaviour {

	public SpriteRenderer spriteRend;
	private Color color;
	private bool revealing;

	void Start () 
	{
		spriteRend = GetComponentInChildren<SpriteRenderer>();
		color = spriteRend.material.color;
	}
	

	void Update () 
	{
		if (revealing)
		{
			Reveal();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			revealing = true;
		}
	}

	void Reveal()
	{
		Debug.Log("Start fading");
		color.a -= 0.05f;
		spriteRend.material.color = color;
	}
}
