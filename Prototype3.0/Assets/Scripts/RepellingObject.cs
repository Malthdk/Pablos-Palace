using UnityEngine;
using System.Collections;

public class RepellingObject : MonoBehaviour {

	public PipeTransport pipeTrans;
	Player playerScript;
	public Vector2 endForcePablo;
	public Vector2 endForceParticles;

	void Start () 
	{
		playerScript = (Player)FindObjectOfType(typeof(Player));
	}

	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			pipeTrans.movableObjects.Remove(other.gameObject);
			playerScript.velocity.x = endForcePablo.x;
			playerScript.velocity.y = endForcePablo.y;
		}
		else if (other.tag == "DynamicParticle")
		{
			pipeTrans.movableObjects.Remove(other.gameObject);
			Rigidbody2D rgb = other.GetComponent<Rigidbody2D>();
			rgb.AddForce(endForceParticles);
		}
		
	}
	
	void OnTriggerExit2D(Collider2D other)
	{

	}
}
