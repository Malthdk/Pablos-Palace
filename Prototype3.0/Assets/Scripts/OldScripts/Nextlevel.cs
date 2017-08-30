using UnityEngine;
using System.Collections;

public class Nextlevel : MonoBehaviour {

	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
//			LevelManager.lManager.NextLevel();
		}
	}
}