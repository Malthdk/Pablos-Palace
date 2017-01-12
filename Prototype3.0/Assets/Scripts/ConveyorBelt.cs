using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour {

	//ALL MARKED CODE SHOULD BE SAVED SINCE IT WORKS WAY BETTER IF PABLO HAS A RGB ATTACHED TO HIM//

//	public float speed = 2.0f;
//	public float distanceTravelled = 0f;
//
//	private Vector2 moveRight = new Vector2(1,0);

	public List<Transform> movedObjects = new List<Transform>();
	public float playerSpeed;
	public float objectSpeed;
	public bool right, left;

	private bool moving, movingPlayer;
	public Vector3 direction;
	private Bounds bounds;
	void Start()
	{
		bounds = GetComponent<BoxCollider2D>().bounds;
		bounds.Expand(new Vector3(20f,0,0));
	}

	void FixedUpdate()
	{

		if (moving)
		{
			if (right)
			{
				direction = new Vector3(bounds.max.x, bounds.max.y, 0f);
				MoveObjects(direction);
			}
			if(left)
			{
				direction = new Vector3(bounds.min.x, bounds.max.y, 0f);
				MoveObjects(direction);
			}
		}
		
//		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
//		rigidbody.position -= moveRight * speed * Time.deltaTime;
//		rigidbody.MovePosition (rigidbody.position + moveRight * speed * Time.deltaTime);
//		
//		distanceTravelled += speed * Time.deltaTime;
	}

	void Update()
	{
		if (movingPlayer)
		{
			MovePlayer();
		}
	}

	/// Collision Enter
	void OnCollisionEnter2D(Collision2D other)
	{
		movedObjects.Add(other.transform);
		moving = true;
	}

	/// Collision Exit
	void OnCollisionExit2D(Collision2D other)
	{
		movedObjects.Remove(other.transform);
	}

	/// Trigger Enter
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			movingPlayer = true;
		}
	}

	/// Trigger Exit
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			movingPlayer = false;
		}
	}

	void MovePlayer()
	{
		Player.instance.gameObject.transform.position += transform.right * playerSpeed * Time.deltaTime;
		Debug.Log("MovingPlyer on ConveyorBelt");
	}

	void MoveObjects(Vector3 direction)
	{
		for (int i = 0; i < movedObjects.Count; i++)
		{
			movedObjects[i].position = Vector3.Lerp(movedObjects[i].position, direction, objectSpeed * Time.deltaTime);
		}
	}
}
