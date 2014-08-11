using UnityEngine;
using System.Collections;

public class ChaserAI : MonoBehaviour {

	// Chaser will check for player in vicinity/vision range
	// Chaser will move forward and bump off walls if no player around

	public float moveSpeed;
	public int moveAngle;
	public int visionRange = 5;

	// Use this for initialization
	void Start () {
		transform.Rotate (0,Time.deltaTime,0, Space.Self);
	}
	
	// Update is called once per frame
	void Update () {
		//Kaue here, had to comment out, but transform.Translate takes a Vector3. transform.rotation is a Quaternion.
		//transform.Translate (transform.rotation * Time.deltaTime * moveSpeed, Space.World);
	}
}
