using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerNavigation : MonoBehaviour {

	[SerializeField] private float runSpeed = 10f;
	[SerializeField] private float runAcceleration = 2f;
	[SerializeField] private float turnSpeed = 5f;
	
	public bool enable_movement = true;
	public GameObject noMovementTrigger;

	private float acceleration = 0;
	private float gravity = 20;
	private CharacterController controller = null;
	private Vector3 moveDirection = Vector3.zero;
	private Vector2 lastDirection = Vector2.zero;

	void Start() {
		controller = this.GetComponent<CharacterController>();
	}

	public void Navigate(Vector2 _direction) {
		if( !enable_movement ) {
			return;
		}
		
		if (_direction == Vector2.zero){
			acceleration -= Time.deltaTime * runAcceleration;
			_direction = lastDirection;
		} else {
			acceleration += Time.deltaTime * runAcceleration;
			lastDirection = _direction;
		}
		acceleration = Mathf.Clamp (acceleration, 0, 1);

		//Move
		moveDirection = new Vector3 (_direction.x * acceleration,-gravity*Time.deltaTime,_direction.y * acceleration);
		controller.Move(moveDirection * runSpeed * Time.deltaTime);

		//Rotate
		Vector3 newDir = Vector3.RotateTowards (transform.forward,new Vector3(_direction.x,0,_direction.y),turnSpeed * Time.deltaTime,0f);
		transform.rotation = Quaternion.LookRotation (newDir);
	}
	
	
	void OnTriggerEnter(Collider other) {
		if( other.gameObject == noMovementTrigger ) {
			enable_movement = false;
		}
	}
}