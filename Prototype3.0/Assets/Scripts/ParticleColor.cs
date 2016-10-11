using UnityEngine;
using System.Collections;

public class ParticleColor : MonoBehaviour {

	ParticleSystem pSystem;
	Abilities abilities;

	void Start () 
	{
		pSystem = GetComponentInChildren<ParticleSystem>();
		abilities = GetComponentInParent<Abilities>();
		pSystem.gameObject.SetActive(false);
	}

	void Update () 
	{
		if(abilities.isBlue && !abilities.isPurple && !abilities.isGreen)
		{
			pSystem.startColor = Color.blue;
		}
		else if (abilities.isRed && !abilities.isPurple && !abilities.isOrange)
		{
			pSystem.startColor = Color.red;
		}
		else if (abilities.isYellow && !abilities.isOrange && !abilities.isGreen)
		{
			pSystem.startColor = Color.yellow;
		}
		else if (abilities.isGreen)
		{
			pSystem.startColor = Color.green;
		}
		else if (abilities.isPurple)
		{
			pSystem.startColor = Color.magenta;
		}
		else if (abilities.isOrange)
		{
			pSystem.startColor = Color.cyan;
		}
		else
		{
			pSystem.startColor = Color.white;
		}
	}
}
