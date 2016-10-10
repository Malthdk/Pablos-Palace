using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	private float targetCameraSize;
	float t = 0;
	float time = 0;
	public bool isAZoomingZone;
	public float newSize = 25;
	[Range(0, 2)]
	public float zoomSpeed = 1f;
	private float duration = 3f;

	public bool isAnOffsettingCameraZone;
	public float xOffset = 0f;
	public float yOffset = 9f;
	[Range(0,1)]
	public float offsetSpeed;
	private bool enterZooming = false, exitZooming = false, lockCamera = false, enterOffset = false, exitOffset = false;

	public bool isALockingCameraZone;
	[Range(0, 1)]
	public float CameraMoveSpeed = 1f;

	public Vector3 targetPos;
	private Vector3 targetPosOffline;

	private Camera mainCamera;
	CameraFollow cameraPos;

	Camera[] cameras;

	float newCameraY;
	float newCameraX;
	float iniCameraPosY;
	float iniCameraPosX;

	void Start () 
	{
		exitZooming = false; 

		///mainCamera = Camera.main;
		mainCamera = GameObject.FindGameObjectWithTag("playerCamera").GetComponent<Camera>();
		targetCameraSize = mainCamera.orthographicSize;

		cameraPos = mainCamera.gameObject.GetComponent<CameraFollow> ();
		newCameraY = cameraPos.verticalOffset + yOffset;
		newCameraX = cameraPos.horizontalOffset + xOffset;
		iniCameraPosY = cameraPos.verticalOffset;
		iniCameraPosX = cameraPos.horizontalOffset;

		targetPosOffline = targetPos;

		cameras = mainCamera.transform.GetComponentsInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isAZoomingZone)
		{
			if (enterZooming)
			{
				time += Time.deltaTime;
				t = time / duration;
				for (int i = 0; i < cameras.Length; i++)
				{
					cameras[i].orthographicSize = Mathf.SmoothStep(mainCamera.orthographicSize, newSize, t * zoomSpeed);
					if (cameras[i].orthographicSize == newSize)
					{
						enterZooming = false;
					}
				}
			}
			else if (exitZooming)
			{
				time += Time.deltaTime;
				t = time / duration;

				for (int i = 0; i < cameras.Length; i++)
				{
					cameras[i].orthographicSize = Mathf.SmoothStep(mainCamera.orthographicSize, targetCameraSize, t * zoomSpeed);
				}
			}
		}

		if (isAnOffsettingCameraZone)
		{
			if (enterOffset)
			{
				time += Time.deltaTime;
				t = time / duration;
				cameraPos.verticalOffset = Mathf.SmoothStep(cameraPos.verticalOffset, newCameraY, t * offsetSpeed);
				cameraPos.horizontalOffset = Mathf.SmoothStep(cameraPos.horizontalOffset, newCameraX, t * offsetSpeed);

				if (cameraPos.verticalOffset == newCameraY && cameraPos.horizontalOffset == newCameraX)
				{
					enterOffset = false;
				}
			}
			else if (exitOffset)
			{
				time += Time.deltaTime;
				t = time / duration;
				cameraPos.verticalOffset = Mathf.SmoothStep(cameraPos.verticalOffset, iniCameraPosY, t* offsetSpeed);
				cameraPos.horizontalOffset = Mathf.SmoothStep(cameraPos.horizontalOffset, iniCameraPosX, t* offsetSpeed);
			}
		}
		if (isALockingCameraZone)
		{
			if (lockCamera)
			{
				time += Time.deltaTime;
				t = time / duration;

				cameraPos.enabled = false;
				Vector3 pos = mainCamera.transform.position;
				pos.x = Mathf.SmoothStep(pos.x, transform.position.x + targetPos.x + cameraPos.horizontalOffset, t * CameraMoveSpeed);
				pos.y = Mathf.SmoothStep(pos.y, transform.position.y + targetPos.y + cameraPos.verticalOffset, t * CameraMoveSpeed);
				mainCamera.transform.position = pos;

			}
			if (!lockCamera)
			{
				cameraPos.enabled = true;
			}
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
		lockCamera = true;
		enterOffset = true;
		exitOffset = false;
		t = 0;
		time = 0;
	}

	void ZoomExit()
	{
		enterZooming = false;
		exitZooming = true;
		lockCamera = false;
		exitOffset = true;
		enterOffset = false;
		if (isALockingCameraZone)
		{
			//cameraPos.findingTarget = true;
		}
		t = 0;
		time = 0;

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		float size = .3f;
		if(isALockingCameraZone)
		{
			Vector3 globalWaypointPos = (Application.isPlaying)?targetPosOffline + transform.position:targetPos + transform.position;
			Gizmos.DrawLine (globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
			Gizmos.DrawLine (globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			Gizmos.DrawWireCube(transform.position + targetPos, new Vector3(newSize*2*mainCamera.aspect,newSize*2, 2f));

		}
	}
		
}
