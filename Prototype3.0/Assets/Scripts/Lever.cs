using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	public bool activated;

	public GameObject[] particleSpawners = {};
	public GameObject[] movingPlatforms = {};

	[HideInInspector]
	public ParticleGenerator particlegenerator;		//calling particleGenerator class
	[HideInInspector]
	public PlatformController platformcontroller;

	private Transform from;
	private Transform to;

	private bool rotating = true;


	void Start () 
	{

	}
	
	
	void Update () 
	{
		for (int i = 0; i < particleSpawners.Length; i++) {
			particlegenerator = particleSpawners [i].GetComponent<ParticleGenerator> ();

			if (activated && rotating)										//Lever is activated
			{
				particlegenerator.spawn = true;
				//transform.Rotate(new Vector3(0, 0, 50) * Time.deltaTime);
				StartCoroutine("RotateRight");
			}
			if (!activated && !rotating)										//Lever is deactivated
			{
				particlegenerator.spawn = false;
				StartCoroutine("RotateLeft");
			}
		}
		for (int i = 0; i < movingPlatforms.Length; i++) {
			platformcontroller = movingPlatforms [i].GetComponent<PlatformController> ();
			
			if (activated)										//Lever is activated
			{
				platformcontroller.platformActivator = false;
				transform.localRotation = Quaternion.Euler(0, 0, -327.2233f);
			}
			if (!activated)										//Lever is deactivated
			{
				platformcontroller.platformActivator = true;
				transform.localRotation = Quaternion.Euler(0, 0, 327.2233f);
			}
		}
	}

	public IEnumerator RotateRight() {
		transform.Rotate(new Vector3(0, 0, 50) * Time.deltaTime);
		yield return new WaitForSeconds(2.0f);
		rotating = false;
	}
	public IEnumerator RotateLeft() {
		transform.Rotate(new Vector3(0, 0, -50) * Time.deltaTime);
		yield return new WaitForSeconds(2.0f);
		rotating = true;
	}

}