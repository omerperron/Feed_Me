using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : Projectile {

	int numBounces;

	protected override void  Awake(){
		base.Awake ();
		power = 2;
		numBounces = 3;
	}
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}

	public void bounce(Collision2D col){
		numBounces--;
		if (numBounces <= 0)
			setStationary (col.gameObject);
		
	}






}
