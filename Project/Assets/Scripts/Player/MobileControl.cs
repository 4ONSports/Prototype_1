using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]

public class MobileControl : MonoBehaviour {

	private Vector3 screenPoint = Vector3.zero;
	private bool useTouch = false;

	[SerializeField] private Camera uiCamera = null;
	[SerializeField] Transform joystickTop  = null;

	public Vector3 normal;

	void Start () {
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			useTouch = true;
		}
	}

	void OnMouseDrag() 	{
		Vector3 curScreenPoint = Vector3.zero;
		if ( !useTouch ) {
			curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		} else {
			Touch touch = Input.GetTouch(0);
			curScreenPoint = new Vector3(touch.position.x, touch.position.y, screenPoint.z);
		}
		Vector3 curPosition = uiCamera.ScreenToWorldPoint(curScreenPoint);
		joystickTop.position = new Vector3 (curPosition.x,curPosition.y,joystickTop.position.z);
		normal = Vector3.Normalize (joystickTop.localPosition);
		joystickTop.localPosition = normal * 0.5f;
	}
	
	void OnMouseUp() {
		joystickTop.localPosition = Vector3.zero;
		normal = Vector3.zero;
	}
}
