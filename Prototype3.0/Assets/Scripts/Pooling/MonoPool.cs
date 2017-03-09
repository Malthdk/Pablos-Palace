using UnityEngine;
using Pablo.Pooling;

public class MonoPool : MonoBehaviour {

	public int Capacity;
	public IPool<GameObject> Pool { get; private set;}
	public GameObject Prefab;
	public enum PoolType { Queue, List }
	public PoolType Pooltype = PoolType.Queue;
	public bool Expandable = true;

	void Awake() {
		switch(Pooltype) {
			case PoolType.Queue:
				Pool = new QueuePool<GameObject>(() => Instantiate(Prefab), Capacity);	
				break;
			case PoolType.List:
				Pool = new ListPool<GameObject>(() => Instantiate(Prefab), Capacity, g=>g.activeInHierarchy, Expandable);
				break;
			default:
				return;

		}
	}
}
