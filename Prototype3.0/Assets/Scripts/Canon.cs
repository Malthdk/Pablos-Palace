using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

	public float cooldown = 2f;
	public int particleNumber = 20;
	public float particleLifetime = 2f;
	public bool spawn = true;
	/// <summary>
	/// The y velocity of the canon.
	/// </summary>
	public float power = 4500f;
	public DynamicParticle.STATES states = DynamicParticle.STATES.BLUE;

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
		ball.GetComponent<Rigidbody2D> ().AddForce (transform.up * power); //Add our custom force
		ball.GetComponent<ParticleGeneratorStill> ().particlesState = states;
		ball.GetComponent<ParticleGeneratorStill> ().particleLifetime = particleLifetime;
		ball.GetComponent<ParticleGeneratorStill> ().particleNumber = particleNumber;


		yield return new WaitForSeconds(cooldown);

		StartCoroutine ("Fire");

	}
}
