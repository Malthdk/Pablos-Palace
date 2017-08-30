using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushRotate : MonoBehaviour {


	public int rotationOffset = 90;
	public Camera mainCamera;
	public float speed;
	public float maxDistance;
	CreateSplat createSplat;
	GameObject brushHead;

	void Start () 
	{
		brushHead = transform.GetChild(0).gameObject;
		createSplat = transform.GetComponentInChildren<CreateSplat>();
	}
	

	void Update () 
	{
		//For testing
		if (Input.GetButton("Fire1"))
		{
			speed = 0.008f;
			createSplat.isSplatting = true;
			Controller2D.instance.painting = true;
		}
		if (Input.GetButtonUp("Fire1"))
		{
			speed = 0.1f;
			createSplat.isSplatting = false;
			Controller2D.instance.painting = false;
		}

		Vector3 difference = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		difference.Normalize();

		float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
		//transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);  //Instant rotation
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotZ + rotationOffset), Time.time * speed);  //Lerping rotation


		//brushHead.transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0f,0f,-10f);

		Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
		Vector3 allowedPos = mousePos - transform.parent.transform.position;
		allowedPos = Vector3.ClampMagnitude(allowedPos, maxDistance);
		brushHead.transform.position = transform.parent.transform.position + allowedPos;
	}
}
