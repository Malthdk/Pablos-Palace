using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public Text uiImage;
	public bool tutorialEnabled;

	void Start () 
	{
	
	}
	

	void Update () 
	{
		if (tutorialEnabled)
		{
			uiImage.gameObject.SetActive(true);
			if (Input.GetKeyDown(KeyCode.Return))
				{
					tutorialEnabled = false;
					uiImage.gameObject.SetActive(false);
					gameObject.SetActive(false);
				}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			tutorialEnabled = true;
		}
	}
}
