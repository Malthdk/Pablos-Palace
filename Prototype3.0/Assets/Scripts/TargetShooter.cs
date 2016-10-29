using UnityEngine;
using System.Collections;

public class TargetShooter : MonoBehaviour {

	public float speed = 5f;
	public Transform barrel;
	public Transform target;

	// Use this for initialization
	void Start () {
		barrel = this.transform.GetChild(0);
		target = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vectorToTarget = target.position - barrel.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		barrel.rotation = Quaternion.Lerp(barrel.rotation, q, Time.deltaTime * speed);
	}
}
