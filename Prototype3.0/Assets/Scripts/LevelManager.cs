using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static LevelManager lManager;

	public GameObject currentCheckpoint;
	public string currentTag;

	public float respawnTime = 0.5f;
	public Player player;
	private Checkpoint check;

	public int coinCount;

	public List<GameObject> stateObjects;
	public List<PlatformController> platforms;
	public List<Lever> levers;
	public List<FallingPlatform> fallingPlatforms;
	public List<PickUpGlobe> orbs;
	public List<PickUpSecret> secrets;

	public int numberOrbs;

	private Scene scene;

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
		scene = SceneManager.GetActiveScene();
		if (GameObject.Find("Music") == null && GameObject.Find("Music(Clone)") == null) {
			Debug.Log("StartMusic");
			GameObject instance = (GameObject)Instantiate(Resources.Load("Music")); // Instantiates music if none is found
		}
		player = FindObjectOfType<Player>();
		FillLists();
	}

	void Update()
	{
		//spawnPoint = GameObject.FindGameObjectWithTag("spawnpoint");

		if (player == null)
		{
			player = FindObjectOfType<Player>();
		}

		/*if (SceneManager.GetActiveScene() != scene) {
			Debug.Log("Scene changed");
			FillLists();
			scene = SceneManager.GetActiveScene();
		}*/
	}

	//Handles all player respawning
	IEnumerator Respawned() 
	{
		GameObject graphics = player.gameObject.transform.GetChild(0).gameObject;
		GameObject particleEffect = player.gameObject.transform.GetChild(1).GetChild(0).gameObject;
		BoxCollider2D boxCol = player.gameObject.GetComponent<BoxCollider2D>();
		player.enabled = false;
		graphics.SetActive(false);
		particleEffect.SetActive(true);
		//Abilities.instance.Reset();

		yield return new WaitForSeconds(0.9f);

		particleEffect.SetActive(false);
		player.transform.position = currentCheckpoint.transform.position;
		player.tag = currentTag;
		StartCoroutine(	ColorStates.instance.ChangeColor(Color.white, 1f));
		player.velocity.x = 0f;
		player.velocity.y = 0f;

		ResetFallingPlatforms(fallingPlatforms);
		ResetStates(stateObjects);
		ResetPlatforms(platforms);
		ResetLevers(levers);
		ResetParticles();
		ResetOrbs(orbs);
		ResetSeOrbs(secrets);
		numberOrbs = orbs.Count;
		yield return new WaitForSeconds(respawnTime);

		player.enabled = true;
		graphics.SetActive(true);
		boxCol.enabled = true;
		Debug.Log ("Respawned!");
	}
	public void Respawn()
	{
		StartCoroutine(Respawned());
	}

	public void NextLevel(string myLevel)
	{
		Destroy(player.gameObject);
		Destroy (this.gameObject);
		Application.LoadLevel(myLevel);
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

	void ResetOrbs(List<PickUpGlobe> theList)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			theList[i].ResetOrb();
		}
	}
	void ResetSeOrbs(List<PickUpSecret> theList)
	{
		for (int i = 0; i < theList.Count; i++)
		{
			theList[i].ResetSeOrb();
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

	void FillLists() {
		foreach(GameObject oObject in GameObject.FindGameObjectsWithTag("orb")) 
		{
			PickUpGlobe oOrb = oObject.GetComponent<PickUpGlobe>();
			orbs.Add(oOrb);
		}
		numberOrbs = orbs.Count;
		foreach(GameObject seObject in GameObject.FindGameObjectsWithTag("coin")) 
		{
			PickUpSecret sOrb = seObject.GetComponent<PickUpSecret>();
			secrets.Add(sOrb);
		}
		foreach(GameObject sObject in FindGameObjectsWithTags(new string[]{"orangeDestroy", "coin"})) 
		{
			stateObjects.Add(sObject);
		}

		foreach(GameObject dObject in GameObject.FindGameObjectsWithTag("dissPlatform")) 
		{
			FallingPlatform fPlatform = dObject.GetComponent<FallingPlatform>();
			fallingPlatforms.Add(fPlatform);
		}

		foreach(GameObject pObject in FindGameObjectsWithTags(new string[]{"movingPlatform", "chaseBoss"})) 
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
}
