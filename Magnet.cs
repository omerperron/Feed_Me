using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Magnet : GObject {
	Vector3 fanDirection;
	bool rotating = false;
	Vector3 clickPosition = Vector3.zero;
	bool clickedOn = false;
	// Use this for initialization
	void Start () {
		//this.fanDirection = new Vector3 (0, 0.1f, 0);
	}


	// Update is called once per frame
	void Update () {
		base.Update ();
		if(rotating) rotate ();
	}

	void OnTriggerStay2D(Collider2D col){
		float zAngle = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
//		Debug.Log ("angle: " + zAngle);
		float x = Mathf.Cos(zAngle);
		float y = Mathf.Sin (zAngle);
		//Vector3 vec = 
		if (col.attachedRigidbody)
			col.attachedRigidbody.AddForce(new Vector3(-x, -y, 0) * (12 + Random.Range(-2, 2)));
		
	}

	void rotate(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 vec = mousePos - (Vector2) gameObject.transform.position;
		vec.Normalize();

		float angle = vec.y / vec.x;
		float fAngle = (Mathf.Atan (angle) * Mathf.Rad2Deg);

		if (vec.x < 0)fAngle += 180;
		
		if(!float.IsNaN(fAngle))
			gameObject.transform.rotation = Quaternion.Euler(0, 0, fAngle);

	}

	public override void onClickBegan (){
		Vector3 clickPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 position = transform.position;
		float distance = Vector2.Distance (position, clickPosition);
		Debug.Log ("down" + distance);
		if (distance > 1.4f) {
			return;
			clickedOn = false;
		}

		clickedOn = true;
		base.mouseDown ();

		if (!GameControl.controller.isPaused()) {
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (false);
			rotating = true;
		}
	}
	public override void onClickRelease (){
		if (!clickedOn)
			return;

		base.mouseUp ();
		if (rotating == true) {
			rotating = false;
			clickPosition = Vector3.zero;
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (true);
		}
		clickedOn = false;
	}

	public override void buttonAction(){

	}


	
}
