using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailRendererWith2DCollider : MonoBehaviour {

	//************
	//
	// Fields
	//  
	//************

	public Material trailMaterial;                  //the material of the trail.  Changing this during runtime will have no effect.

	private bool isActionPerformed = false;
	private Transform trans;                        //transform of the object this script is attached to                                               
	private Trail currentTrail;
	private CreateSplat createSplat;
	//************
	//
	// Private Unity Methods
	//
	//************

	private void Awake() 
	{
		createSplat = GetComponent<CreateSplat>();
	}

	private void Update() {
		if (Input.GetButton("Special") && !ColorStates.instance.isWhite)
		{
			if (createSplat.onMiddleGround)
			{
				StartCoroutine(BuildTrail());
			}
			else
			{
				currentTrail.building = false;
				isActionPerformed = false;
			}
		}
		if (Input.GetButtonUp("Special"))
		{
			currentTrail.building = false;
			isActionPerformed = false;
		}
			
	}

	//************
	//
	// Private Methods
	//
	//************

	IEnumerator BuildTrail(){

		while(!isActionPerformed){
			
			yield return new WaitForEndOfFrame ();
			CreateTrail();
			currentTrail.building = true;

			//toggle the boolean whenever you acheive your objective
			isActionPerformed = true;
		}
	}

	private void CreateTrail()
	{
		GameObject trail = new GameObject("Trail", new[] { typeof(MeshRenderer), typeof(MeshFilter), typeof(PolygonCollider2D), typeof(Trail) } );
		currentTrail = trail.GetComponent<Trail>();
		trail.layer = 8;
		trail.tag = "Through";

		Trail trailScript = trail.GetComponent<Trail>();
		//get and set the polygon collider on this trail.
		//isTrigger = colliderIsTrigger;
	}
}