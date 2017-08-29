using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour {

	private Vector3 targetScale = new Vector3(3.69f,3.69f,0f);
	public bool changingScene;
	public float lerpTime;
	public Camera camera;
	void Start () 
	{
		
	}
	

	void Update () 
	{
		if (changingScene)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpTime);

			Vector3 endPos = camera.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, camera.nearClipPlane));
			transform.position = Vector3.Lerp(transform.position, endPos, lerpTime);
		}	
	}


}
