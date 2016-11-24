using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq ;

public class LevelManager : MonoBehaviour {

	public static LevelManager lManager;

	public GameObject currentCheckpoint;
	//public GameObject spawnPoint;
	public string currentTag;

	public float respawnTime = 0.5f;
	public Player player;
	private Checkpoint check;

	public string MyLevel;

	//public GameObject[] stateObjects;

	public List<GameObject> stateObjects;
	public List<PlatformController> platforms;
	public List<Lever> levers;
	public List<FallingPlatform> fallingPlatforms;

	[HideInInspector]
	public static LevelManager _instance;

	public static LevelManager instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<LevelManager>();
			}
			return _instance;
		}
	}

	void Awake()
	{
		if(lManager == null)
		{
			DontDestroyOnLoad(gameObject);
			lManager = this;
		}
		else if(lManager != this)
		{
			Destroy(gameObject);
		}
	}

	void Start () 
	{
		player = FindObjectOfType<Player>();

		foreach(GameObject sObject in FindGameObjectsWithTags(new string[]{"dissPlatform", "orangeDestroy", "coin"})) 
		{
			stateObjects.Add(sObject);
		}

		foreach(GameObject dObject in GameObject.FindGameObjectsWithTag("dissPlatform")) 
		{
			FallingPlatform fPlatform = dObject.GetComponent<FallingPlatform>();
			fallingPlatforms.Add(fPlatform);
		}

		foreach(GameObject pObject in GameObject.FindGameObjectsWithTag("movingPlatform")) 
		{
			PlatformController pController = pObject.GetComponent<PlatformController>();
			platforms.Add(pController);
		}

		foreach(GameObject lObject in GameObject.FindGameObjectsWithTag("Lever")) 
		{
			Lever lever = lObject.GetComponent<Lever>();
			levers.Add(lever);
		}
	}
	void Update()
	{
		//spawnPoint = GameObject.FindGameObjectWithTag("spawnpoint");

		if (player == null)
		{
			player = FindObjectOfType<Player>();
		}
	}
	IEnumerator Respawned() 
	{
		GameObject graphics = player.gameObject.transform.GetChild(0).gameObject;
		GameObject particleEffect = player.gameObject.transform.GetChild(1).GetChild(0).gameObject;
		player.enabled = false;
		graphics.SetActive(false);
		particleEffect.SetActive(true);

		yield return new WaitForSeconds(0.7f);

		particleEffect.SetActive(false);
		player.transform.position = currentCheckpoint.transform.position;
		player.tag = currentTag;
		player.velocity.x = 0f;
		player.velocity.y = 0f;

		ResetFallingPlatforms(fallingPlatforms);
		ResetStates(stateObjects);
		ResetPlatforms(platforms);
		ResetLevers(levers);
		ResetParticles();

		yield return new WaitForSeconds(respawnTime);

		player.enabled = true;
		graphics.SetActive(true);
		Debug.Log ("Respawned!");
	}
	public void Respawn()
	{
		StartCoroutine(Respawned());
		//Application.LoadLevel(Application.loadedLevel);
	}

	public void NextLevel()
	{
		Destroy(player.gameObject);
		Destroy (this.gameObject);
		Application.LoadLevel(MyLevel);
		//player.transform.position = spawnPoint.transform.position;
	}

	void ResetStates(List<GameObject> theList)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			theList[i].SetActive(true);
		}
	}

	void ResetFallingPlatforms(List<FallingPlatform> theList)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			theList[i].Reset();
		}
	}

	void ResetPlatforms(List<PlatformController> theList)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			theList[i].ResetPlatform();
		}
	}

	void ResetLevers(List<Lever> theList)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			theList[i].ResetLever();
		}
	}

	void ResetParticles()
	{
		foreach(GameObject particle in GameObject.FindGameObjectsWithTag("DynamicParticle")) {

			DynamicParticle dp = particle.GetComponent<DynamicParticle>();
			dp.Destroy();
		}
	}

	GameObject[] FindGameObjectsWithTags(params string[] tags)
	{
		var all = new List<GameObject>() ;

		foreach(string tag in tags)
		{
			var temp = GameObject.FindGameObjectsWithTag(tag).ToList() ;
			all = all.Concat(temp).ToList() ;
		}

		return all.ToArray() ;
	}
}
