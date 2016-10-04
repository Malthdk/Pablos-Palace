using UnityEngine;
using System.Collections;


public class ExampleTopDownPlayer : MonoBehaviour {

	//Example scripts are included to demonstate how tile prefabs can be customized to behave in a game setting.

	/// <summary>
	/// ExampleTopDownPlayer is a simple script to move around in the game world to test tile world behavior.
	/// </summary>


	Rigidbody2D cacheRB;
	public float speed = 2;

	void Awake () {
		cacheRB = GetComponent<Rigidbody2D>();
	}
	

	void FixedUpdate () {
		cacheRB.velocity = Movement();
	}

	Vector2 Movement() {
		Vector2 vel = new Vector2();
		vel.x = Input.GetAxis("Horizontal")* speed;
		vel.y = Input.GetAxis("Vertical")* speed;
		return vel;
	}


#if UNITY_EDITOR
	void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere((Vector2)transform.position, .15f);
	}
#endif
}
