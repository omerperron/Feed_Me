using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : GObject {

	public GameObject companion = null;
	private Vector3 direction;
	bool rotating = false;
	bool clickedOn = false;

	// Use this for initialization
	void Start () {
		isDraggable = false;
	}

	void Update () {
		base.Update ();
		if(rotating) rotate ();
	}


	public override void onClickBegan (){
		Vector3 position = transform.position;

		clickedOn = true;
		base.mouseDown ();

		//if (!GameControl.controller.isPaused()) {
		Camera.main.GetComponent<CameraCode> ().setIsDraggable (false);
			rotating = true;
		//}
	}
	public override void onClickRelease (){
		if (!clickedOn)
			return;

		base.mouseUp ();
		if (rotating == true) {
			rotating = false;
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (false);
		}
		clickedOn = false;
	}


	public override void buttonAction(){
		
	}



	void rotate(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 vec = mousePos - (Vector2) gameObject.transform.parent.position;
		vec.Normalize();

		float angle = vec.y / vec.x;
		float fAngle = (Mathf.Atan (angle) * Mathf.Rad2Deg);

		if (vec.x < 0)fAngle += 180;
		Debug.Log (fAngle);
		if(!float.IsNaN(fAngle))
			gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, fAngle);

	}

	public GameObject getCompanion(){
		return companion;

	}
}

