using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	GameObject splatterParent;
	public float splatStayTime = 5f;
	public static PlayerManager pManager;

	Vector3 lastPosition;
	Vector3 currentPosition;
	Vector3 dirToCurrentPos;
	Vector3 absDirToCurrentPos;
	Vector3 cross;
	Vector3 absCross;
	Vector3 renderDirection = new Vector3(0, 0, -1f);
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

		lastPosition = transform.position;
	}

	void Update()
	{
		currentPosition = transform.position;

		dirToCurrentPos = (currentPosition - lastPosition).normalized;
		//absDirToCurrentPos = new Vector3 (Mathf.Abs(dirToCurrentPos.x), Mathf.Abs(dirToCurrentPos.y), Mathf.Abs(dirToCurrentPos.z));

		//cross = Vector3.Cross(renderDirection, dirToCurrentPos);
		//absCross = new Vector3(Mathf.Abs(cross.x), Mathf.Abs(cross.y), Mathf.Abs(cross.z));

		//Debug.DrawRay(currentPosition, dirToCurrentPos, Color.red);
		//Debug.DrawRay(currentPosition, absCross, Color.blue);
		//Debug.DrawRay(currentPosition, Vector3.forward, Color.black);
		lastPosition = transform.position;
	}

	public void KillPlayer()
	{
		LevelManager.lManager.Respawn();
	}

	public void SpawnSplat(Vector3 position)
	{
		// Brug sidste objekts position i stedet for sidste update;
		//dirToCurrentPos.x = 0.0f;
		//dirToCurrentPos.y = 0.0f;

		//Vector3 vectorToTarget = targetTransform.position - transform.position;
		float angle = Mathf.Atan2(dirToCurrentPos.y, dirToCurrentPos.x) * Mathf.Rad2Deg;
		if (Controller2D.instance.collisions.faceDir == -1)
		{
			angle += 180;
		}
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		//transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 1f);

		//cross.x = 0.0f;
		//cross.y = 0.0f;
		//Quaternion rotation = Quaternion.LookRotation(cross);

		//Debug.Log("direction = " + dirToCurrentPos);
		//Debug.Log("cross = " + cross);

		GameObject splat = (GameObject) Instantiate(Resources.Load("Splatter", typeof(GameObject)), position, q);
		//splat.transform.parent = tf;
		//Destroy(splat.gameObject, splatStayTime);
	}
}