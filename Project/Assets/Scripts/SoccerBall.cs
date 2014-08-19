using UnityEngine;
using System.Collections;

public class SoccerBall : MonoBehaviour {

	private bool IsInPossession {
		get;
		set;
	}

	void Start() {
		IsInPossession = false;
	}

	void OnTriggerEnter(Collider c) {
		BallController bc = c.GetComponent<BallController> ();
		if ( c.tag == "Player" && !bc.IsPlayerInPossessionOfABall() ) {
			//Deactivate();
			bc.OnGetBall(this);
			//Debug.Log ("Ball In Possession");
		}
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
