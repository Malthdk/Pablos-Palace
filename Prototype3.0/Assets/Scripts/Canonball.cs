using UnityEngine;
using System.Collections;

public class Canonball : MonoBehaviour {

	public ParticleGeneratorStill particlegenerator;
	public SpriteRenderer spriterenderer;
	private CircleCollider2D cCollider;
	private ParticleSystem pSystem;
	private Rigidbody2D rgb;

	// FOR SOUND
	public AudioClip collisionSound;
	private AudioSource source;

	void Start () {
		particlegenerator = this.gameObject.GetComponent<ParticleGeneratorStill>();
		spriterenderer = this.gameObject.GetComponent<SpriteRenderer>();
		cCollider = this.gameObject.GetComponent<CircleCollider2D>();
		pSystem = this.transform.GetComponentInChildren<ParticleSystem>();
		rgb = this.gameObject.GetComponent<Rigidbody2D>();
		source = this.gameObject.GetComponent<AudioSource>();
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "CanvasTile" || other.name == "Player" || other.tag == "Obsticle" || other.tag == "Through" || other.tag == "killTag" || other.tag == "movingPlatform") {
			float vol = Random.Range(0.4f,0.6f);
			source.pitch =  Random.Range(0.7f,1.3f);
			source.PlayOneShot(collisionSound, vol);
			StartCoroutine(Destroy());
		}
		if (other.tag == "blackBox") {
			Destroy(gameObject);
		}
	}

	/*
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.name == "CanvasTile" || other.collider.name == "Player" || other.collider.tag == "Through" || other.collider.tag == "killTag" || other.collider.tag == "movingPlatform") {
			StartCoroutine(Destroy());
			Debug.Log("YOu hit something");
		}
	}*/

	public IEnumerator Destroy()
	{
		rgb.velocity = new Vector2(0f, 0f);
		spriterenderer.enabled = false;
		pSystem.Play();
		yield return new WaitForEndOfFrame();
		cCollider.enabled = false;
	}
}
