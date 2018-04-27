using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBalloon: Balloon {

	private bool rotLeft = true;
	private int maxRotation = 10;
	private float rotationSpeed = 0.4f;
	private float floatingSpeed = 0.05f;

	protected override void Awake(){
		floatingSpeed = Random.Range(0.03f, 0.07f);
		//health = 1;
		//maxRotation = Random.Range(8, 12);
	}


	protected override void Update(){

		transform.position = new Vector3 (transform.position.x, transform.position.y + floatingSpeed);
		Vector3 rotationVector = transform.rotation.eulerAngles;
		if (rotLeft) {
			rotationVector.z += rotationSpeed;
		}else {

			rotationVector.z -= rotationSpeed;
		}
		transform.rotation = Quaternion.Euler(rotationVector);

		if (rotationVector.z > maxRotation && rotationVector.z < maxRotation + 1 && rotLeft) {
			rotLeft = false;
		} else if (rotationVector.z < 360 - maxRotation && rotationVector.z > 360 - maxRotation-1 && !rotLeft) {
			rotLeft = true;
		}


		if (transform.position.y > 20) {
			Destroy (gameObject);
		}
//		Debug.Log ("rot " + rotationVector);
			
	}

}
