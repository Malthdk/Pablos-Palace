using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
	
	[TextArea(3,10)]
	public string tutorialText;

	public Text canvasText;
	public bool tutorialEnabled;

	void Start() {
		canvasText = GameObject.Find("TutorialText").GetComponent<Text>();
	}

	void Update () {
		
		/*if (tutorialEnabled) {
			uiImage.gameObject.SetActive(true);
			if (Input.GetKeyDown(KeyCode.Return)){
					tutorialEnabled = false;
					uiImage.gameObject.SetActive(false);
					gameObject.SetActive(false);
				}
		}*/

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.name == "Player"){
			canvasText.text = tutorialText;
		}
	}

	void OnDestroy() {
		canvasText.text = "";
	}
}
