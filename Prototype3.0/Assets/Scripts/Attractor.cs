using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {

	Player player;

	void Start () 
	{
		player = GameObject.Find("Player").GetComponent<Player>();	
	}
	

	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			/*
			 * DRAW THE PLAYER SLOWLY TOWARDS THIS OBJECT
			 * Should be used both for the pipes that suck in the player/particles and spit them out. And for the magnets that can transport Pablo
			 */
		}
			
	}
}
