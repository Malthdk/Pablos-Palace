using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	
	public float targetCameraSize;

	float t = 0;
	float time = 0;
	public float newSize = 25;
	public float zoomSpeed = 1f;
	public float duration = 3f;

	public float xOffset = 0f;
	public float yOffset = 9f;
	public bool isMoovingCamera;

	public bool enterZooming = false;
	public bool exitZooming = false;

	private Camera mainCamera;

	CameraFollow cameraPos;

	float newCamera;
	float iniCameraPos;

	void Start () 
	{
		exitZooming = false; 

		mainCamera = Camera.main;
		targetCameraSize = Camera.main.orthographicSize;

		cameraPos = Camera.main.GetComponent<CameraFollow> ();
		newCamera = cameraPos.verticalOffset - yOffset;
		iniCameraPos = cameraPos.verticalOffset;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (enterZooming)
		{
			time += Time.deltaTime;
			t = time / duration;
			mainCamera.orthographicSize = Mathf.SmoothStep(mainCamera.orthographicSize, newSize, t * zoomSpeed);

			cameraPos.verticalOffset = Mathf.SmoothStep(cameraPos.verticalOffset, newCamera, t* zoomSpeed);

			if (mainCamera.orthographicSize == newSize)
			{
				enterZooming = false;
			}
			if (cameraPos.verticalOffset == newCamera && cameraPos.horizontalOffset == newCamera)
			{
				enterZooming = false;
			}
		}
		else if (exitZooming)
		{
			time += Time.deltaTime;
			t = time / duration;
			mainCamera.orthographicSize = Mathf.SmoothStep(mainCamera.orthographicSize, targetCameraSize, t * zoomSpeed);

			cameraPos.verticalOffset = Mathf.SmoothStep(cameraPos.verticalOffset, iniCameraPos, t* zoomSpeed);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			Debug.Log("Zooming!");
			ZoomEnter ();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			Debug.Log("Going back to original camera size!");
			ZoomExit();
		}
	}

	void ZoomEnter()
	{
		enterZooming = true;
		exitZooming = false;
		t = 0;
		time = 0;
	}

	void ZoomExit()
	{
		enterZooming = false;
		exitZooming = true;
		t = 0;
		time = 0;

	}
		
}
