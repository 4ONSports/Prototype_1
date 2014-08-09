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
		transform.Translate (transform.rotation * Time.deltaTime * moveSpeed, Space.World);
	}
}
