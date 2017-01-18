using UnityEngine;
using System.Collections;

public class WaterTop : MonoBehaviour {

	//Public
	public bool onSurface;


	//Private and hidden
	private Transform trans;
	private float boundsY;
	private float playerY;

	[HideInInspector]
	public static WaterTop _instance;
	public static WaterTop instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<WaterTop>();
			}
			return _instance;
		}
	}

	void Start () 
	{
		boundsY = GetComponent<BoxCollider2D>().bounds.center.y;
	}

	void Update () 
	{
		if (onSurface)
		{
			playerY = boundsY;
			Swimming.instance.isSwimming = true;
			Swimming.instance.notSurface = false;
		}
		else 
		{

		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			onSurface = true;
			trans = other.GetComponent<Transform>();
			playerY = trans.position.y;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			onSurface = false;
		}
	}
}
