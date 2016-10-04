using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractingObject : MonoBehaviour {

	public bool attracting;
	GameObject player;
	
	public List<Transform> attractedGameobjects = new List<Transform>();

	void Start () 
	{
		player = GameObject.Find("Player");
	}
	

	void Update () 
	{
		if (attracting)
		{
			for (int i = 0; i < attractedGameobjects.Count; i++)
			{
				attractedGameobjects[i].position = Vector3.Lerp(attractedGameobjects[i].position, gameObject.transform.position, 1.1f*Time.deltaTime);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player" || other.tag == "DynamicParticle")
		{
			attractedGameobjects.Add(other.transform);
			attracting = true;
		}

	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		attractedGameobjects.Remove(other.transform);
	}

}
