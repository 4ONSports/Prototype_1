using UnityEngine;
using System.Collections;

public class MouseKick : MonoBehaviour {
	
	public float xSpeed = 1F;
	public float zSpeed = 1F;
	private float x;
	private float z;
	private bool mouseClicked;
	public float mouseSensitivity = 2.0F;

	// Use this for initialization
	void Start () {
	
	}

	void Update () {

		mouseClicked = Input.GetMouseButtonDown(0);
		while (mouseClicked == true) {
			print ("MouseClicked");
			x = xSpeed * Input.GetAxis ("Horizontal");
			z = zSpeed * Input.GetAxis ("Vertical");
			
			print ("Kicking " + x + " " + z);
			
			transform.Translate (x, 0, z);

			mouseClicked = Input.GetMouseButtonDown(0);
		}


	}

}
