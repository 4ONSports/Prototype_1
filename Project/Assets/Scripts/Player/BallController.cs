using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallController : MonoBehaviour {
	private Vector2 swipeDir = Vector2.zero;
	private Vector2 swipeDir_Mod = Vector2.zero;
	
	public bool enableDebugLine = false;
	public bool enableDebugText = false;
	public bool createBallFromBC = true;
	public bool useConstantSwipeTime  = true;
	public float constantSwipeTime  = 2.0f;
	public float minSwipeMagnitude  = 0.0f;
	public float maxSwipeTime_Touch = 15.0f;
	public float maxSwipeTime_Mouse = 15.0f;
	private float maxSwipeTime = 1.0f;
	//public float upwardForce = 180.0f;
	public float MAX_UPFORCE = 100.0f;
	public float MAX_BALLSPEED = 0.35f;
	public float shotSpeedFactor_Touch = 15.0f;
	public float shotSpeedFactor_Mouse = 90.0f;
	private float shotSpeedFactor = 1.0f;
	public float shotHeightFactor_Touch = 15.5f;
	public float shotHeightFactor_Mouse = 15.5f;
	private float shotHeightFactor = 1.0f;
	public Rigidbody ballProjectile;
	public Color c1 = Color.gray;
	public Color c2 = Color.red;
	public Color c3 = Color.yellow;
	public Color c4 = Color.red;
	public LineRenderer lineRenderer;
	public int fingerTouchIndex = 1;
	public float[] shotLerpOffsets;
	public int NumOfUpdatesForShooting = 10;
	public float shotDirLineLength = 10.0f;
	public GameObject shotTarget;
	
	[SerializeField] private Transform parentedSoccerBallTransform = null;

	private bool takeShot = false;
	private bool isPlayerInPossessionOfABall = false;
	private bool renderShotDirLine = false;
	private const bool kUseConstantSwipeMagnitude  = true;
	private float gestureTime;
	private float gestureMagnitude;
	private float ballLaunchSpeed;
	private const float kSwipeMagnitude  = 1.0f;
	private Vector3 ballLaunchForceVector;
	private Vector3 ballLaunchForceVector_Mod;
	private Vector3 shotTargetPosition;
	private Camera_InputListener match_IL;
	private SoccerBall soccerBall = null;
	private LineRenderer shotDirLineRenderer;
	private MatchScenario_Test match;

	// Use this for initialization
	void Start () {
		match = GameObject.Find("MatchScenario").GetComponent<MatchScenario_Test> ();
		match_IL = GameObject.Find("GameMatch").GetComponent<Camera_InputListener> ();

		shotTargetPosition = GameObject.Find("GoalPost").transform.position;
		if( shotTarget != null ) {
			shotTargetPosition = shotTarget.transform.position;
		}
		else {
			shotTargetPosition = Vector3.zero;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (InputHandler.useTouch) {
			maxSwipeTime =  maxSwipeTime_Touch;
			shotSpeedFactor =  shotSpeedFactor_Touch;
			shotHeightFactor =  shotHeightFactor_Touch;
		}
		else {
			maxSwipeTime =  maxSwipeTime_Mouse;
			shotSpeedFactor =  shotSpeedFactor_Mouse;
			shotHeightFactor =  shotHeightFactor_Mouse;
		}

		switch( InputHandler.swipe_state ) {
		case InputHandler.SwipeState.NONE:
			break;
		case InputHandler.SwipeState.BEGIN:
			break;
		case InputHandler.SwipeState.INPROGRESS:
			renderShotDirLine = true;
			swipeDir = InputHandler.swipe_direction;
			break;
		case InputHandler.SwipeState.END:
			gestureTime = InputHandler.swipe_duration;
			gestureMagnitude = InputHandler.swipe_length;
			swipeDir = InputHandler.swipe_direction;
		
			if ( gestureTime < maxSwipeTime && gestureMagnitude > minSwipeMagnitude ) {
				takeShot = true;
			}

			renderShotDirLine = false;
			if( shotDirLineRenderer != null ) {
				Destroy (shotDirLineRenderer.gameObject, 3.0f);
			}
			break;
		}

		if( renderShotDirLine && shotTarget!=null ) {
			InitBallLaunch();
			RenderShotDirLine();
		}

		if ( takeShot  && shotTarget!=null ) {
			InitBallLaunch();
			CalcShotSpeedAndHeight();
			if( match != null ) {
				match.OnPlayerShoot();
			}

			if( enableDebugLine ) {
				//LineRenderer ballLine = Instantiate(lineRenderer, transform.position, transform.rotation) as LineRenderer;
				LineRenderer ballLine = Instantiate(lineRenderer, transform.position, Quaternion.identity) as LineRenderer;
				LineRenderer ballLine_Mod = Instantiate(lineRenderer, transform.position, Quaternion.identity) as LineRenderer;
				
				ballLine.SetColors(c1, c2);
				ballLine.SetWidth(0.1F, 0.1F);
				ballLine.SetVertexCount(2);
				ballLine.SetPosition(0, transform.position);
				ballLine.SetPosition(1, transform.position+(ballLaunchForceVector));
				
				ballLine_Mod.SetColors(c3, c4);
				ballLine_Mod.SetWidth(0.1F, 0.1F);
				ballLine_Mod.SetVertexCount(2);
				ballLine_Mod.SetPosition(0, transform.position);
				ballLine_Mod.SetPosition(1, transform.position+(ballLaunchForceVector_Mod));
				
				Destroy (ballLine.gameObject, 1.0f);
				Destroy (ballLine_Mod.gameObject, 1.0f);
			}
		}
		else {
			MoveBall();
		}
	}

	void FixedUpdate() {
		if (takeShot) {
			takeShot = false;

			TakeShot();
		}
	}

	void InitBallLaunch() {
		swipeDir.Normalize();
		switch (match_IL.fieldView)
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
		
		Vector3 goalXZPos = shotTargetPosition;
		goalXZPos.y = transform.position.y;
		Vector3 playerToGoalDir = goalXZPos - transform.position;
		playerToGoalDir.Normalize ();
		if( enableDebugLine ) {
			//LineRenderer ballLine = Instantiate(lineRenderer, transform.position, transform.rotation) as LineRenderer;
			LineRenderer playerToGoalDir_Line = Instantiate(lineRenderer, transform.position, Quaternion.identity) as LineRenderer;
			
			playerToGoalDir_Line.SetColors(Color.green, Color.green);
			playerToGoalDir_Line.SetWidth(0.1F, 0.1F);
			playerToGoalDir_Line.SetVertexCount(2);
			playerToGoalDir_Line.SetPosition(0, transform.position);
			playerToGoalDir_Line.SetPosition(1, transform.position+(playerToGoalDir*30.0f));

			Destroy (playerToGoalDir_Line.gameObject, 1.0f);
		}
		
		//Vector2 swipeDir_Up = Vector2.up;
		Vector2 swipeDir_Up = new Vector2(playerToGoalDir.x, playerToGoalDir.z);
		swipeDir_Up.Normalize();
		float shotLerpOffset = 0.0f;
		switch (match_IL.fieldView)
		{
		case TypeOfFieldView.BACK_VIEW :
			shotLerpOffset = shotLerpOffsets[0];
			break;
		case TypeOfFieldView.SIDE_VIEW_LEFT :
			shotLerpOffset = shotLerpOffsets[1];
			break;
		case TypeOfFieldView.SIDE_VIEW_RIGHT :
			shotLerpOffset = shotLerpOffsets[2];
			break;
		}
		swipeDir_Mod = Vector2.Lerp(swipeDir, swipeDir_Up, shotLerpOffset);
	}

	void RenderShotDirLine () {

		shotDirLineRenderer = (shotDirLineRenderer == null) ? Instantiate(lineRenderer, transform.position, Quaternion.identity) as LineRenderer: shotDirLineRenderer;

		Color col = this.GetComponent<GameplayGUI> ().currColor;
		shotDirLineRenderer.SetColors(col, col);

		//shotDirLineRenderer.SetColors(Color.cyan, Color.cyan);
		shotDirLineRenderer.SetWidth(0.2F, 0.2F);
		shotDirLineRenderer.SetVertexCount(2);
		Vector3 shotDir_Line_StartPos = transform.position;
		shotDir_Line_StartPos.y = transform.position.y - 1.0f;
		shotDirLineRenderer.SetPosition(0, shotDir_Line_StartPos);
		Vector3 shotDirVector;
		shotDirVector.x = swipeDir_Mod.x * shotDirLineLength;
		shotDirVector.y = 0.0f;
		shotDirVector.z = swipeDir_Mod.y * shotDirLineLength;
		shotDirLineRenderer.SetPosition(1, shotDir_Line_StartPos+(shotDirVector));
	}
	
	void CalcShotSpeedAndHeight () {
		// Shooting feature can be developed using both swipe magnitude and swipe time
		// for simplicity of controls, we are using only one which would be swipe time
		if( kUseConstantSwipeMagnitude ) {
			gestureMagnitude = kSwipeMagnitude;
		}
		if( useConstantSwipeTime ) {
			gestureTime = constantSwipeTime;
		}
		gestureTime = (gestureTime > maxSwipeTime) ? maxSwipeTime : gestureTime;
		Utility.DebugLog ("gestureTime == " + gestureTime, enableDebugText);
		
		// determine ball speed
		// gesture time == power
		// speed = gesture time * tune factor
		ballLaunchSpeed = gestureMagnitude * gestureTime * shotSpeedFactor;
		string speedDebugStr;
		speedDebugStr = "Ball Speed = " + ballLaunchSpeed;

		if( ballLaunchSpeed > MAX_BALLSPEED ) {
			speedDebugStr += ", MaxSpeed Hit!";
			ballLaunchSpeed = MAX_BALLSPEED;
		}
		Utility.DebugLog (speedDebugStr, enableDebugText);
		
		// determine ball shot height
		float upForce = 1.0f;
		upForce = gestureTime * shotHeightFactor;
		if( upForce > MAX_UPFORCE ) {
			Utility.DebugLog ("MaxUPForce Hit!", enableDebugText);
			upForce = MAX_UPFORCE;
		}
		
		
		ballLaunchForceVector.x = swipeDir.x * ballLaunchSpeed;
		ballLaunchForceVector.y = upForce;
		ballLaunchForceVector.z = swipeDir.y * ballLaunchSpeed;
		ballLaunchForceVector_Mod.x = swipeDir_Mod.x * ballLaunchSpeed;
		ballLaunchForceVector_Mod.y = upForce;
		ballLaunchForceVector_Mod.z = swipeDir_Mod.y * ballLaunchSpeed;
	}

	void TakeShot () {
		if( createBallFromBC ) {
			Rigidbody shot = Instantiate(ballProjectile, transform.position, transform.rotation) as Rigidbody;
			shot.freezeRotation = true;
			Physics.IgnoreCollision(gameObject.collider, shot.collider);
			Physics.IgnoreLayerCollision(shot.gameObject.layer, shot.gameObject.layer);
			shot.AddForce(ballLaunchForceVector_Mod, ForceMode.Impulse);
			
			Destroy (shot.gameObject, 3.0f);
		}
		else {
			if (soccerBall) {
				soccerBall.ballTrigger.DisableTrigger(NumOfUpdatesForShooting);
				soccerBall.rigidbody.velocity = Vector3.zero;
				soccerBall.rigidbody.angularVelocity = Vector3.zero;
				soccerBall.rigidbody.AddForce(ballLaunchForceVector_Mod, ForceMode.Impulse);

				soccerBall = null;
				isPlayerInPossessionOfABall = false;
			}
		}

		//Debug.DrawLine(transform.position, transform.position+(ballLaunchForceVector), Color.red, 2, false);
	}
	
	public void OnGetBall (SoccerBall _soccerBall) {
		soccerBall = _soccerBall;
		
		soccerBall.transform.position = parentedSoccerBallTransform.position;
		soccerBall.transform.rotation = transform.rotation;
		soccerBall.rigidbody.freezeRotation = true;

		isPlayerInPossessionOfABall = true;
		Physics.IgnoreCollision(gameObject.collider, soccerBall.collider);
	}
	
	public void MoveBall () {
		//Snap Soccer Ball
		if (soccerBall) {
			soccerBall.transform.position = parentedSoccerBallTransform.position;
		}
	}
	
	public bool IsPlayerInPossessionOfABall () {
		return isPlayerInPossessionOfABall;
	}
}
