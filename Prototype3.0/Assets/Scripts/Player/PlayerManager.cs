using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public AudioClip killSoundClip;

	[HideInInspector]
	public static PlayerManager _instance;

	[HideInInspector]
	public AudioSource killSound;

	public static PlayerManager instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<PlayerManager>();
			}
			return _instance;
		}
	}
		
	void Start () 
	{
		killSound = gameObject.transform.GetChild(10).GetComponent<AudioSource>();
	}

	void Update () 
	{
		
	}

	public void KillPlayer()
	{
		killSound.PlayOneShot(killSoundClip, 0.8f);
		LevelManager.lManager.Respawn();
	}
}
