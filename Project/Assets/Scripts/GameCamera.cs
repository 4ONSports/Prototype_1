using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCamera : MonoBehaviour {

	public Transform debugTransform = null;
	public List<Transform> targetTransforms = null;
	public Vector3 offsetBack = new Vector3(0,7,-9);
	public Vector3 offsetSide_Left = new Vector3(-14,7,0);
	public Vector3 offsetSide_Right = new Vector3(14,7,0);
	public Vector3 back_View = new Vector3(30,0,0);
	public Vector3 side_View_Left = new Vector3(30,90,0);
	public Vector3 side_View_Right = new Vector3(30,-90,0);

	private Vector3 offset;
	private Camera_InputListener match_IL;
	private Dictionary<TypeOfFieldView, GameCameraBehaviour> cameraBehaviours = new Dictionary<TypeOfFieldView, GameCameraBehaviour>();

	void Start() {
		match_IL = GameObject.Find("GameMatch").GetComponent<Camera_InputListener> ();
		cameraBehaviours.Add (TypeOfFieldView.BACK_VIEW,new GameCameraBehaviourBack(this));
		cameraBehaviours.Add (TypeOfFieldView.SIDE_VIEW_LEFT,new GameCameraBehaviourSideLeft(this));
		cameraBehaviours.Add (TypeOfFieldView.SIDE_VIEW_RIGHT,new GameCameraBehaviourSideRight(this));
	}

	void LateUpdate () {
		//Deal with Camera switches
		//TODO: make this triggered by something
		cameraBehaviours [match_IL.fieldView].OnCameraSwitch ();
		//Deal with Camera Logic
		cameraBehaviours [match_IL.fieldView].OnCameraLogic ();
		//Debug
		if(GameDebug.CheckIfActiveFeature(DebugFeature.DEBUG_CAMERA_MIDDLE_POINT))
			cameraBehaviours [match_IL.fieldView].OnDebugMiddlePoint ();
	}
}

public abstract class GameCameraBehaviour {
	public GameCamera gc;
	public Vector3 offset;

	abstract public void OnCameraSwitch();
	abstract public void OnCameraLogic();

	public Vector3 GetMediumPoint(List<Transform> _transforms) {
		List<Vector3> _vectors = new List<Vector3>();
		foreach(Transform t in _transforms)_vectors.Add(t.position);
		return GetMediumPoint (_vectors);
	}

	public Vector3 GetMediumPoint(List<Vector3> _vectors) {
		Vector3 sum = Vector3.zero;
		foreach (Vector3 v in _vectors) {
			sum += v;
		}
		return sum / _vectors.Count;
	}

	public void OnDebugMiddlePoint() {
		if(gc.debugTransform)
		gc.debugTransform.position = GetMediumPoint (gc.targetTransforms);
	}


}
//BACK BEHAVIOUR
public class GameCameraBehaviourBack : GameCameraBehaviour {

	public GameCameraBehaviourBack (GameCamera _gc) {
		gc = _gc;
	}

	override public void OnCameraSwitch() {
		gc.transform.rotation = Quaternion.Euler(gc.back_View.x, gc.back_View.y, gc.back_View.z);
		offset = gc.offsetBack;
	}

	override public void OnCameraLogic() {
		Vector3 avarageTarget = GetMediumPoint (gc.targetTransforms);
		gc.transform.position = Vector3.MoveTowards (gc.transform.position,avarageTarget  + offset,10 * Time.deltaTime);
		gc.transform.LookAt (GetMediumPoint(new List<Vector3>(){avarageTarget,new Vector3(0,0,55)})); 
	}
}

//SIDE LEFT BEHAVIOUR
public class GameCameraBehaviourSideLeft : GameCameraBehaviour {

	Vector3 goalPosition = new Vector3(0,0,55);

	float minDistanceCameraZoomOut = 15;
	float maxDistanceCameraZoomOut = 55;
	float rangeCameraZoomOut = 40;//max - min
	float multiplierCameraZoomOut = 2.2f;
	
	public GameCameraBehaviourSideLeft (GameCamera _gc) {
		this.gc = _gc;
	}

	override public void OnCameraSwitch() {
		gc.transform.rotation = Quaternion.Euler(gc.side_View_Left.x, gc.side_View_Left.y, gc.side_View_Left.z);
		offset = gc.offsetSide_Left;
	}

	override public void OnCameraLogic() {
		Vector3 avarageTarget = GetMediumPoint (gc.targetTransforms);
		float clampedDistance = Mathf.Clamp (Vector3.Distance (avarageTarget, goalPosition), minDistanceCameraZoomOut, maxDistanceCameraZoomOut);
		float offsetMultiplier = ((clampedDistance - minDistanceCameraZoomOut) / (rangeCameraZoomOut)) + (multiplierCameraZoomOut - 1); 
		gc.transform.position = Vector3.MoveTowards (gc.transform.position,avarageTarget  + (offset * offsetMultiplier),10 * Time.deltaTime);
	}
}

//SIDE RIGHT BEHAVIOUR
public class GameCameraBehaviourSideRight : GameCameraBehaviour {
	
	public GameCameraBehaviourSideRight (GameCamera _gc) {
		this.gc = _gc;
	}

	override public void OnCameraSwitch() {
		gc.transform.rotation = Quaternion.Euler(gc.side_View_Right.x, gc.side_View_Right.y, gc.side_View_Right.z);
		offset = gc.offsetSide_Right;
	}

	override public void OnCameraLogic() {
		gc.transform.position = Vector3.MoveTowards (gc.transform.position,GetMediumPoint(gc.targetTransforms)  + offset,10 * Time.deltaTime);
	}
}