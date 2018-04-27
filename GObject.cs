using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GObject : MonoBehaviour {
	//GO objects need a rigid body do 2d detection
	public bool isDraggable = false;
	public bool isDragged = false;
	public GameObject mount;
	public int price = 0;
	public abstract void onClickBegan ();
	public abstract void onClickRelease ();
	public abstract void buttonAction();


	protected void mouseDown(){
		Debug.Log ("mouse is down");
		if (isDraggable) {
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (false);
			isDragged = true;
		}

	}



	protected void mouseUp(){
		if (isDraggable) {
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (true);
			isDragged = false;
			if (mount == null) {


			}
		}

	}
		
	
	// Update is called once per frame
	protected void Update () {
		if (isDragged) {
			Debug.Log ("dragging");
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
		}
	}
}
