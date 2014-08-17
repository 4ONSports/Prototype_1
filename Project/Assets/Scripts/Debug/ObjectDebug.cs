using UnityEngine;
using System.Collections;

public class ObjectDebug : MonoBehaviour {

	[SerializeField] private DebugFeature myDebugFeature = DebugFeature.DEBUG_CAMERA_MIDDLE_POINT;

	void Start () {
		if (!GameDebug.CheckIfActiveFeature (myDebugFeature)) {
			Destroy(this.gameObject);
		}
	}

}
