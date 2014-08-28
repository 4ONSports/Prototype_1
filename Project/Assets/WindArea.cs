using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindArea : MonoBehaviour {

	[SerializeField] private Vector3 direction = Vector3.up;
	[SerializeField] private float windForce = 10;
	private List<Rigidbody> bodies = new List<Rigidbody>();

	void Start() {
		direction.Normalize ();
	}

	void OnTriggerEnter (Collider c) {
		if (c.rigidbody) bodies.Add (c.rigidbody);
	}

	void OnTriggerExit (Collider c) {
		if (c.rigidbody) bodies.Remove (c.rigidbody);
	}

	void FixedUpdate () {
		if (bodies.Count > 0) {
			foreach (Rigidbody body in bodies) {
				body.AddForce(direction * windForce);
			}
		}
	}
}
