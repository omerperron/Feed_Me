using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
/*
---- THINGS TO DO -----
- design lay out, menu and level selection
- find better images
- add functionality for iphone 
- 10 levels for easy, medium and hard.
- put random upgrades in random balloons
- parachute upgrades which randomly drop
- moving blocks so you have to time arrow
- more arrows, clusters, bombs, kamakaze, powerful, invincible, lazer, fire arrows, ice arrows
- more balloons: balloons that need the same colour to pop them (like armour), balloons that reflect, balloons that can only be popped
-                via explosion, defended balloons, balloons that attack back, fire and ice balloons etc.
- puzzles: shoot button to open door, anti gravity, portals, reflectors, fire.




left off not sure why arrows are set to zero
*/


public class Player : GObject {

	public int[] arrows = new int[ 6 ];
	private int currentProjectileType = Projectile.ARROW;


	private float maxSpeed = 20; //temp, this should be in projectile? 
	private float maxPullBack; //how far the bow can be pulledBack
	private bool pullingBack = false;
	private Vector3 pullPos;


	void Start(){
		isDraggable = false;
		Vector3 center = new Vector3();
		foreach (Transform child in transform){
			if (child.tag == "Player") {
				center = child.transform.position;
			}

		}

		Vector3 distance = new Vector3(3, 0, 0);
		Vector3 pointA = Camera.main.WorldToScreenPoint (center);
		Vector3 pointB = center + distance;
		pointB = Camera.main.WorldToScreenPoint (pointB);
		maxPullBack = Vector3.Distance (pointB, pointA);
			
	}



	new void  Update(){

		if (pullingBack) rotateBow ();

	}

	//rotates bow to face the mouse
	public void rotateBow(){
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




	public override void onClickBegan(){
		Debug.Log ("began" + arrows[currentProjectileType] + " " + GameControl.controller.isPaused());
		Debug.Log ("arrows: " + arrows);
		if (arrows[currentProjectileType] > 0 && !GameControl.controller.isPaused()) {
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (false);
			pullingBack = true;
		}
	}

	public override void onClickRelease(){
		Debug.Log ("end");
		if (pullingBack == true) {
			shoot ();

			arrows[currentProjectileType] -= 1;
			pullingBack = false;
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (true);
		}
	}

	public override void buttonAction(){

	}



	void shoot(){
		Debug.Log ("shooting");
		Vector3 currentMousePosition = Input.mousePosition;
		float distance = Vector3.Distance (currentMousePosition, Camera.main.WorldToScreenPoint (transform.position));
		Debug.Log ("distance: " + distance);
		Debug.Log ("max: " + maxPullBack);

		if (distance > maxPullBack) {
			distance = maxPullBack;
		} else if (distance < 20) {
			distance = 20;
		}
		float launchSpeed = (distance / maxPullBack) * maxSpeed;

		Ray ray = Camera.main.ScreenPointToRay (currentMousePosition);
		Vector2 vector = transform.position - ray.origin;


		InstantiateProjectile (vector, launchSpeed);


	}

	public void InstantiateProjectile(Vector3 vector, float launchSpeed){
		GameObject projectile = null;
		if (currentProjectileType == Projectile.CLUSTER) {

			GameObject cluster = new GameObject ("yay");
			cluster.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			cluster.AddComponent<Cluster> ();
			cluster.GetComponent<Cluster> ().launch (vector, launchSpeed);


		} else {
			projectile = Instantiate ((GameObject)Resources.Load (Projectile.getProjectileName(currentProjectileType)), transform.position, Quaternion.identity) as GameObject;
			projectile.GetComponent<Projectile> ().launch (vector, launchSpeed);

			Camera.main.GetComponent<CameraCode> ().setCurrentArrow (projectile);
			GameControl.controller.addArrow (projectile.GetComponent<Projectile>());
		} 


	}
		

	public void setCurProjectile(int proj){
		this.currentProjectileType = proj;
	}



	public void setNumArrows(int[] arrows){
		Debug.Log ("============== setting num arrows");

		this.arrows = arrows;
	}
	public int[] getNumArrows(){
		return arrows;
	}

	public bool isPullingBack(){
		if (pullingBack) return true;
		return false;
	}



}

