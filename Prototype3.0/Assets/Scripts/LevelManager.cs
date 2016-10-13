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

	public GameObject[] stateObjects;

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

		stateObjects = FindGameObjectsWithTags(new string[]{"dissPlatform", "orangeDestroy"});
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
		ResetStates(stateObjects);

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

	void ResetStates(GameObject[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
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
