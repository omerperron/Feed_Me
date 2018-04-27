using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void boost(Projectile projectile){

		float newSpeed = projectile.GetComponent<Rigidbody2D> ().velocity.magnitude * 2;
		Vector3 direction = projectile.GetComponent<Rigidbody2D> ().velocity.normalized;
		Vector3 newVelocity = new Vector3 (direction.x * newSpeed, direction.y * newSpeed, direction.z * newSpeed);
		projectile.GetComponent<Rigidbody2D> ().velocity = newVelocity;

	}


}
