using UnityEngine;
using System.Collections;

public class ScenarioBox : MonoBehaviour {
	
	private GameObject ball;

	// Use this for initialization
	void Start () {
		ball = GameObject.Find("Ball");
	}
	
	void OnCollisionEnter(Collision collision) {
		if( collision.gameObject == ball ) {
			Destroy(this.gameObject, 2.0f);
		}
	}
}
