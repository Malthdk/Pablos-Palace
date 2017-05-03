using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffPaper : MonoBehaviour {

	//publics
	public float turnTime;
	public float startDelay;
	public OnAndOff onAndOff;

	//privates
	private MeshRenderer myRenderer;
	private float tempTurnTime;

	public enum OnAndOff
	{
		On,
		Off
	}

	void Start () 
	{
		myRenderer = gameObject.GetComponent<MeshRenderer>();
		tempTurnTime = turnTime;
	}

	void Update () 
	{
		switch (onAndOff)
		{
		case OnAndOff.On:

			myRenderer.material.color = Color.white;
			gameObject.layer = 14;

			tempTurnTime -= Time.deltaTime;
			if (tempTurnTime <= 0)
			{
				onAndOff = OnAndOff.Off;
				tempTurnTime = turnTime;
			}
			break;

		case OnAndOff.Off:

			myRenderer.material.color = Color.grey;
			gameObject.layer = 0;

			tempTurnTime -= Time.deltaTime;
			if (tempTurnTime <= 0)
			{
				onAndOff = OnAndOff.On;
				tempTurnTime = turnTime;
			}
			break;
		}
	}

	IEnumerator StartDelay()
	{
		yield return new WaitForSeconds(startDelay);

		onAndOff = OnAndOff.On;
	}
		
}
