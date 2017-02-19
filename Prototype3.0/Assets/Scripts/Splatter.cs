using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
	public static Splatter splatManager;

    public List<Sprite> sprites;
	public GameObject player;

    private SpriteRenderer spriteRenderer;
	private Vector3 playerColor;

	public LayerMask collisionMask;
	private Bounds bounds;
	private CircleCollider2D circleCol;
	private Collider2D playerCollide;
	private Vector2 pointA;
	public bool checkForPlayer;

	private void Awake()
    {
		circleCol = GetComponent<CircleCollider2D>();
		bounds = circleCol.bounds;
        spriteRenderer = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Player");	
		CompareColors();
		pointA = bounds.center;
    }

    private void Start()
    {
		circleCol.enabled = false;
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
		spriteRenderer.color = new Color(playerColor.x, playerColor.y, playerColor.z);
		transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		checkForPlayer = true;
    }

	void Update ()
	{
		if (checkForPlayer)
		{
			playerCollide = Physics2D.OverlapCircle(pointA, circleCol.radius + 0.5f, collisionMask);
			if (playerCollide == null)
			{
				circleCol.enabled = true;
				checkForPlayer = false;
			}
		}
			
	}

	private void CompareColors() {
		if (player.gameObject.tag == "blue") {
			playerColor = new Vector3(0.17f,0.17f,0.85f);
		} else if (player.gameObject.tag == "purple") {
			playerColor = new Vector3(0.55f,0.21f,0.7f);
		} else if (player.gameObject.tag == "red") {
			playerColor = new Vector3(0.85f,0f,0.22f);
		} else if (player.gameObject.tag == "green") {
			playerColor = new Vector3(0.185f,0.75f,0.175f);
		} else if (player.gameObject.tag == "yellow") {
			playerColor = new Vector3(1f,0.82f,0.22f);
		} else if (player.gameObject.tag == "orange") {
			playerColor = new Vector3(1f,0.42f,0.0f);
		}
	}
		
}
