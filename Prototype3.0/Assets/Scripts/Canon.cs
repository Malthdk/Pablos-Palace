using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

	public float cooldown = 2f;
	public int particleNumber = 20;
	public float particleLifetime = 2f;
	public bool spawn = true;
	public Vector3 force;
	public DynamicParticle.STATES states = DynamicParticle.STATES.BLUE;

	public bool freezeOnY;

	public GameObject ball;

	public void Update() 
	{	
		if (spawn)
		{	
			StartCoroutine ("Fire");
			spawn = false;
		}
	}

	IEnumerator Fire () 
	{	
		ball = (GameObject) Instantiate(Resources.Load("Canonball", typeof(GameObject)));
		ball.gameObject.transform.position = this.gameObject.transform.position;
		ball.GetComponent<Rigidbody2D> ().AddForce (force); //Add our custom force
		ball.GetComponent<ParticleGeneratorStill> ().particlesState = states;
		ball.GetComponent<ParticleGeneratorStill> ().particleLifetime = particleLifetime;
		ball.GetComponent<ParticleGeneratorStill> ().particleNumber = particleNumber;

		if (freezeOnY) 
		{
			//FOR FREEZING BALLS ON Y AXIS, FOR EASY CONTROL - AND SLOW MOVING FLOATING BALLS ***DOESNT WORK***
			/*
			RigidbodyConstraints2D rgb2DConstraints;
			Rigidbody2D rgb2D;

			rgb2D = ball.GetComponent<Rigidbody2D>();
			rgb2D.constraints = rgb2DConstraints.FreezePositionY;
			*/
		}

		yield return new WaitForSeconds(cooldown);

		StartCoroutine ("Fire");

	}
}
