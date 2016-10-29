using UnityEngine;
using System.Collections;

public class LeverPlayer : MonoBehaviour {

	[Tooltip("Very important that levers are called Lever0, Lever1, etc.")]
	public GameObject[] leversInMap;
	private bool[] onLever;

	GameObject[] leverArray;
	public Lever[] lever = {};								//calling Lever class

	void Start () 
	{ 
		//Checking if LeversInMap is empty and filling it if it is
		for(int i = 0; i < leversInMap.Length; i++){
			if (leversInMap[i] == null)
			{
				leversInMap = GameObject.FindGameObjectsWithTag("Lever");
			}
		}

		onLever = new bool[leversInMap.Length];
		leverArray = new GameObject[leversInMap.Length];
		lever = new Lever[leversInMap.Length];

		Debug.Log(leversInMap);
	}
	

	void Update () 
	{

		//Checking if lever is empty and filling it if it is
		for(int i = 0; i < lever.Length; i++){
			if (lever[i] == null)
			{
				lever = new Lever[leversInMap.Length];
			}
		}


		for (int i = 0; i < leversInMap.Length; i++) {
			if (lever[i] != null)
			{
				lever[i] = leversInMap[i].GetComponent<Lever>();
			}
			//lever[i] = leversInMap[i].GetComponent<Lever> ();
			else if (lever[i] == null)
			{
				lever[i] = leversInMap[i].GetComponent<Lever>();
			}
			leverArray[i]=leversInMap[i];
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))		//For activating Levers
		{
			for (int i = 0; i < leversInMap.Length; i++) {
				if(onLever[i] && !lever[i].activated)
				{
					lever[i].activated = true;
				}
				else if(onLever[i] && lever[i].activated)
				{
					lever[i].activated = false;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		for  (int i = 0; i < leversInMap.Length; i++) {
			if (other.gameObject == leversInMap[i].gameObject )
			{
				onLever[i] = true;
			} else {
				onLever[i] = false;
			}
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		for  (int i = 0; i < leversInMap.Length; i++) {
			if (other.tag == "Lever" && leverArray[i].name == "Lever"+i.ToString() )
			{
				onLever[i] = false;
			} 
		}
	}
}
