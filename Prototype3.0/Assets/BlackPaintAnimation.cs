using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPaintAnimation : MonoBehaviour {

	public float rotateRate = 1.0f;
	public float invokeRate = 1.0f;

	float xRot;
	float yRot;
	float zRot;
	float wRot;

	void Start () 
	{
		InvokeRepeating("newRotation", 0.0f, invokeRate);	
	}
	

	void Update () 
	{
		Quaternion RandomQuat = new Quaternion(xRot, yRot, zRot, wRot);
		transform.rotation = Quaternion.Slerp(transform.rotation, RandomQuat, rotateRate * Time.deltaTime);
	}

	void newRotation ()
	{
		xRot = Random.Range(-0, 0);
		yRot = Random.Range(-0, 0);
		zRot = Random.Range(-45, 45);
		wRot = Random.Range(-45, 45);
	}
}
