using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public bool bounce = false;
	public float bounceAmount = 20f;
	public float downDashBounceAmount = 38f;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public Player playerScript;
	[HideInInspector]
	public Abilities abilitiesScript;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<Player> ();
		abilitiesScript = player.GetComponent<Abilities> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (bounce == true) {
			if (abilitiesScript.isDownDashing) {
				abilitiesScript.downDashState = Abilities.DownDashState.Cooldown;
				playerScript.velocity.y = downDashBounceAmount;
			} else {	
				playerScript.velocity.y = bounceAmount;
			}
			bounce = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.name == "Player"){
			StartCoroutine("Squeeze");
			bounce = true;
		}
		if (other.tag == "DynamicParticle"){
			DynamicParticle particle = other.gameObject.GetComponent<DynamicParticle>();
			particle.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f,150f));
		}
	}

	public IEnumerator Squeeze() {
		Vector3 originalSize = transform.localScale;
		Vector3 scaleDown = originalSize;
		scaleDown.y = 0.5f;
		transform.localScale = scaleDown;
		yield return new WaitForSeconds(0.1f);
		transform.localScale = originalSize;
	}

}
