using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	private bool isSwipe = false;
	private float swipeStartTime  = 0.0f;
	private Vector2 swipeStartPos = Vector2.zero;
	private Vector2 swipeDir = Vector2.zero;

	public float minSwipeMagnitude  = 0.0f;
	public float maxSwipeTime = 15.0f;
	public float touchSpeed = 5.0f;
	public float mouseSpeed = 5.0f;
	public float upwardForce = 180.0f;
	public float MAX_UPFORCE = 100.0f;
	public float MAX_BALLSPEED = 0.35f;
	public Rigidbody ballProjectile;
	public Color c1 = Color.gray;
	public Color c2 = Color.red;
	public Color c3 = Color.yellow;
	public Color c4 = Color.red;
	public LineRenderer lineRenderer;
	public bool enableDebugLine = true;
	public int fingerTouchIndex = 1;
	public float shotLerpOffset = 0.3f;

	private bool takeShot = false;
	private float gestureTime;
	private float gestureMagnitude;
	private bool useTouch = false;
	private Vector3 ballLaunchDir;
	private Vector3 ballLaunchDir_Mod;
	private float ballLaunchSpeed;
	private TypeOfFieldView fieldView;

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.Android) {
			useTouch = true;
		}
		fieldView = Camera.main.GetComponent<GameCamera> ().fieldView;
	}
	
	// Update is called once per frame
	void Update () {
		// Mouse Controls
		if (!useTouch) {
			Vector2 mousePos;
			mousePos.x = Input.mousePosition.x;
			mousePos.y = Input.mousePosition.y;

			if( Input.GetMouseButtonDown(0) ) {
				isSwipe = true;
				swipeStartTime = Time.time;
				swipeStartPos = mousePos;
			}
			
			if( Input.GetMouseButtonUp(0) ) {
				gestureTime = Time.time - swipeStartTime;
				gestureMagnitude = (mousePos - swipeStartPos).magnitude;
				
				if (isSwipe && gestureTime < maxSwipeTime && gestureMagnitude > minSwipeMagnitude){
					swipeDir = mousePos - swipeStartPos;
					takeShot = true;
				}
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
					/* this is a new touch */
					isSwipe = true;
					swipeStartTime = Time.time;
					swipeStartPos = touch.position;
					break;
					
				case TouchPhase.Canceled :
					/* The touch is being canceled */
					isSwipe = false;
					break;
					
				case TouchPhase.Ended :
					gestureTime = Time.time - swipeStartTime;
					gestureMagnitude = (touch.position - swipeStartPos).magnitude;
					
					//Debug.Log("gestureTime:  "+gestureTime);
					//Debug.Log("gestureMagnitude:  "+gestureMagnitude);
					if (isSwipe && gestureTime < maxSwipeTime && gestureMagnitude > minSwipeMagnitude){
						swipeDir = touch.position - swipeStartPos;
						takeShot = true;
					}
					break;
				}
			}
		}

		if (takeShot) {
			InitBallLaunch();

			if( enableDebugLine ) {
				//LineRenderer ballLine = Instantiate(lineRenderer, transform.position, transform.rotation) as LineRenderer;
				LineRenderer ballLine = Instantiate(lineRenderer, transform.position, Quaternion.identity) as LineRenderer;
				LineRenderer ballLine_Mod = Instantiate(lineRenderer, transform.position, Quaternion.identity) as LineRenderer;
				
				ballLine.SetColors(c1, c2);
				ballLine.SetWidth(0.1F, 0.1F);
				ballLine.SetVertexCount(2);
				ballLine.SetPosition(0, transform.position);
				ballLine.SetPosition(1, transform.position+(ballLaunchDir * ballLaunchSpeed));
				
				ballLine_Mod.SetColors(c3, c4);
				ballLine_Mod.SetWidth(0.1F, 0.1F);
				ballLine_Mod.SetVertexCount(2);
				ballLine_Mod.SetPosition(0, transform.position);
				ballLine_Mod.SetPosition(1, transform.position+(ballLaunchDir_Mod * ballLaunchSpeed));
				
				Destroy (ballLine.gameObject, 1.0f);
				Destroy (ballLine_Mod.gameObject, 1.0f);
			}
		}
	}

	void FixedUpdate() {
		if (takeShot) {
			takeShot = false;

			Rigidbody shot = Instantiate(ballProjectile, transform.position, transform.rotation) as Rigidbody;
			Physics.IgnoreCollision(gameObject.collider, shot.collider);
			Physics.IgnoreLayerCollision(shot.gameObject.layer, shot.gameObject.layer);
			shot.AddForce(ballLaunchDir_Mod * ballLaunchSpeed, ForceMode.Impulse);
			//Debug.DrawLine(transform.position, transform.position+(ballLaunchDir * ballLaunchSpeed), Color.red, 2, false);
			
			Destroy (shot.gameObject, 3.0f);
		}
	}

	void InitBallLaunch() {
		swipeDir.Normalize();
		switch (fieldView)
		{
		case TypeOfFieldView.BACK_VIEW :
			break;
		case TypeOfFieldView.SIDE_VIEW_LEFT :
			swipeDir = Utility.Rotate2D(swipeDir, -90.0f);
			break;
		case TypeOfFieldView.SIDE_VIEW_RIGHT :
			swipeDir = Utility.Rotate2D(swipeDir, 90.0f);
			break;
		}
		swipeDir.Normalize();
		//Vector2 swipeDir_Up = Utility.Rotate2D(Vector2.up, 90.0f);
		Vector2 swipeDir_Up = Vector2.up;
		//Vector2 swipeDir_Up = swipeStartPos + Vector2.up;
		swipeDir_Up.Normalize();
		//Vector2 swipeDir_Up = Vector2.right;//Vector2.up;
		Vector2 swipeDir_Mod = Vector2.Lerp(swipeDir, swipeDir_Up, shotLerpOffset);
		
		
		float upForce = upwardForce * gestureTime;
		if( upForce > MAX_UPFORCE )
			upForce = MAX_UPFORCE;
		ballLaunchDir.x = swipeDir.x * gestureMagnitude;
		ballLaunchDir.y = upForce;
		ballLaunchDir.z = swipeDir.y * gestureMagnitude;
		ballLaunchDir_Mod.x = swipeDir_Mod.x * gestureMagnitude;
		ballLaunchDir_Mod.y = upForce;
		ballLaunchDir_Mod.z = swipeDir_Mod.y * gestureMagnitude;

		if( useTouch ){
			ballLaunchSpeed = gestureTime * touchSpeed;
		}
		else {
			ballLaunchSpeed = gestureTime * mouseSpeed;
		}
		if( ballLaunchSpeed > MAX_BALLSPEED )
			ballLaunchSpeed = MAX_BALLSPEED;
	}
}
