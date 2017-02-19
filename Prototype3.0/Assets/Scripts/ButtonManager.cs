using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonManager : MonoBehaviour {
	
	// For loading
	public Slider loadingbar;
	public GameObject loadingImage;
	private AsyncOperation async;




	// Loads scene with a loadscreen
	public void LoadScene(string sceneName) {
		GetLevel(sceneName);
	}

	// Loads scene without a loadscreen
	public void LoadSceneNoScreen(string sceneName){ 
		SceneManager.LoadScene(sceneName);
	}

	// Exits the game
	public void ExitApplication() {
		Application.Quit();
	}
		
	// Sets loading image to active and starts coroutine
	public void GetLevel(string level)
	{
		loadingImage.SetActive(true);
		StartCoroutine(LoadLevelWithBar(level));
	}

	// While level is loading display loadingbar
	IEnumerator LoadLevelWithBar (string level)
	{
		async = Application.LoadLevelAsync(level);
		while (!async.isDone)
		{
			loadingbar.value = async.progress;
			yield return null;
		}
	}



}
