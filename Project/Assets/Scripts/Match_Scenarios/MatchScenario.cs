using UnityEngine;
using System.Collections;

public class MatchScenario : MonoBehaviour {

	void Start () {
	}

	void Update () {
	}
	
	public void OnPlayerShoot() {
		_OnPlayerShoot ();
	}

	public void OnGoalScored() {
		_OnGoalScored ();
	}
	
	public void OnTargetHit() {
		_OnTargetHit ();
	}
	
	protected virtual void _OnPlayerShoot() {
	}
	
	protected virtual void _OnGoalScored() {
	}
	
	protected virtual void _OnTargetHit() {
	}
}
