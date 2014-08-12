using UnityEngine;
using System.Collections;

public class InputListener : MonoBehaviour {

	[SerializeField] private PlayerNavigation pn = null;
	[SerializeField] private MobileControl mc = null;
	
	void Update () {
		if (mc) {
			MoveInput(mc.normal);
		} else {
			MoveInput (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")));
		}
	}

	void MoveInput (Vector2 dir) {
		pn.Navigate (dir);
	}
}
