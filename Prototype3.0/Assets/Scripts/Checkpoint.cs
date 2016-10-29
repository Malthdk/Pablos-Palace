using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {

	//public string tempTag;
	private Player player;

	public List<GameObject> objectsToRemove;
	public List<PlatformController> platformsToRemove;
	public List<Lever> leversToRemove;

	void Start () 
	{
		player = FindObjectOfType<Player>();
	}

	void Update()
	{
		if (player == null)
		{
			player = FindObjectOfType<Player>();
		}
	}
		
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
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
				
		}
	}

}
 