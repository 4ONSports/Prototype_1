using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameCamera : MonoBehaviour {

	[HideInInspector] public TypeOfFieldView fieldView = TypeOfFieldView.BACK_VIEW;

	[SerializeField] private Transform debugTransform = null;
	[SerializeField] private List<Transform> targetTransforms = null;
	[SerializeField] private Vector3 offsetBack = new Vector3(0,7,-9);
	[SerializeField] private Vector3 offsetSide = new Vector3(-14,7,0);

	private Vector3 offset;


	void Start() {
		//TODO:Remove this switch
		switch(fieldView) {
		case TypeOfFieldView.BACK_VIEW:
			offset = offsetBack;
			transform.rotation = Quaternion.Euler(30,0,0);
			break;
		case TypeOfFieldView.SIDE_VIEW:
			offset = offsetSide;
			transform.rotation = Quaternion.Euler(30,90,0);
			break;
		}
	}


	void LateUpdate () {
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
		debugTransform.position = mediumPoint;
		transform.position = Vector3.MoveTowards (transform.position,mediumPoint  + offset,10f* Time.deltaTime);
	}
}
