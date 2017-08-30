using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

	public float cooldown = 2f;
	public bool spawn = true;
	public bool stopShooting = false;
	public float power = 4500f;
	public bool constantVelocity;
	public bool canon;
	private Vector3 vel;
	public GameObject ball;

	void Start() {
		ball = PoolManager.instance.canonballPrefab;
	}

	public void Update() 
	{	
		if (spawn)
		{	
			StartCoroutine ("Fire");
			spawn = false;
		}
		if (stopShooting) 
		{
			StopCoroutine ("Fire");
		}
		vel = transform.right * 0.4f;
	}

	IEnumerator Fire () 
	{	
		yield return new WaitForSeconds(cooldown);
		PoolManager.instance.ReuseCanonball (transform, ball, transform.position, power, constantVelocity);

		/*if (!constantVelocity) {
			if (!canon) {
				ball.GetComponent<Rigidbody2D> ().AddForce (transform.right * power); //Add our custom force
			} else {
				ball.GetComponent<Rigidbody2D> ().AddForce (transform.up * power); //Add our custom force
			}
		} else {
			ball.GetComponent<Rigidbody2D> ().gravityScale = 0f;
			ball.GetComponent<Rigidbody2D> ().AddRelativeForce (transform.right * power);
		}*/
		StartCoroutine ("Fire");
	}
}
