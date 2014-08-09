using UnityEngine;
using System.Collections;

public class FakePlayer : MonoBehaviour {

	public int degPerSec = 20;
	public Transform rotateAround;

	void Start() {
	}

	void Update() {
		transform.RotateAround(Vector3.up, Vector3.up, degPerSec * Time.deltaTime);
	}

}
