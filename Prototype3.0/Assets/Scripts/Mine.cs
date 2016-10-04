using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	public float cooldown = 2f;
	public ParticleGenerator particlegenerator;
	public SpriteRenderer spriterenderer;

	private float tempTime = 0f;

	void Start () {
		particlegenerator = this.gameObject.GetComponent<ParticleGenerator>();
		spriterenderer = this.gameObject.GetComponent<SpriteRenderer>();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			tempTime += Time.deltaTime;
			spriterenderer.color = new Color(1f,0f,0f);
			if (tempTime > cooldown) {
				particlegenerator.spawn = true;
				tempTime = 0f;
				spriterenderer.color = new Color(0f,0f,0f);
			}
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			tempTime = 0f;
			spriterenderer.color = new Color(0f,0f,0f);
		} 
	}
}
