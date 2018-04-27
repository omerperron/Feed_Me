using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBalloon : Balloon {

	protected override void Awake () {
		health = 3;
		base.Awake ();

	}
		
	// Update is called once per frame
	protected override void  Update () {
		base.Update ();
	}



}
