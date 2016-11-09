using UnityEngine;
using System.Collections;

public class TargetShooter : MonoBehaviour {

	public float speed = 5f;
	public Transform barrel;
	public Transform target;
	public Canon canonScript;
	public SpriteRenderer sr;

	private bool searchdestroy;
	private bool resetpos;
	private Vector3 barrelDefaultPosition;

	// Use this for initialization
	void Start () {
		barrel = this.transform.GetChild(1);
		target = GameObject.Find("Player").transform;
		barrelDefaultPosition = barrel.transform.position;
		canonScript = GetComponentInChildren<Canon>();
		sr = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (searchdestroy) {
			SearchAndDestroy();
		} else if (resetpos) {
			ResetPosition();
		}
	}

	void SearchAndDestroy() {
		Vector3 vectorToTarget = target.position - barrel.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		barrel.rotation = Quaternion.Lerp(barrel.rotation, q, Time.deltaTime * speed);
	}

	void ResetPosition() {
		Vector3 vectorToTarget = barrelDefaultPosition - barrel.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		barrel.rotation = Quaternion.Lerp(barrel.rotation, q, Time.deltaTime * speed);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.name == "Player") {
			searchdestroy = true;
			canonScript.stopShooting = false;
			canonScript.spawn = true;
			sr.color = new Color(0.1f,0.1f,0.1f);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.name == "Player") {
			searchdestroy = false;
			resetpos = true;
			canonScript.stopShooting = true;
			sr.color = new Color(1f,1f,1f);
		}
	}
}
