using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	GameObject splatterParent;
	Transform tf;
	public float splatStayTime = 5f;
	public static PlayerManager pManager;

	[HideInInspector]
	public static PlayerManager _instance;

	public static PlayerManager instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<PlayerManager>();
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Awake () 
	{
		if(pManager == null)
		{
			DontDestroyOnLoad(gameObject);
			pManager = this;
		}
		else if(pManager != this)
		{
			Destroy(gameObject);
		}

	}

	void Start()
	{
		//splatter = FindObjectOfType<Splatter>();
		splatterParent = new GameObject("Splatter Parent");
		tf = splatterParent.transform;
	}

	public void KillPlayer()
	{
		LevelManager.lManager.Respawn();

	}

	public void SpawnSplat(Vector3 position)
	{
		GameObject splat = (GameObject) Instantiate(Resources.Load("Splatter", typeof(GameObject)), position, Quaternion.identity);
		splat.transform.parent = tf;
		Destroy(splat.gameObject, splatStayTime);
	}
}
