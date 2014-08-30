using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {
	
	private MatchScenario match;
	
	void Start () {
		match = (GameObject.Find("MatchScenario") != null)? GameObject.Find("MatchScenario").GetComponent<MatchScenario> (): null;
		//Physics.IgnoreLayerCollision(9, 10);
	}

	void OnTriggerEnter(Collider c) {
		//SoccerBall sBall = c.GetComponent<SoccerBall> ();
		if ( c.tag == "Ball" ) {
			//sBall.rigidbody.velocity = Vector3.zero;
			//sBall.rigidbody.angularVelocity = Vector3.zero;
			match.OnGoalScored();
		}
	}
}
