using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {
	public static Vector2 Rotate2D( Vector2 v, float angle )
	{
		float radAngle = angle * Mathf.Deg2Rad;
		float sin = Mathf.Sin( radAngle );
		float cos = Mathf.Cos( radAngle );

		float tx = (cos * v.x) - (sin * v.y);
		float ty = (cos * v.y) + (sin * v.x);

		return new Vector2 (tx, ty);
	}

	public static void DebugLog( string str, bool bShow=false ) {
		if( bShow ) {
			Debug.Log (str+":::"+bShow);
		}
	}
}
