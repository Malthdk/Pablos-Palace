using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour {

	private Vector3 pos;
	public Transform player;

	float speed = 100f;
	void Start () 
	{
		
	}

	void Update ()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "particleSource")
		{
			other.GetComponent<ParticleGenerator>().enabled = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "particleSource")
		{
			other.GetComponent<ParticleGenerator>().enabled = false;
		}
	}
}
