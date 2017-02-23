using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splatter : MonoBehaviour
{
	public static Splatter splatManager;

    //public List<Sprite> sprites;
	public GameObject player;

    private SpriteRenderer spriteRenderer;
	private Vector3 playerColor;

	public LayerMask collisionMask;
	private Bounds bounds;
	private CircleCollider2D circleCol;
	private Collider2D playerCollide;
	private Vector2 pointA;
	public bool checkForPlayer;

	public float splatStayTime = 5f;


	private Material material;
	private void Awake()
    {
		material = GetComponent<MeshRenderer>().material;
		circleCol = GetComponent<CircleCollider2D>();
		bounds = circleCol.bounds;
        //spriteRenderer = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Player");	
		//CompareColors();
		pointA = bounds.center;
    }

    private void Start()
    {
		circleCol.enabled = false;
        //spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
		//spriteRenderer.color = new Color(playerColor.x, playerColor.y, playerColor.z);
		//transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		checkForPlayer = true;
		StartCoroutine(DestroySplat());
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
			playerColor = new Vector3(0.185f,0.55f,0.175f);
		} else if (player.gameObject.tag == "yellow") {
			playerColor = new Vector3(1f,0.82f,0.22f);
		} else if (player.gameObject.tag == "orange") {
			playerColor = new Vector3(1f,0.42f,0.0f);
		}
	}

	public IEnumerator DestroySplat()
	{
		yield return new WaitForSeconds(splatStayTime);

		circleCol.enabled = false;
		Color color = material.color;

		if (player.gameObject.tag == "blue") {
			color = new Color (0.17f,0.17f,0.85f, 0.05f);
			material.color = color;

		} else if (player.gameObject.tag == "purple") {
			color = new Color (0.55f,0.21f,0.7f, 0.05f);
			spriteRenderer.color = color;

		} else if (player.gameObject.tag == "red") {
			color = new Color (0.85f,0f,0.22f, 0.05f);
			spriteRenderer.color = color;

		} else if (player.gameObject.tag == "green") {
			color = new Color (0.185f,0.55f,0.175f, 0.05f);
			spriteRenderer.color = color;

		} else if (player.gameObject.tag == "yellow") {
			color = new Color (1f,0.82f,0.22f, 0.05f);
			spriteRenderer.color = color;

		} else if (player.gameObject.tag == "orange") {
			color = new Color (1f,0.42f,0.0f, 0.05f);
			spriteRenderer.color = color;
		}
			
	}

}
