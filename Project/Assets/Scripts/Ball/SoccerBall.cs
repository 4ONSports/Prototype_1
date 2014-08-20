using UnityEngine;
using System.Collections;

public class SoccerBall : MonoBehaviour {

	private bool IsInPossession {
		get;
		set;
	}
	public BallTrigger ballTrigger;

	void Start() {
		IsInPossession = false;
	}
	
	public void OnPlayerTouch(BallController bc) {
		bc.OnGetBall(this);
	}

	/*
	public void Kick(){
		StartCoroutine (KickCoroutine());
	}

	IEnumerator KickCoroutine() {
		Activate ();
		yield return new WaitForSeconds(0.2f);
		IsInPossession = false;
	}

	public void Activate() {
		this.rigidbody.WakeUp();
		this.collider.enabled = true;
	}

	public void Deactivate() {
		this.rigidbody.Sleep();
		this.collider.enabled = false;
	}
	*/
}
