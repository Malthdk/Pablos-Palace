using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public float splatStayTime = 5f;
	public static PlayerManager pManager;

	private GameObject splatterParent;
	private Vector3 lastPosition;
	private Vector3 currentPosition;
	private Vector3 dirToCurrentPos;
	private Vector3 absDirToCurrentPos;
	private Vector3 cross;
	private Vector3 absCross;
	private Vector3 renderDirection = new Vector3(0, 0, -1f);
	[HideInInspector]
	public static PlayerManager _instance;
	[HideInInspector]
	public static GameObject splatterPrefab;

	public static PlayerManager instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<PlayerManager>();
			}
			return _instance;
		}
	}

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
		splatterParent = new GameObject("Splatter Parent");
		lastPosition = transform.position;
		splatterPrefab = PoolManager.instance.prefab;
	}

	void Update()
	{
		currentPosition = transform.position;
		dirToCurrentPos = (currentPosition - lastPosition).normalized;
		lastPosition = transform.position;
	}

	public void KillPlayer()
	{
		LevelManager.lManager.Respawn();
	}

	public void SpawnSplat(Vector3 position)
	{
		float angle = Mathf.Atan2(dirToCurrentPos.y, dirToCurrentPos.x) * Mathf.Rad2Deg;
		if (Controller2D.instance.collisions.faceDir == -1)
		{
			angle += 180;
		}
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

		PoolManager.instance.ReuseObject (splatterPrefab, position, q);

	}
}