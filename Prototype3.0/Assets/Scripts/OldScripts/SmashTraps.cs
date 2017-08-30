using UnityEngine;
using System.Collections;

public class SmashTraps : MonoBehaviour {
	
	LevelManager levelmanager;
	Rigidbody2D rgb;

	public Vector2 velocity;
	public Vector2 killVelocity;

	float absVelocityX;
	float absVelocityY;

	float killVelocityY;
	float killVelocityX;

	void Start () 
	{
		levelmanager = FindObjectOfType<LevelManager>();
		rgb = GetComponent<Rigidbody2D>();
	}

	void Update () 
	{
		velocity = rgb.velocity;

		absVelocityX = Mathf.Abs(velocity.x);
		absVelocityY = Mathf.Abs(velocity.y);

		killVelocityY = killVelocity.y;
		killVelocityX = killVelocity.x;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "Player" && absVelocityX >= killVelocityX && absVelocityY >= killVelocityY)
		{
			Debug.Log("U were killed");
			levelmanager.Respawn();
		}
	}
}
