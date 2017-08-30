using UnityEngine;
using System.Collections;

public class BlackWaterShooter : MonoBehaviour {


	public GameObject bullet;
	public ParticleSystem bubbles;

	public float shootForceY;
	public float shootForceX;
	public float shootTime;
	public float bubbleTime;
	public float startDelay;
	public float shootdelay;
	private Vector3 startPos;

	Rigidbody2D rgb;

	void Awake()
	{
		startPos = bullet.transform.position;
	}

	void Start () 
	{
		StartCoroutine(Shoot(startDelay));
	}


	void Update () 
	{
	
	}

	IEnumerator Shoot(float delay)
	{
		bubbles.Stop();
		bullet.SetActive(false);

		yield return new WaitForSeconds(delay);

		bubbles.Play();

		yield return new WaitForSeconds(bubbleTime);

		bullet.SetActive(true);
		rgb = bullet.GetComponent<Rigidbody2D>();
		rgb.AddForce(new Vector2(shootForceX, shootForceY), ForceMode2D.Impulse);
		bubbles.Stop();

		yield return new WaitForSeconds(shootTime);

		bullet.transform.position = startPos;
		StartCoroutine(Shoot(shootdelay));

	}
}
