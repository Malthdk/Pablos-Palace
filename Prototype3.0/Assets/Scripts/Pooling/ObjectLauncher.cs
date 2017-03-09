using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour {

	public Vector3 LaunchVector;

	public void Launch(Object obj) {
		var body = (obj as GameObject).GetComponent<Rigidbody>();
		(obj as GameObject).SetActive(true);

		body.transform.position = transform.position;
		body.transform.rotation = transform.rotation;
		body.velocity = Vector3.zero;
		body.AddForce(LaunchVector, ForceMode.VelocityChange);
	}

}
