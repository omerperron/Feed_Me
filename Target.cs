using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
	bool isHit = false;
	// Use this for initialization
	void Start () {
		
	}

	public void hit(){
		isHit = true; 
	}

	public bool getIsHit(){
		return isHit;
	}

}
