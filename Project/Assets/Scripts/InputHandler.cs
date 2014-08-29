using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	
	public enum SwipeState{
		NONE,
		BEGIN,
		INPROGRESS,
		END,
	}

	public int fingerTouchIndex = 1;
	public static bool useTouch = false;
	public static bool isSwipe = false;
	public static SwipeState swipe_state  = SwipeState.NONE;
	public static float swipe_startTime  = 0.0f;
	public static Vector2 swipe_startPos  = Vector2.zero;
	public static Vector2 swipe_direction  = Vector2.zero;
	public static float swipe_duration  = 0.0f;
	public static float swipe_length  = 0.0f;

//	private bool useTouch = false;
//	private bool isSwipe = false;
//	private float swipeStartTime  = 0.0f;

	
	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			useTouch = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( swipe_state == SwipeState.END )  {
			swipe_state = SwipeState.NONE;
			
			swipe_startTime = 0.0f;
			swipe_startPos = Vector2.zero;
			swipe_direction = Vector2.zero;
			swipe_duration = 0.0f;
			swipe_length = 0.0f;
		}

		// Mouse Controls
		if (!useTouch) {
			Vector2 mousePos;
			mousePos.x = Input.mousePosition.x;
			mousePos.y = Input.mousePosition.y;

			if( Input.GetMouseButtonDown(1) ) {
				swipe_state = SwipeState.BEGIN;
				isSwipe = true;
				swipe_startTime = Time.time;
				swipe_startPos = mousePos;
			}
			else if( Input.GetMouseButtonUp(1) ) {
				swipe_state = SwipeState.END;
				isSwipe = false;
				
				swipe_duration = Time.time - swipe_startTime;
				swipe_length = (mousePos - swipe_startPos).magnitude;
				swipe_direction = mousePos - swipe_startPos;
				swipe_direction.Normalize();
			}
			else if( Input.GetMouseButton(1) ) {
				swipe_state = SwipeState.INPROGRESS;
				swipe_direction = mousePos - swipe_startPos;
				swipe_direction.Normalize();
			}
		}
		
		// Touch Controls
		if (useTouch && Input.touchCount > 0){
			//foreach (Touch touch in Input.touches)
			Touch touch;// = Input.GetTouch(0);
			if( Input.touchCount == 1 ) {
				touch = Input.GetTouch(0);
			}
			else {
				touch = Input.GetTouch(fingerTouchIndex);
			}
			
			if( touch.position.x > (Screen.width/2) )
			{
				switch (touch.phase)
				{
				case TouchPhase.Began :
					swipe_state = SwipeState.BEGIN;
					isSwipe = true;
					swipe_startTime = Time.time;
					swipe_startPos = touch.position;
					break;
					
				case TouchPhase.Canceled :
					/* The touch is being canceled */
					isSwipe = false;
					swipe_state = SwipeState.NONE;
					break;
					
				case TouchPhase.Ended :
					swipe_state = SwipeState.END;
					isSwipe = false;
					
					swipe_duration = Time.time - swipe_startTime;
					swipe_length = (touch.position - swipe_startPos).magnitude;
					swipe_direction = touch.position - swipe_startPos;
					swipe_direction.Normalize();
					break;
					
				case TouchPhase.Moved :
					swipe_state = SwipeState.INPROGRESS;
					swipe_direction = touch.position - swipe_startPos;
					swipe_direction.Normalize();
					break;
				}
			}
		}
	}
}
