using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonManager : MonoBehaviour {

	//For loading
	public Slider loadingbar;
	public GameObject loadingImage;
	private AsyncOperation async;

	public void LoadScene(string sceneName) { //Loads scene with a loadscreen
		//SceneManager.LoadScene(sceneName);

		GetLevel(sceneName);
	}
	public void LoadSceneNoScreen(string sceneName){ //Loads scene without a loadscreen
		SceneManager.LoadScene(sceneName);
	}

	public void ExitApplication() {
		Application.Quit();
	}
		
	public void GetLevel(string level)
	{
		loadingImage.SetActive(true);
		StartCoroutine(LoadLevelWithBar(level));
	}

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
