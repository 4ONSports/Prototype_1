using UnityEngine;
using System.Collections;

public class Debug_GUI : MonoBehaviour {
	
	private InputListener match_IL;
	private BallController playerBC;

	// Use this for initialization
	void Start () {
		match_IL = GameObject.Find("GameMatch").GetComponent<InputListener> ();
		playerBC = GameObject.Find("Player").GetComponent<BallController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.Box(new Rect(10,10,200,180), "Debug Menu");

		if(GUI.Button(new Rect(20,40,160,40), "Back Cam")) {
			match_IL.fieldView = TypeOfFieldView.BACK_VIEW;
		}

		if(GUI.Button(new Rect(20,80,160,40), "Side_R Cam")) {
			match_IL.fieldView = TypeOfFieldView.SIDE_VIEW_RIGHT;
		}
		
		if(GUI.Button(new Rect(20,120,160,40), "Toggle Debug Line")) {
			playerBC.enableDebugLine = !playerBC.enableDebugLine;
		}
	}
}
