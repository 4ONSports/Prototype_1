using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerNavigation : MonoBehaviour {

	[SerializeField] private float runSpeed = 10f;
	[SerializeField] private float turnSpeed = 5f;
	[SerializeField] private Transform parentedSoccerBallTransform = null;

	private CharacterController controller = null;
	private Vector3 moveDirection = Vector3.zero;
	private SoccerBall soccerBall = null;

	void Start() {
		controller = this.GetComponent<CharacterController>();
	}

	public void Navigate(Vector2 _direction) {

		//Move
		moveDirection = new Vector3 (_direction.x,-10*Time.deltaTime,_direction.y);
		controller.Move(moveDirection * runSpeed * Time.deltaTime);

		//Rotate
		Vector3 newDir = Vector3.RotateTowards (transform.forward,new Vector3(_direction.x,0,_direction.y),turnSpeed * Time.deltaTime,0f);
		transform.rotation = Quaternion.LookRotation (newDir);

		//Snap Soccer Ball
		if (soccerBall) {
			soccerBall.transform.position = parentedSoccerBallTransform.position;
		}
	}

	public void OnGetBall (SoccerBall _soccerBall) {
		this.soccerBall = _soccerBall;
	}

	public void Kick() {
		if(!soccerBall)return;
		soccerBall.Kick ();
		soccerBall = null;
	}
}