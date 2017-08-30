using UnityEngine;
using System.Collections;

public class SpinningWheel : MonoBehaviour {

	public SpinState spinState;
	public float spinTime;
	private float tempSpinTime;

	public float waitTime;
	private float tempWaitTime;

	public float spinSpeed;

	CircleCollider2D circleCol;

	public GameObject[] particleEffects;

	private bool isSpinning;

	void Start () 
	{
		tempSpinTime = spinTime;
		tempWaitTime = waitTime;

		spinState = SpinState.Spinning;

		circleCol = gameObject.GetComponent<CircleCollider2D>();
	}

	public enum SpinState 
	{
		Waiting,
		Spinning,
	}

	void Update () 
	{
		switch (spinState) 
		{
		case SpinState.Waiting:
			waitTime -= Time.deltaTime;
			spinTime = tempSpinTime;
			isSpinning = false;

			SetThemInactive(particleEffects);

			if (waitTime <= 0)
			{
				spinState = SpinState.Spinning;	
			}
			break;
		case SpinState.Spinning:
			spinTime -= Time.deltaTime;
			waitTime = tempWaitTime;
			isSpinning = true;
			SetThemActive(particleEffects);
			gameObject.transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

			if (spinTime <= 0)
			{	
				spinState = SpinState.Waiting;	
			}
			break;
		}
	}

	void SetThemInactive(GameObject[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	void SetThemActive(GameObject[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if( isSpinning && other.name == "Player")
		{
			PlayerManager.instance.KillPlayer();
		}
	}
}
