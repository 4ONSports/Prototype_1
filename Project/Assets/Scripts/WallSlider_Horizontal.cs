using UnityEngine;
using System.Collections;

public class WallSlider_Horizontal : MonoBehaviour {
	
	public Transform startTransform;
	public Transform endTransform;
	public GameObject ball;
	public Vector3 moveDir = Vector3.zero;
	public float moveSpeed = 1.0f;

	private bool move = true;
	private Vector3 init_Position;
	private Quaternion init_Rotation;
	
	void Start () {
		init_Position = transform.position;
		init_Rotation = transform.rotation;
		if( startTransform != null && endTransform != null ) {
			moveDir = endTransform.position - startTransform.position;
			moveDir.Normalize ();
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if( other.gameObject == endTransform.gameObject || other.gameObject == startTransform.gameObject ) {
			moveDir *= -1.0f;
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if( collision.gameObject == ball.gameObject ) {
			move = false;
		}
	}

	void FixedUpdate() {
		if( move ) {
			rigidbody.MovePosition(rigidbody.position + moveDir * moveSpeed * Time.deltaTime);
		}
	}

	public void Reset() {
		transform.position = init_Position;
		transform.rotation = init_Rotation;
		
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		move = true;
	}
}
