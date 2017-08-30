using UnityEngine;
using System.Collections;

public class Pendulum : MonoBehaviour {

	public float angle = 40.0f;
	public float speed = 1.5f;
	//public bool fullRot;
	public bool pendulum;

	Quaternion qStart, qEnd;

	//private float angle2 = 360f;

	//private Vector3 v3To = new Vector3(0f, 0f, 360f);
	//public Vector3 v3Current = new Vector3(0f, 0f, 0f);

	void Start () 
	{
		qStart = Quaternion.AngleAxis ( angle, Vector3.forward);
		qEnd   = Quaternion.AngleAxis (-angle, Vector3.forward);

	//	transform.eulerAngles = v3Current; 

	//	Quaternion target = Quaternion.Euler(30, 0, tiltAroundZ);
	}

	void Update () 
	{
		//OTHER KINDS OF ROTATIONS CAN BE PUT HERE
		//if (fullRot)
		//{
			//transform.Rotate(Vector3.forward * (Mathf.Sin(Time.time * speed) + 1.0f)); //MAYBE USEFULL FOR GEAR ISH MOVEMENT


			//v3Current = Vector3.Lerp(v3Current, v3To, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);
			//transform.eulerAngles = v3Current; 

		//}

		if (pendulum)
		{
			transform.rotation = Quaternion.Lerp (qStart, qEnd, (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f);		
		}
	}
}