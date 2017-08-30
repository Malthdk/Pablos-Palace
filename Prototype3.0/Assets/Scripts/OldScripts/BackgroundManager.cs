using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

	public List<SpriteRenderer> backgroundMaterials;
	public int orbs = 5;
	private float amount = 1f;
	private int temp;
	private Color tempCol;

	[HideInInspector]
	public static BackgroundManager _instance;

	public static BackgroundManager instance {	// Makes it possible to call script easily from other scripts
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<BackgroundManager>();
			}
			return _instance;
		}
	}

	void Start () {
		foreach(GameObject obj in FindGameObjectsWithTags(new string[]{"BackgroundElement"})) 
		{
			backgroundMaterials.Add(obj.GetComponent<SpriteRenderer>());
		}
		orbs = FindGameObjectsWithTags ("orb").Length;
		tempCol = new Color (1f, 1f, 1f);
	}

	public void ColorBackground(Color col) {
		temp++;
		StartCoroutine(FadeTime(3f, col));
	}

	IEnumerator FadeTime(float time, Color col)
	{
		float oldValue = amount;
		amount = 1f-(temp * 1f / orbs);
		if (amount < 0) { // QUICKFIX FOR BG TURNING BLACK
			amount = 0;
		}
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
		{
			float newValue = Mathf.Lerp(oldValue,amount,t);
			Color newColorValue = Color.Lerp(tempCol,col,t); 
			foreach(SpriteRenderer m in backgroundMaterials) 
			{
				//m.material.SetFloat("_Brightness", newValue);
				m.material.SetColor ("_Color", newColorValue);
				//m.material.SetFloat("_EffectAmount", newValue);
			}
			yield return null;
		}
		tempCol = col;
	}

	GameObject[] FindGameObjectsWithTags(params string[] tags)
	{
		var all = new List<GameObject>();

		foreach(string tag in tags)
		{
			var temp = GameObject.FindGameObjectsWithTag(tag).ToList();
			all = all.Concat(temp).ToList();
		}

		return all.ToArray();
	}
}
