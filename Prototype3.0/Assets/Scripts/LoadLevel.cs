using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	
	
	public void LoadScene(int Level)
	{
		Application.LoadLevel(Level);
	}
}
