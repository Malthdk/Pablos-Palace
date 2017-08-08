using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeeking : MonoBehaviour {

	public bool isSeeking;
	public float speed = 5f;
	public float rotatingSpeed = 200f;
	public GameObject target;

	Rigidbody2D rb;

	void Start () 
	{
		target = GameObject.Find("Player");
		rb = GetComponent<Rigidbody2D>();
	}
	

	void FixedUpdate () 
	{
		if(isSeeking)
		{
			Vector2 point2Target = (Vector2)transform.position - (Vector2)target.transform.position;

			point2Target.Normalize();

			float value = Vector3.Cross(point2Target, transform.right).z;

			rb.angularVelocity = rotatingSpeed * value;

			rb.velocity = transform.right * speed;
		}
	}
}
