using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFireBall : MonoBehaviour {

	public ParticleGeneratorStill particlegenerator;
	public LayerMask collisionlayer;

	void Start () 
	{
		particlegenerator = this.gameObject.GetComponent<ParticleGeneratorStill>();
	}
	

	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.name == "Player")
		{
			PlayerManager.pManager.KillPlayer();
		}
		else if ((collisionlayer.value & 1<<other.gameObject.layer) != 0)
		{
			particlegenerator.spawn = true;
		}
	}
}
