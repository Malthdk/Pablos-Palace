using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	public int splatterPoolSize = 500;
	public int canonballPoolSize = 30;
	public GameObject splatterPrefab;
	public GameObject canonballPrefab;

	Dictionary<int,Queue<ObjectInstance>> poolDic = new Dictionary<int, Queue<ObjectInstance>> ();

	static PoolManager _instance;

	public static PoolManager instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<PoolManager>();
			}
			return _instance;
		}
	}

	void Start() {
		CreatePool (splatterPrefab, splatterPoolSize);
		CreatePool (canonballPrefab, canonballPoolSize);
	}

	public void CreatePool(GameObject prefab, int poolSize) {
		int poolKey = prefab.GetInstanceID ();

		GameObject poolHolder = new GameObject (prefab.name + " pool");
		poolHolder.transform.parent = transform;

		if (!poolDic.ContainsKey (poolKey)) {
			poolDic.Add (poolKey, new Queue<ObjectInstance> ());

			for (int i = 0; i < poolSize; i++) {
				ObjectInstance newObject = new ObjectInstance (Instantiate (prefab) as GameObject);
				poolDic [poolKey].Enqueue (newObject);
				newObject.SetParent(poolHolder.transform);
			}
		}
	}

	public void ReuseSplatter (GameObject prefab, Vector3 pos, Quaternion rotation, Color color, Vector3 scale) {
		int poolKey = prefab.GetInstanceID ();

		if (poolDic.ContainsKey (poolKey)) {
			ObjectInstance objectToReuse = poolDic [poolKey].Dequeue ();
			poolDic [poolKey].Enqueue (objectToReuse);

			objectToReuse.ReuseSplat (pos, rotation, color, scale);
		}
	}

	public void ReuseCanonball (Transform t, GameObject prefab, Vector3 pos, float power, bool constVel) {
		int poolKey = prefab.GetInstanceID ();

		if (poolDic.ContainsKey (poolKey)) {
			ObjectInstance objectToReuse = poolDic [poolKey].Dequeue ();
			poolDic [poolKey].Enqueue (objectToReuse);

			objectToReuse.ReuseCanon (t, pos, power, constVel);
		}
	}

	public class ObjectInstance {

		GameObject gameObject;
		Transform transform;

		bool hasPoolObjectComponent;
		Splatter splatterScript;
		Canonball canonballScript;

		public ObjectInstance(GameObject objectInstance) {
			gameObject = objectInstance;
			transform = gameObject.transform;
			gameObject.SetActive (false);

			if (gameObject.GetComponent<Splatter>()) {
				hasPoolObjectComponent = true;
				splatterScript = gameObject.GetComponent<Splatter>();
			} else if (gameObject.GetComponent<Canonball>()) {
				hasPoolObjectComponent = true;
				canonballScript = gameObject.GetComponent<Canonball>();
			}
		}

		public void ReuseSplat(Vector3 pos, Quaternion rotation, Color color, Vector3 scale) {
			transform.position = pos;
			transform.rotation = rotation;
			transform.localScale = scale;
			splatterScript.transform.GetComponent<MeshRenderer>().material.color = color;

			if (hasPoolObjectComponent) {
				splatterScript.OnObjectReuse();
			}
			gameObject.SetActive(true);
		}

		public void ReuseCanon(Transform t, Vector3 pos, float power, bool constVel) {
			if (hasPoolObjectComponent) {
				canonballScript.OnObjectReuse();
			}
			transform.position = pos;
			if (!constVel) {
				transform.GetComponent<Rigidbody2D> ().AddForce (t.right * power); //Add our custom force
			} else {
				transform.GetComponent<Rigidbody2D> ().gravityScale = 0f;
				transform.GetComponent<Rigidbody2D> ().AddRelativeForce (t.right * power);
			}
		}

		public void SetParent(Transform parent) {
			transform.parent = parent;
		}

	}
}
