using UnityEngine;
using System.Collections;

public enum TypeOfFieldView{
	BACK_VIEW,
	SIDE_VIEW
}

public class InputListener : MonoBehaviour {

	
	[SerializeField] private TypeOfFieldView fieldView = TypeOfFieldView.BACK_VIEW;
	[SerializeField] private PlayerNavigation pn = null;
	[SerializeField] private MobileControl mc = null;

	void Awake() {
		Camera.main.GetComponent<GameCamera> ().fieldView = this.fieldView;
	}
	
	void Update () {
		if (mc) {
			if(fieldView == TypeOfFieldView.BACK_VIEW) MoveInput(mc.normal);
			else MoveInput(new Vector2(mc.normal.y,-mc.normal.x));
		} else {
			MoveInput (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")));
		}
	}

	void MoveInput (Vector2 dir) {
		pn.Navigate (dir);
	}
}
