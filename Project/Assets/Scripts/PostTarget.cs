using UnityEngine;
using System.Collections;

public class PostTarget : MonoBehaviour {

	public GameObject spawnpoint;

	private GameObject ball;
	private MatchScenario match;
	private Vector3 initPosition;
	private Quaternion initLocalRotation;
	private Quaternion initRotation;

	void Start () {
		ball = GameObject.Find("Ball");
		match = (GameObject.Find("MatchScenario") != null)? GameObject.Find("MatchScenario").GetComponent<MatchScenario> (): null;
		initPosition = transform.localPosition;
		initRotation = transform.rotation;
		initLocalRotation = transform.localRotation;
	}

	void OnCollisionEnter(Collision collision) {
		if( collision.gameObject == ball ) {
			match.OnTargetHit();
		}
	}

	public void Reset() {
		//transform.localPosition = initPosition;
		//transform.rotation = initRotation;
		transform.localRotation = initLocalRotation;

//		rigidbody.velocity = Vector3.zero;
//		rigidbody.angularVelocity = Vector3.zero;
//		rigidbody.inertiaTensor = Vector3.zero;
//		rigidbody.inertiaTensorRotation = Quaternion.identity;
	}
}
