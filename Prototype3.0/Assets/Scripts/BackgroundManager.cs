using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

	public List<SpriteRenderer> backgroundMaterials;
	public int orbs = 5;
	private float amount = 1f;
	private int temp;

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
	}

	public void ColorBackground() {
		temp++;
		StartCoroutine(FadeTime(2f));
	}

	IEnumerator FadeTime(float time)
	{
		float oldValue = amount;
		amount = 1f-(temp * 1f / orbs);
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
		{
			float newValue = Mathf.Lerp(oldValue,amount,t);
			foreach(SpriteRenderer m in backgroundMaterials) 
			{
				m.material.SetFloat("_Brightness", newValue);
				m.material.SetFloat("_EffectAmount", newValue);
			}
			yield return null;
		}
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
