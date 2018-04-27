using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamakaze : Projectile {

	private bool boosting = false;

	protected override void Awake(){
		base.Awake ();
		power = 2;

	}
	// Update is called once per frame
	protected override void Update () {
		if (Input.GetButtonDown("Fire1") && boosting == false && isMoving()){
			rb.velocity = rb.velocity.normalized * 20;
			boosting = true;
		}
		base.Update ();
	}






}
