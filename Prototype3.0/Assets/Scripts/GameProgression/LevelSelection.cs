using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {

	[System.Serializable]
	public class Levels
	{
		public Level level;
		public Transform transform;
		public string levelText;
		public int unlocked;
	}

	public Levels[] wayPoints;
	public Transform pabloImage;
	public Transform wayPointRef;

	public int level;
	public int amountOfSecretLevels;

	//For loading
	public Slider loadingbar;
	public GameObject loadingImage;
	private AsyncOperation async;

	void Start () 
	{
		amountOfSecretLevels = amountOfSecretLevels + 1; //because arrays start 0
		level = 0;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (level < wayPoints.Length - 1)
			{
				if (level != wayPoints.Length - amountOfSecretLevels)
				{
					if (wayPoints[level + 1].unlocked == 1)
					{
						level ++;
					}
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (level > 0)
			{
				level --;
			}
		}

		//Script this for more dynamic secret level setup
		if (Input.GetKeyDown(KeyCode.UpArrow) && level == 2)
		{
			level = wayPoints.Length - 1;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) && level == wayPoints.Length - 1)
		{
			level = 2;
		}

		wayPointRef = wayPoints[level].transform;
		pabloImage.transform.position = wayPointRef.position;	

		if (Input.GetKeyDown(KeyCode.Space))
		{
			GetLevel(level);
		}
	}
		
	public void GetLevel(int level)
	{
		loadingImage.SetActive(true);
		StartCoroutine(LoadLevelWithBar(level));
	}

	IEnumerator LoadLevelWithBar (int level)
	{
		async = Application.LoadLevelAsync(level);
		while (!async.isDone)
		{
			loadingbar.value = async.progress;
			yield return null;
		}
	}
}
