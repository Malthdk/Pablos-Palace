﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeTransport : MonoBehaviour {

	Transform endPoint;

	public AttractingObject attObjects;

	private bool entered;
	
	public float waitBeforeExit;
	
	public List<GameObject> movableObjects = new List<GameObject>();

	void Start () 
	{
		endPoint = transform.GetChild(1).GetComponent<Transform>();
	}

	IEnumerator Transport() 
	{
		for (int i = 0; i < movableObjects.Count; i++)
		{
			movableObjects[i].transform.position = endPoint.transform.position;
			movableObjects[i].SetActive(false);
		}
		
		yield return new WaitForSeconds(waitBeforeExit);
		for (int i = 0; i < movableObjects.Count; i++)
		{
			//AddForce to objects
			movableObjects[i].SetActive(true);
		}

		//player.SetActive(true);
	}

	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player" || other.tag == "DynamicParticle")
		{
			Debug.Log ("Should be sucked in now");

			movableObjects.Add(other.gameObject);
	
			attObjects.attractedGameobjects.Remove(other.transform);
			StartCoroutine(Transport());
		}
	}


	
}
