using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Music : MonoBehaviour {

	public static Music music;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

}
