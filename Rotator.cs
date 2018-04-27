using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	//speed and direction of rotator
	public float rotationSpeed = 15f;
	public bool rotatingForward = true;
	 


	// Update is called once per frame
	void Update () {
		Vector3 rotationVector = transform.rotation.eulerAngles;
		if (rotatingForward) {
			rotationVector.z += rotationSpeed * Time.deltaTime;
		}else {
			rotationVector.z -= rotationSpeed * Time.deltaTime;
		}
		transform.rotation = Quaternion.Euler(rotationVector);

	}

}
