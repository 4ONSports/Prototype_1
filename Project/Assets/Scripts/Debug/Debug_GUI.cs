using UnityEngine;
using System.Collections;

public class Debug_GUI : MonoBehaviour {
	
	private InputListener match_IL;

	// Use this for initialization
	void Start () {
		match_IL = GameObject.Find("GameMatch").GetComponent<InputListener> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.Box(new Rect(10,10,100,90), "Debug Menu");

		if(GUI.Button(new Rect(20,40,80,20), "Back Cam")) {
			match_IL.fieldView = TypeOfFieldView.BACK_VIEW;
		}

		if(GUI.Button(new Rect(20,70,80,20), "Side_R Cam")) {
			match_IL.fieldView = TypeOfFieldView.SIDE_VIEW_RIGHT;
		}
	}
}
