using UnityEngine;
using System.Collections;

public class HauntedPicture : MonoBehaviour {

	public SpriteRenderer spriteRend;
	private Color color;
	public Haunt haunt;

	public float killTimeFraction;

	private bool killPlayer;
	private bool hauntingPlayer;
	private bool waiting; 

	/// NEW ATTEMT
	public LayerMask collisionMask;
	private Transform player;
	private Vector2 rayOrigin;
	private BoxCollider2D bCollider;
	private Bounds bounds;
	///


	void Start () 
	{
		player = GameObject.Find("Player").transform;
		bCollider = GetComponent<BoxCollider2D>();
		bounds = bCollider.bounds;
		PlaceRayOrigin();

		color = spriteRend.material.color;
		color.a = 0;
		spriteRend.material.color = color;
	}

	public enum Haunt
	{
		Waiting,
		Haunting,
		Kill
	}

	void Update () 
	{
		switch(haunt)
		{
		case Haunt.Waiting:
			Waiting();
			break;

		case Haunt.Haunting:

			/// NEW STUFF
			Vector3 center = new Vector3(bounds.center.x, bounds.center.y, 0);
			Vector2 dir = player.position - center;

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, Mathf.Infinity, collisionMask);
			Debug.DrawRay(rayOrigin, dir * 15f, Color.red);

			if (hit.collider != null) 
			{
				if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
				{
					Debug.Log("I SEE YOU!");
					Haunting();
				}
				else
				{
					Waiting();
				}
			}
			/// 

			if (color.a >= 1)
			{
				haunt = Haunt.Kill;
			}

			break;

		case Haunt.Kill:
			PlayerManager.pManager.KillPlayer();
			haunt = Haunt.Waiting;
			break;
		}
	}
				
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			Debug.Log("entered kill zone");
			haunt = Haunt.Haunting;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player")
		{
			Debug.Log("exited kill zone");
			haunt = Haunt.Waiting;
		}
	}

	void Haunting()
	{
		color.a += killTimeFraction;
		spriteRend.material.color = color;
	}

	void Waiting()
	{
		if(color.a > 0)
		{
			color.a -= killTimeFraction;
			spriteRend.material.color = color;
		}
	}

	void PlaceRayOrigin()
	{
		bounds.Expand (0.015f * -2);
		rayOrigin = new Vector2(bounds.center.x, bounds.center.y);

	}
}
