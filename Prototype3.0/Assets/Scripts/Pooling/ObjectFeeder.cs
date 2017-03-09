using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFeeder : MonoBehaviour {

	public MonoPool SourcePool;
	public string Message;
	public GameObject Destination;
	public float InvokeDelay = 1f;
	public float InvokePeriod = .5f;

	void Start() {
		InvokeRepeating("FeedObject", InvokeDelay, InvokePeriod);
	}

	void FeedObject() {
		Destination.SendMessage(Message, SourcePool.Pool.GetInstance());
	}
}
