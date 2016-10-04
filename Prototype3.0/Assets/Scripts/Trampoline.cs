using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {

	public bool bounce = false;
	public float bounceAmount = 38f;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public Player playerScript;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (bounce == true) {
			playerScript.velocity.y = bounceAmount;
			bounce = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.name == "Player"){
			StartCoroutine("Squeeze");
			bounce = true;
		}
		if (other.tag == "DynamicParticle"){
			//StartCoroutine("Squeeze");
			//bounce = true;
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
