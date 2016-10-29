using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

	public float cooldown = 2f;
	public int particleNumber = 20;
	public float particleLifetime = 2f;
	public bool spawn = true;
	public float power = 4500f;
	public DynamicParticle.STATES states = DynamicParticle.STATES.BLUE;
	public bool constantVelocity;
	public bool canon;
	private Vector3 vel;
	public GameObject ball;

	public void Update() 
	{	
		if (spawn)
		{	
			StartCoroutine ("Fire");
			spawn = false;
		}

		vel = transform.right * 0.4f;
	}

	IEnumerator Fire () 
	{	
		ball = (GameObject) Instantiate(Resources.Load("Canonball", typeof(GameObject)));
		ball.gameObject.transform.position = this.gameObject.transform.position;
		ball.GetComponent<ParticleGeneratorStill> ().particlesState = states;
		ball.GetComponent<ParticleGeneratorStill> ().particleLifetime = particleLifetime;
		ball.GetComponent<ParticleGeneratorStill> ().particleNumber = particleNumber;

		if (!constantVelocity) {
			if (!canon) {
				ball.GetComponent<Rigidbody2D> ().AddForce (transform.right * power); //Add our custom force
			} else {
				ball.GetComponent<Rigidbody2D> ().AddForce (transform.up * power); //Add our custom force
			}
		} else {
			Destroy(ball.GetComponent<Rigidbody2D> ());
			vel = Vector3.zero;
			ball.transform.Translate(vel);
		}
		yield return new WaitForSeconds(cooldown);

		StartCoroutine ("Fire");
	}
}
