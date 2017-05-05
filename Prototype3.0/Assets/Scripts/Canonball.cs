using UnityEngine;
using System.Collections;

public class Canonball : MonoBehaviour {

	public SpriteRenderer spriterenderer;
	private CircleCollider2D cCollider;
	private ParticleSystem pSystem;
	private Rigidbody2D rb;

	// FOR SOUND
	public AudioClip collisionSound;
	private AudioSource source;

	void Awake () {
		spriterenderer = this.gameObject.GetComponent<SpriteRenderer>();
		cCollider = this.gameObject.GetComponent<CircleCollider2D>();
		pSystem = this.transform.GetComponentInChildren<ParticleSystem>();
		rb = this.gameObject.GetComponent<Rigidbody2D>();
		source = this.gameObject.GetComponent<AudioSource>();
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "CanvasTile" || other.name == "Player" || other.tag == "Obsticle" || other.tag == "Through" || other.tag == "killTag" || other.tag == "movingPlatform" || other.tag == "blackBox") {
			float vol = Random.Range(0.4f,0.8f);
			source.pitch =  Random.Range(0.7f,1.3f);
			source.PlayOneShot(collisionSound, vol);
			StartCoroutine(Destroy());
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

	public void OnObjectReuse () {
		gameObject.SetActive(true);
		rb.velocity = new Vector2(0f, 0f);
	}

	public IEnumerator Destroy()
	{
		rb.velocity = new Vector2(0f, 0f);
		spriterenderer.enabled = false;
		pSystem.Play();
		yield return new WaitForSeconds(0.4f);
		spriterenderer.enabled = true;
		gameObject.SetActive(false);
	}
}
