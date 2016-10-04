using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	GameObject splatterParent;
	Transform tf;

	public static PlayerManager pManager;

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
		Destroy(splat.gameObject, 25.0f);
	}
}
