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
	public bool moving;
	void FixedUpdate()
	{

		if (moving)
		{
			for (int i = 0; i < movedObjects.Count; i++)
			{
				movedObjects[i].position = Vector3.Lerp(movedObjects[i].position, Vector3.right, 0.05f*Time.deltaTime);
			}
		}
//		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
//		rigidbody.position -= moveRight * speed * Time.deltaTime;
//		rigidbody.MovePosition (rigidbody.position + moveRight * speed * Time.deltaTime);
//		
//		distanceTravelled += speed * Time.deltaTime;
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.name == "Player" || other.gameObject.tag == "DynamicParticle")
		{
			movedObjects.Add(other.transform);
			moving = true;
		}
	}
	void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.name == "Player" || other.gameObject.tag == "DynamicParticle")
		{
			movedObjects.Remove(other.transform);
		}
	}
	
}
