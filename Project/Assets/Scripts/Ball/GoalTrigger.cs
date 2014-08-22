using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {
	
	void OnTriggerEnter(Collider c) {
		SoccerBall sBall = c.GetComponent<SoccerBall> ();
		if ( c.tag == "Ball" ) {
			sBall.rigidbody.velocity = Vector3.zero;
			sBall.rigidbody.angularVelocity = Vector3.zero;
		}
	}
}
