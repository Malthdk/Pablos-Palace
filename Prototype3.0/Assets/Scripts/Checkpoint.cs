using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	//public string tempTag;
	private Player player;
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
		}
	}

}
 