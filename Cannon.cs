using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : GObject {

	private GameObject curProjectile;
	private bool pullingBack;

	void Start(){
		base.price = 100;
	}

	public override void onClickBegan (){
		Debug.Log ("mouseDown");
		base.mouseDown ();
		if (curProjectile != null && !GameControl.controller.isPaused()) {
			Vector3 pullPos = Input.mousePosition;
			pullingBack = true;
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (false);

			Debug.Log("pulling back!! " + pullPos);
		}
	}
	public override void onClickRelease (){
		base.mouseUp ();
		if (pullingBack == true) {
			shoot ();
			//curProjectile = null;
			pullingBack = false;
			Camera.main.GetComponent<CameraCode> ().setIsDraggable(true);

		}
		
	}

	public override void buttonAction(){
		
	}
	
	// Update is called once per frame
	void  Update () {
		base.Update ();
		if (pullingBack) {
			rotateCannon ();
		}
		if(curProjectile != null && !curProjectile.GetComponent<Projectile>().isMoving()){
			curProjectile = null;
		}
	}

	public void setArrow(GameObject proj){
		if (curProjectile != null)
			return;
		GameObject newObj = Instantiate(proj, gameObject.transform.position, proj.transform.rotation);
		Destroy (proj);

		curProjectile = newObj;
		curProjectile.SetActive (false);
		//Projectile script = proj.GetComponent<Projectile> ();


	}

 

	public void rotateCannon(){
		Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 vec = (Vector2) gameObject.transform.position - mousePos;
		vec.Normalize();

		float angle = vec.y / vec.x;
		float fAngle = (Mathf.Atan (angle) * Mathf.Rad2Deg);

		if (vec.x < 0) {
			fAngle += 180;
		}

		gameObject.transform.rotation = Quaternion.Euler(0, 0, fAngle);

	}

	void shoot(){
		curProjectile.SetActive (true);

		Vector3 currentMousePosition = Input.mousePosition;
		float distance = Vector3.Distance (currentMousePosition, Camera.main.WorldToScreenPoint (transform.position));
		Debug.Log ("distance: " + distance);
		int maxPullBack = 200;

		Debug.Log ("max: " + maxPullBack);
		int maxSpeed = 20; //DO WE NEED THESE OR CAN IT JUST BE A SET SPEED
		if (distance > maxPullBack) {
			distance = maxPullBack;
		} else if (distance < 20) {
			distance = 20;
		}
		float launchSpeed = (distance / maxPullBack) * maxSpeed;

		Ray ray = Camera.main.ScreenPointToRay (currentMousePosition);
		Vector2 vector = transform.position - ray.origin;
		Projectile script = curProjectile.GetComponent<Projectile> ();
		script.launch (vector, launchSpeed);
		//DO YOU HAVE TO FIND WHAT TYPE OF ARROW IT IS TO CALL LAUNCH? CAN YOU JUST CALL LAUNCH ON THE PROJECTILE 
		//BASE CLASS ITSELF? FIGURE IT OUT TOMORROW!
	}
}
