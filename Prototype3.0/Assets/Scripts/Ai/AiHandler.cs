using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHandler : MonoBehaviour {

	public AiBehaviour behaviour;

	AiPatrolling patrollingScript;
	HeatSeeking heatSeekingScript;

	public enum AiBehaviour
	{
		Patrol,
		Agro,
		Chase
	}

	void Start () 
	{
		if (gameObject.GetComponent<AiPatrolling>() == null || gameObject.GetComponent<HeatSeeking>() == null)
		{
			return;
		}
		else
		{
			patrollingScript = gameObject.GetComponent<AiPatrolling>();
			heatSeekingScript = gameObject.GetComponent<HeatSeeking>();
		}
	}
	

	void Update () 
	{
		switch(behaviour)
		{
		case AiBehaviour.Patrol:
			patrollingScript.isPatrolling = true;
			break;

		case AiBehaviour.Agro:
			patrollingScript.isPatrolling = false;
			Debug.Log("Agroed!");
			behaviour = AiBehaviour.Chase;
			break;

		case AiBehaviour.Chase:
			patrollingScript.isPatrolling = false;
			heatSeekingScript.isSeeking = true;
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(heatSeekingScript.isSeeking == true)
		{
			//If it collides with walls - Destroy it.
		}
	}

	//Enemy1 - small - fast - dies on paint and walls
	//Enemy2 - large - slow - only dies on walls
}
