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
	private InputListener match_IL;
	private Dictionary<TypeOfFieldView, GameCameraBehaviour> cameraBehaviours = new Dictionary<TypeOfFieldView, GameCameraBehaviour>();

	void Start() {
		match_IL = GameObject.Find("GameMatch").GetComponent<InputListener> ();
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

	public Vector3 GetMediumPoint() {
		Vector3 sum = Vector3.zero;
		//TODO: Get rid of this if
		if(gc.targetTransforms.Count > 1) {
			foreach (Transform t in gc.targetTransforms) {
				sum += t.transform.position;
			}
			return sum * 0.5f;
		} else {
			return gc.targetTransforms[0].transform.position;
		}
	}

	public void OnDebugMiddlePoint() {
		gc.debugTransform.position = GetMediumPoint ();
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
		gc.transform.position = Vector3.MoveTowards (gc.transform.position,GetMediumPoint()  + offset,10f* Time.deltaTime);
	}
}

//SIDE LEFT BEHAVIOUR
public class GameCameraBehaviourSideLeft : GameCameraBehaviour {
	
	public GameCameraBehaviourSideLeft (GameCamera _gc) {
		this.gc = _gc;
	}

	override public void OnCameraSwitch() {
		gc.transform.rotation = Quaternion.Euler(gc.side_View_Left.x, gc.side_View_Left.y, gc.side_View_Left.z);
		offset = gc.offsetSide_Left;
	}

	override public void OnCameraLogic() {
		gc.transform.position = Vector3.MoveTowards (gc.transform.position,GetMediumPoint()  + offset,10f* Time.deltaTime);
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
		gc.transform.position = Vector3.MoveTowards (gc.transform.position,GetMediumPoint()  + offset,10f* Time.deltaTime);
	}
}