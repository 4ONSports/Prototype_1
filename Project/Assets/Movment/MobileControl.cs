using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]

public class MobileControl : MonoBehaviour {

	private Vector3 screenPoint = Vector3.zero;
	private Vector3 offset;

	[SerializeField] private Camera uiCamera = null;
	[SerializeField] Transform joystickTop  = null;

	public Vector3 normal;

	void OnMouseDown() {
		offset = joystickTop.position - uiCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));	
	}

	void OnMouseDrag() 	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = uiCamera.ScreenToWorldPoint(curScreenPoint) + offset;
		joystickTop.position = new Vector3 (curPosition.x,curPosition.y,joystickTop.position.z);
		normal = Vector3.Normalize (joystickTop.localPosition);
		joystickTop.localPosition = normal * 0.5f;
	}
	
	void OnMouseUp() {
		joystickTop.localPosition = Vector3.zero;
		normal = Vector3.zero;
	}
}
