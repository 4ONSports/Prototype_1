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
		if (c.tag == "Player"&& !IsInPossession) {
			Deactivate();
			c.GetComponent<PlayerNavigation>().OnGetBall(this);
		}
	}

	public void Kick(){
		StartCoroutine (KickCoroutine());
	}

	IEnumerator KickCoroutine() {
		Activate ();
		yield return new WaitForSeconds(0.2f);
		IsInPossession = false;
	}

	void Activate() {
		this.rigidbody.WakeUp();
		this.collider.enabled = true;
	}

	void Deactivate() {
		this.rigidbody.Sleep();
		this.collider.enabled = false;
	}
}
