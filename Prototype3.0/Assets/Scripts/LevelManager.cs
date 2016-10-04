using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static LevelManager lManager;

	public GameObject currentCheckpoint;
	//public GameObject spawnPoint;
	public string currentTag;

	public Player player;
	private Checkpoint check;

	public string MyLevel;

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

	void Start () {
		player = FindObjectOfType<Player>();
	}
	void Update()
	{
		//spawnPoint = GameObject.FindGameObjectWithTag("spawnpoint");

		if(player == null)
		{
			player = FindObjectOfType<Player>();
		}
	}

	public void Respawn()
	{
		Debug.Log ("Respawned!");

		player.transform.position = currentCheckpoint.transform.position;
		Application.LoadLevel(Application.loadedLevel);
		player.tag = currentTag;
	}

	public void NextLevel()
	{
		Destroy(player.gameObject);
		Destroy (this.gameObject);
		Application.LoadLevel(MyLevel);
		//player.transform.position = spawnPoint.transform.position;
	}
}
