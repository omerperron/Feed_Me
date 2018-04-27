using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D col){
		
		if (col.gameObject.tag == "Button") {
			hitButton (col.gameObject);
		}
	}

	void hitButton(GameObject button){
		Debug.Log ("button hit");
		Button but = button.GetComponent<Button> ();
		but.buttonPressed ();
	}
}
