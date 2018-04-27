using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleObject : MonoBehaviour {


	//starting position of the object
	private Vector3 originalPos;
	private Vector3 originalRot;
	private Vector3 originalScale;

	//temporary variables used to find the position we want to move the object to
	private Vector3 wantedPos;
	private Vector3 wantedRot;
	private Vector3 wantedScale;

	//the transformations the object undergoes when the button is pressed
	public Vector3 transformation;
	public Vector3 rotate;
	public Vector3 scale;


	// Use this for initialization
	void Start () {
		//initialise original and wanted variables
		originalPos = transform.position;
		originalRot = transform.rotation.eulerAngles;
		originalScale = transform.localScale;
		wantedPos = transform.position;
		wantedRot = transform.rotation.eulerAngles;
		wantedScale = transform.localScale;

	}


	// Update is called once per frame
	void Update () {
		move ();
	}

	//if the button was hit performAction is called
	public void performAction(bool forward){
		if (forward) {
			wantedPos = originalPos + transformation;

			Vector3 rotationVector = originalRot;
			rotationVector.z += rotate.z;
			wantedRot = rotationVector;
			////gameObject.transform.rotation = Quaternion.Euler(0, 0, rotationVector.z);
			wantedScale = new Vector3(originalScale.x * scale.x, originalScale.y * scale.y, originalScale.z * scale.z);
		} else if (!forward) {
			wantedPos = originalPos;
			wantedRot = originalRot;
			wantedScale = originalScale;
			
		} else {
			Debug.Log ("no idea what was passed in");
		}
		
	}

	//moves the object towards the wanted position, rotation and scale
	void move(){
		transformObj ();
		rotateObj ();
		scaleObj ();
	}

	//update position
	private void transformObj(){
		if (Vector3.Distance (wantedPos, transform.position) > Mathf.Epsilon) {
			float distanceX = wantedPos.x - transform.position.x;
			float distanceY = wantedPos.y - transform.position.y;
			Vector3 newPos = new Vector3 (transform.position.x + distanceX * Time.deltaTime, transform.position.y + distanceY * Time.deltaTime, transform.position.z);
			transform.position =  newPos;

		}
	}

	//update rotation
	private void rotateObj(){
		Vector3 currentRotation = gameObject.transform.rotation.eulerAngles;
		if (Mathf.Abs(wantedRot.z - currentRotation.z) > Mathf.Epsilon) {

			float rotZBack = currentRotation.z - wantedRot.z;
			float rotZForward = (360 - currentRotation.z) + wantedRot.z;
			if (rotZForward <= rotZBack) {
				transform.rotation = Quaternion.Euler (0, 0, currentRotation.z + rotZForward * Time.deltaTime);

			} else {
				transform.rotation = Quaternion.Euler (0, 0, currentRotation.z - rotZBack * Time.deltaTime);

			}


		}
	}

	//need to fix this
	private void scaleObj(){

		Vector3 currentScale = transform.localScale;
		if (Mathf.Abs(wantedScale.x - currentScale.x) > Mathf.Epsilon ||
			Mathf.Abs(wantedScale.y - currentScale.y) > Mathf.Epsilon) {
			
			float scaleX = wantedScale.x - currentScale.x;
			float scaleY = wantedScale.y - currentScale.y;
			scaleX =currentScale.x + scaleX * Time.deltaTime;
			scaleY =currentScale.y + scaleY * Time.deltaTime;
			transform.localScale = new Vector3 (scaleX, scaleY, 1);
		}
	}


}
