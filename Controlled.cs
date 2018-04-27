using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlled : Projectile {
	private float rotationSpeed = 1f;

	protected override void Awake(){
		base.Awake ();
		power = 2;
		this.GetComponent<Rigidbody2D> ().gravityScale = 0f;

	}
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (Input.GetMouseButton (0) && isMoving()) {

			if (Input.mousePosition.x < Screen.width / 2) {

				float magnitude = rb.velocity.magnitude;
				Vector3 direction = rb.velocity.normalized;
				direction = Quaternion.Euler(0, 0, rotationSpeed) * direction;
				Vector3 newVel = direction * magnitude;
				rb.velocity = new Vector3 (newVel.x, newVel.y);


			} else {
				float magnitude = rb.velocity.magnitude;
				Vector3 direction = rb.velocity.normalized;
				direction = Quaternion.Euler(0, 0, -rotationSpeed) * direction;
				Vector3 newVel = direction * magnitude;
				rb.velocity = new Vector3 (newVel.x, newVel.y);

			}
		}

		
	}





}
