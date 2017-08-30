﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {

	//public string tempTag;
	private Player player;

	public List<GameObject> objectsToRemove;
	public List<PlatformController> platformsToRemove;
	public List<Lever> leversToRemove;
	public List<FallingPlatform> fallingToRemove;

	private GameObject graphics;
	private CircleCollider2D boxCol;

	void Start () 
	{
		graphics = this.gameObject.transform.GetChild(0).gameObject;
		graphics.SetActive(false);
		player = FindObjectOfType<Player>();
		boxCol = gameObject.GetComponent<CircleCollider2D>();
	}

	void Update()
	{
		if (player == null)
		{
			player = FindObjectOfType<Player>();
		}
		if (LevelManager.instance.currentCheckpoint != this.gameObject)
		{
			graphics.SetActive(false);
		}
	}
		
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			boxCol.enabled = false;
			graphics.SetActive(true);
			Debug.Log ("Hit checkpoint");

			LevelManager.instance.currentCheckpoint = gameObject;
			LevelManager.instance.currentTag = player.tag;

			foreach(GameObject stateObj in objectsToRemove) 
			{
				LevelManager.instance.stateObjects.Remove(stateObj);
			}
			foreach(PlatformController platformObj in platformsToRemove) 
			{
				LevelManager.instance.platforms.Remove(platformObj);
			}
			foreach(Lever leverObj in leversToRemove) 
			{
				LevelManager.instance.levers.Remove(leverObj);
			}
			foreach(FallingPlatform fallObj in fallingToRemove) 
			{
				LevelManager.instance.fallingPlatforms.Remove(fallObj);
			}
				
		}
	}

}
 