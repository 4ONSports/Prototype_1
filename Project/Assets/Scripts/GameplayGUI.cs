using UnityEngine;
using System.Collections;

public class GameplayGUI : MonoBehaviour {

	public Color startColor = Color.green;
	public Color midColor = Color.yellow;
	public Color endColor = Color.red;
	public float start2midTime = 0.5f;
	public float mid2endTime = 0.5f;
	public Color currColor = Color.red;

	private BallController playerBC;
	private GameObject guiObj;
	private GameObject shotIndicator;
	private float hideStartTime = 0.0f;
	private float timeToHide = 3.0f;

	// Use this for initialization
	void Start () {
		if( GameObject.Find("Player") !=  null ) {
			playerBC = GameObject.Find("Player").GetComponent<BallController> ();
			shotIndicator = playerBC.gameObject.transform.Find("ShotIndicator").gameObject;
			shotIndicator.renderer.enabled = false;
		}

		currColor = startColor;
	}

	// Update is called once per frame
	void Update () {
		
		switch( InputHandler.swipe_state ) {
		case InputHandler.SwipeState.NONE:
			break;
		case InputHandler.SwipeState.BEGIN:
			currColor = startColor;
			shotIndicator.renderer.material.color = currColor;
			shotIndicator.renderer.enabled = true;
			break;
		case InputHandler.SwipeState.INPROGRESS:
			break;
		case InputHandler.SwipeState.END:
			HideIndicator(3.0f);
			break;
		}

		if (InputHandler.isSwipe) {
			UpdateShotIndicatorColor(Time.time-InputHandler.swipe_startTime);
		}

		UpdateIndicatorVisibility ();
	}

	void UpdateShotIndicatorColor( float fTime ) {
		float timeDelta = 1.0f;
//		Color colorDelta = Color.black;

		if( fTime < start2midTime ) {
			timeDelta = fTime / start2midTime;
			timeDelta = (timeDelta > 1.0f) ? 1.0f : timeDelta;
			//colorDelta = (midColor - startColor) * timeDelta;

			//currColor = startColor + colorDelta;
			currColor = Color.Lerp(startColor, midColor, timeDelta);
		}
		else if( fTime < mid2endTime ) {
			timeDelta = (fTime-start2midTime) / (mid2endTime-start2midTime);
			timeDelta = (timeDelta > 1.0f) ? 1.0f : timeDelta;
			//colorDelta = (endColor - midColor) * timeDelta;
			
			//currColor = midColor + colorDelta;
			currColor = Color.Lerp(midColor, endColor, timeDelta);
		}
		else {
			currColor = endColor;
		}
//		Debug.Log ("timeDelta = " + timeDelta);
//		Debug.Log ("fTime = " + fTime);
//		Debug.Log ("currColor = " + currColor);
//		Debug.Log ("guiObj = " + guiObj);

		shotIndicator.renderer.material.color = currColor;
	}
	
	void HideIndicator( float fTime ) {
		hideStartTime = Time.time;
		timeToHide = fTime;
	}
	
	void UpdateIndicatorVisibility() {
		if( hideStartTime > 0 && (Time.time - hideStartTime) >= timeToHide ) {
			shotIndicator.renderer.enabled = false;

			hideStartTime = 0.0f;
		}
	}
}
