using UnityEngine;
using System.Collections;

public class InputListener : MonoBehaviour {

	[SerializeField] private PlayerNavigation pn = null;
	
	void Update () {
		pn.Navigate(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
	}
}
