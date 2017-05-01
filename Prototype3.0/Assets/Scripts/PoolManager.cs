using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	public int poolSize = 500;
	public GameObject prefab;

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
		CreatePool (prefab, poolSize);
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

	public void ReuseObject (GameObject prefab, Vector3 pos, Quaternion rotation, Color color, Vector3 scale) {
		int poolKey = prefab.GetInstanceID ();

		if (poolDic.ContainsKey (poolKey)) {
			ObjectInstance objectToReuse = poolDic [poolKey].Dequeue ();
			poolDic [poolKey].Enqueue (objectToReuse);

			objectToReuse.Reuse (pos, rotation, color, scale);
		}
	}

	public class ObjectInstance {

		GameObject gameObject;
		Transform transform;

		bool hasPoolObjectComponent;
		Splatter splatterScript;

		public ObjectInstance(GameObject objectInstance) {
			gameObject = objectInstance;
			transform = gameObject.transform;
			gameObject.SetActive (false);

			if (gameObject.GetComponent<Splatter>()) {
				hasPoolObjectComponent = true;
				splatterScript = gameObject.GetComponent<Splatter>();
			}
		}

		public void Reuse(Vector3 pos, Quaternion rotation, Color color, Vector3 scale) {
			transform.position = pos;
			transform.rotation = rotation;
			transform.localScale = scale;
			splatterScript.transform.GetComponent<MeshRenderer>().material.color = color;

			if (hasPoolObjectComponent) {
				splatterScript.OnObjectReuse();
			}
			gameObject.SetActive(true);
		}

		public void SetParent(Transform parent) {
			transform.parent = parent;
		}

	}
}
