using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerNavigation : MonoBehaviour {

	[SerializeField] private float runSpeed = 10f;
	[SerializeField] private float turnSpeed = 5f;

	private CharacterController controller = null;
	private Vector3 moveDirection = Vector3.zero;

	void Start() {
		controller = this.GetComponent<CharacterController>();
	}

	public void Navigate(Vector2 _direction) {

		//Move
		moveDirection = new Vector3 (_direction.x,0,_direction.y);
		controller.Move(moveDirection * runSpeed * Time.deltaTime);

		//Rotate
		Vector3 newDir = Vector3.RotateTowards (transform.forward,new Vector3(_direction.x,0,_direction.y),turnSpeed * Time.deltaTime,0f);
		transform.rotation = Quaternion.LookRotation (newDir);
	}
}