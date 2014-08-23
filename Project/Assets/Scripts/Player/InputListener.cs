using UnityEngine;
using System.Collections;

public enum TypeOfFieldView{
	BACK_VIEW,
	SIDE_VIEW_LEFT,
	SIDE_VIEW_RIGHT,
}

public class InputListener : MonoBehaviour {

	
	[SerializeField] public TypeOfFieldView fieldView = TypeOfFieldView.BACK_VIEW;
	[SerializeField] private PlayerNavigation pn = null;
	[SerializeField] private MobileControl mc = null;

	void Awake() {
		//Camera.main.GetComponent<GameCamera> ().fieldView = this.fieldView;
	}
	
	void Update () {
		if (mc) {
			switch (fieldView)
			{
			case TypeOfFieldView.BACK_VIEW :
				MoveInput(mc.normal);
				break;
			case TypeOfFieldView.SIDE_VIEW_LEFT :
				MoveInput(new Vector2(mc.normal.y, -mc.normal.x));
				break;
			case TypeOfFieldView.SIDE_VIEW_RIGHT :
				MoveInput(new Vector2(-mc.normal.y, mc.normal.x));
				break;
			}

			//if(Input.GetKeyDown(KeyCode.Space)) pn.Kick();
		} else {
			MoveInput (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")));
		}
	}

	void MoveInput (Vector2 dir) {
		pn.Navigate (dir);
	}

	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width - 150, 0, 150, 150), "InputListener.cs Restart()"))
						Restart ();
	}

	void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}
}
