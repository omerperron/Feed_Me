using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : GObject {

	private bool opacityUp = false;
	public GameObject companion = null;
	public bool entered = false;
	public bool isInUse = false;


	public override void onClickBegan (){
		base.mouseDown ();
	}
	public override void onClickRelease (){
		base.mouseUp ();

	}
	public override void buttonAction(){

	}

	void OnTriggerStay2D(Collider2D col){
		entered = true;
	}

	// Use this for initialization
	void Start () {
		isDraggable = true;
		base.price = 100;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		companion.GetComponent<Portal> ().isInUse = false;
	}

	// Update is called once per frame
	new void Update () {

		if (companion && companion.GetComponent<Portal> ().isInUse == false) {
			entered = false;
		}



		base.Update ();

		Color color = GetComponent<SpriteRenderer>().material.color;
		if (opacityUp)
			color.a += 0.005f;
		else
			color.a -= 0.005f;
		GetComponent<SpriteRenderer>().material.color = color; 



		if (color.a > 0.7)
			opacityUp = false;
		if (color.a < 0.3)
			opacityUp = true;
	}

	public Vector3 getCompanionPos(){
		if (companion != null) return companion.transform.position;
		return Vector3.zero;
	}

	public bool getEntered(){
		return entered;
	}

	public void teleport(GameObject obj){
		Portal comp = companion.GetComponent<Portal> ();
		if (comp && !comp.entered) {
			entered = true;
			companion.GetComponent<Portal> ().isInUse = true;
			Vector3 partnerPos = getCompanionPos ();
			if (partnerPos != Vector3.zero) {
				obj.transform.position = new Vector3 (partnerPos.x, partnerPos.y, partnerPos.z);
			}		
		}

	}
}
