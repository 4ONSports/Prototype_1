using UnityEngine;
using System.Collections;

public class BallTrigger : MonoBehaviour {
	
	public SoccerBall soccerball;
	private int disableTriggerFrameCount;
	
	void Update () {
		// Re-enable trigger after frame count has passed
		if( disableTriggerFrameCount > 0 ) {
			--disableTriggerFrameCount;

			if( disableTriggerFrameCount == 0 ) {
				this.collider.enabled = true;
			}
		}
	}

	void OnTriggerEnter(Collider c) {
		BallController bc = c.GetComponent<BallController> ();
		if ( c.tag == "Player" && !bc.IsPlayerInPossessionOfABall() ) {
			soccerball.OnPlayerTouch(bc);
		}
	}

	public void DisableTrigger( int numFrames ) {
		this.collider.enabled = false;
		disableTriggerFrameCount = numFrames;
	}
}
