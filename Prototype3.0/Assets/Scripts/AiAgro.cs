using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAgro : MonoBehaviour {

	AiHandler handler;

	void Start () 
	{
		handler = transform.parent.GetComponent<AiHandler>();
	}
	

	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			handler.behaviour = AiHandler.AiBehaviour.Agro;
			gameObject.SetActive(false);
		}
	}


}
