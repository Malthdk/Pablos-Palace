using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Controller2D target;
	public float verticalOffset;
	public float horizontalOffset;
	public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public float horizontalSmoothTime;

	public Vector2 focusAreaSize;

	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
	float smoothVelocityY;
	float smoothVelocityX;

//	float smoothFindX;
//	float smoothFindY;
//	public bool findingTarget = false;

	bool lookAheadStopped;

	void Start()
	{
		target = FindObjectOfType<Controller2D>();
		focusArea = new FocusArea(target.collider.bounds, focusAreaSize);
	}

	void Update()
	{
		if(target == null)
		{
			target = FindObjectOfType<Controller2D>();
		}
	}
	void LateUpdate()
	{
		focusArea.Update (target.collider.bounds);

		Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset + Vector2.right * horizontalOffset;

		if (focusArea.velocity.x != 0)
		{
			lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
			if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0)
			{
				lookAheadStopped = false;
				targetLookAheadX = lookAheadDirX * lookAheadDstX;
			}
			else
			{
				if(!lookAheadStopped)
				{
					lookAheadStopped = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX)/4f;
				}
			}
		}

	//	currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX); //FOR LOOKING AHEAD DO NOT DELETE

		focusPosition.x = Mathf.SmoothDamp(transform.position.x, focusPosition.x, ref smoothVelocityX, horizontalSmoothTime);
		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		//focusPosition += Vector2.right * currentLookAheadX;																//FOR LOOKING AHEAD DO NOT DELETE

		//float rounded_x = RoundToNearestPixel(focusPosition.x);
		//float rounded_y = RoundToNearestPixel(focusPosition.y);

//		if (findingTarget)
//		{
//			Vector3 pos = transform.position;
//			pos.x = Mathf.SmoothDamp(pos.x, focusPosition.x, ref smoothFindX, 1f);
//			pos.y = Mathf.SmoothDamp(pos.y, focusPosition.y, ref smoothFindY, 1f);
//			transform.position = pos;
//			Debug.Log( "fpos:" + focusPosition.x);
//			Debug.Log("pos: " + pos.x);
//			float threshold = 1f;
//			if (Mathf.Abs(pos.x - focusPosition.x) < threshold && Mathf.Abs(pos.y - focusPosition.y) < threshold)
//			{
//				findingTarget = false;
//			}
//		}
//		else 
//		{
			transform.position = (Vector3)focusPosition + Vector3.forward * -10;
			//transform.position = new Vector3(rounded_x, rounded_y, -10);
//		}
	}

	/// <summary>
	/// This is an attempt to remove the tile rendering problem.
	/// </summary>
//	public float pixelToUnits = 128f;
//
//	public float RoundToNearestPixel(float unityUnits)
//	{
//		float valueInPixels = unityUnits * pixelToUnits;
//		valueInPixels = Mathf.Round(valueInPixels);
//		float roundedUnityUnits = valueInPixels * (1 / pixelToUnits);
//		return roundedUnityUnits;
//	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (focusArea.centre, focusAreaSize);
	}
	struct FocusArea
	{
		public Vector2 centre;
		public Vector2 velocity;
		float left,right;
		float top,bottom;


		public FocusArea(Bounds targetBounds, Vector2 size)
		{
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2((left+right)/2, (top+bottom)/2);
		}

		public void Update(Bounds targetBounds)
		{
			float shiftX = 0;
			if (targetBounds.min.x < left)
			{
				shiftX = targetBounds.min.x - left;
			}
			else if (targetBounds.max.x > right)
			{
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom)
			{
				shiftY = targetBounds.min.y - bottom;
			}
			else if (targetBounds.max.y > top)
			{
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			centre = new Vector2((left+right)/2, (top+bottom)/2);
			velocity = new Vector2 (shiftX, shiftY);

		}
	}
}
