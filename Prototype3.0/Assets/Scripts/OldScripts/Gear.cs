using UnityEngine;
using System.Collections;

public class Gear : MonoBehaviour {

	HingeJoint2D hingeJoint;
	JointMotor2D jointMotor;
	bool stopped;
	
	public float motorSpeed;
	public float newMotorSpeed;
	public float invokeTime;

	void Start () {

		hingeJoint = GetComponent<HingeJoint2D>();
		jointMotor = hingeJoint.motor;

	}
	

	void Update () {
		JointMotor2D motor = hingeJoint.motor;
		if(stopped)
		{
			motor.motorSpeed = 0;
			hingeJoint.motor = motor;
			Debug.Log("stopped!");
		}
		if(!stopped)
		{
			motor.motorSpeed = motorSpeed;
			hingeJoint.motor = motor;
			Debug.Log("stopped!");
		}

		if (motor.motorSpeed >= motorSpeed || motor.motorSpeed <= motorSpeed)
		{
			Debug.Log("getting motor speed");
			Invoke("StopClock", invokeTime);
		}
		if (motor.motorSpeed == 0)
		{
			Debug.Log("getting motor speed");
			Invoke("StartClock", invokeTime);
		}
	}

	void StopClock() 
	{
		stopped = true;
	}
	void StartClock()
	{
		stopped = false;
	}
}
