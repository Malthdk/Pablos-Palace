using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrolling : MonoBehaviour {

	public Vector3 target;
	public float speed;

	private Vector3 goTo;

	private Vector3 dir;
	private Rigidbody2D rgb;

	void Awake()
	{
		goTo = transform.position + target;
		rgb = GetComponent<Rigidbody2D>();	
	}

	void Start () 
	{
		dir = (goTo - transform.position).normalized * speed;
	}
	

	void Update () 
	{
		rgb.velocity = dir;
	}
}
