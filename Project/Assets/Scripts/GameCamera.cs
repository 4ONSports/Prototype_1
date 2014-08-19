using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameCamera : MonoBehaviour {

	[SerializeField] private Transform debugTransform = null;
	[SerializeField] private List<Transform> targetTransforms = null;
	[SerializeField] private Vector3 offsetBack = new Vector3(0,7,-9);
	[SerializeField] private Vector3 offsetSide_Left = new Vector3(-14,7,0);
	[SerializeField] private Vector3 offsetSide_Right = new Vector3(14,7,0);
	[SerializeField] private Vector3 back_View = new Vector3(30,0,0);
	[SerializeField] private Vector3 side_View_Left = new Vector3(30,90,0);
	[SerializeField] private Vector3 side_View_Right = new Vector3(30,-90,0);

	private Vector3 offset;
	private InputListener match_IL;


	void Start() {
		match_IL = GameObject.Find("GameMatch").GetComponent<InputListener> ();
	}
	
	void Update () {
	}

	void LateUpdate () {
		//TODO:Remove this switch
		switch(match_IL.fieldView) {
		case TypeOfFieldView.BACK_VIEW:
			offset = offsetBack;
			transform.rotation = Quaternion.Euler(back_View.x, back_View.y, back_View.z);
			break;
		case TypeOfFieldView.SIDE_VIEW_LEFT:
			offset = offsetSide_Left;
			transform.rotation = Quaternion.Euler(side_View_Left.x, side_View_Left.y, side_View_Left.z);
			break;
		case TypeOfFieldView.SIDE_VIEW_RIGHT:
			offset = offsetSide_Right;
			transform.rotation = Quaternion.Euler(side_View_Right.x, side_View_Right.y, side_View_Right.z);
			break;
		}


		Vector3 mediumPoint = Vector3.zero;
		Vector3 sum = Vector3.zero;
		//TODO: Get rid of this if
		if(targetTransforms.Count > 1) {
			foreach (Transform t in targetTransforms) {
				sum += t.transform.position;
			}
			mediumPoint = sum * 0.5f;
		} else {
			mediumPoint = targetTransforms[0].transform.position;
		}
		if(GameDebug.CheckIfActiveFeature(DebugFeature.DEBUG_CAMERA_MIDDLE_POINT)) debugTransform.position = mediumPoint;
		transform.position = Vector3.MoveTowards (transform.position,mediumPoint  + offset,10f* Time.deltaTime);
	}
}
