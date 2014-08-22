using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DebugFeature {
	DEBUG_CAMERA_MIDDLE_POINT
}

public class GameDebug : MonoBehaviour {

	[SerializeField] private bool debugCameraMiddlePoint = false;
	 
	private static Dictionary<DebugFeature, bool> activeDebugFeatures = new Dictionary<DebugFeature, bool>()
	{
		{DebugFeature.DEBUG_CAMERA_MIDDLE_POINT, false},
	};

	void Awake() {
		activeDebugFeatures.Clear ();
		activeDebugFeatures.Add(DebugFeature.DEBUG_CAMERA_MIDDLE_POINT,debugCameraMiddlePoint);
	}

	public static bool CheckIfActiveFeature(DebugFeature feature) {
		return activeDebugFeatures [feature];
	}
}
