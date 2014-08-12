using UnityEngine;
using System.Collections;

public class DragJoystick : MonoBehaviour {

	private Vector3 screenPoint = Vector3.zero;
	private Vector3 offset;
	private float _lockedYPosition;
	[SerializeField] private Camera uiCamera = null;

	void OnMouseDown() {
		_lockedYPosition = screenPoint.y;
		offset = gameObject.transform.position - uiCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));	
	}

	void OnMouseDrag() 	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = uiCamera.ScreenToWorldPoint(curScreenPoint) + offset;
		transform.position = new Vector3 (curPosition.x,curPosition.y,transform.position.z);
		transform.localPosition = new Vector3 (Mathf.Clamp(transform.localPosition.x,-0.5f,0.5f), Mathf.Clamp(transform.localPosition.y,-0.5f,0.5f),transform.localPosition.z);
	}
	
	void OnMouseUp() {
		transform.localPosition = Vector3.zero;
	}
}
