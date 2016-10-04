using UnityEngine;
using System.Collections;

public class Canonball : MonoBehaviour {

	public ParticleGeneratorStill particlegenerator;
	public SpriteRenderer spriterenderer;

	void Start () {
		particlegenerator = this.gameObject.GetComponent<ParticleGeneratorStill>();
		spriterenderer = this.gameObject.GetComponent<SpriteRenderer>();
	}


	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.name == "Standard" || other.collider.name == "Player" || other.collider.tag == "Obsticle") {
			particlegenerator.spawn = true;
		}
		if (other.collider.tag == "blackBox") {
			Destroy(gameObject);
		}
	}
}
