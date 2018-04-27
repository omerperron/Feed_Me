using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravity : Projectile {

	private bool boosting = false;

	protected override void Awake(){
		base.Awake ();
		power = 2;
		this.GetComponent<Rigidbody2D> ().gravityScale = 0f;

	}
	// Update is called once per frame
	protected override void Update () {
		if (Input.GetButtonDown("Fire1") && boosting == false){
			rb.velocity = rb.velocity.normalized * 20;
			boosting = true;
		}
		base.Update ();
	}






}
