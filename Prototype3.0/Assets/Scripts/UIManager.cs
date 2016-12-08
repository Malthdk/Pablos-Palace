using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager uiManager;
	public Text uiText;
	//public Controller2D score;
	public int score = 0;
	public int fps = 0;

	int frameCount = 0;
	float dt = 0.0f;
	float updateRate = 3.0f;  // 4 updates per sec.

	// Use this for initialization

	void Awake()
	{	
		/* DISABLED BY CHRIS FOR USERTEST
		if(uiManager == null)
		{
			DontDestroyOnLoad(gameObject);
			uiManager = this;
		}
		else if(uiManager != this)
		{
			Destroy(gameObject);
		}
		*/
	}

	void Start () {
		//DontDestroyOnLoad(gameObject); DISABLED BY CHRIS FOR USERTEST
		uiText = FindObjectOfType<Text>();
		//score = FindObjectOfType<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {
		// FRAMERATE
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1.0f/updateRate)
		{
			fps = (int)(frameCount / dt);
			frameCount = 0;
			dt -= 1.0f/updateRate;
		}

		uiText.text = "COINS: " + score + "   FPS: " + fps;
	}
}

//if (GameObject.Find("Canvas") == null || GameObject.Find("Canvas(Clone)") == null) {
//	GameObject instance = Instantiate(Resources.Load("Canvas", typeof(GameObject))) as GameObject;
//}