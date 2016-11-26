using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonManager : MonoBehaviour {

	public void LoadScene(string sceneName) {
		SceneManager.LoadScene(sceneName);
	}

	public void ExitApplication() {
		Application.Quit();
	}

}
