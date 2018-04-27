using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour {

	//private List<Projectile> cluster = new List<Projectile>();

	void Awake(){


	}
	void Start(){
	}

	public void launch(Vector2 vector, float launchSpeed){

		GameObject projectile = Instantiate ((GameObject)Resources.Load ("Kamakaze"), transform.position, Quaternion.identity) as GameObject;
		projectile.GetComponent<Kamakaze> ().launch (vector, launchSpeed);
		Camera.main.GetComponent<CameraCode> ().setCurrentArrow (projectile);
		Vector2 low = new Vector2(vector.x, vector.y);
		low.x -= 0.1f * vector.x;
		low.y -= 0.3f * vector.y;
		Debug.Log(low + " " + vector);
		GameObject projectile2 = Instantiate ((GameObject)Resources.Load ("Kamakaze"), transform.position, Quaternion.identity) as GameObject;
		projectile2.GetComponent<Kamakaze> ().launch (low, launchSpeed);


		Vector2 high = new Vector2(vector.x, vector.y);
		high.x += 0.1f * vector.x;
		high.y += 0.3f * vector.y;
		Debug.Log(low + " " + vector);
		GameObject projectile3 = Instantiate ((GameObject)Resources.Load ("Kamakaze"), transform.position, Quaternion.identity) as GameObject;
		projectile3.GetComponent<Kamakaze> ().launch (high, launchSpeed);
	}




	public void launch(Vector3 velocity){
		float magnitude = velocity.magnitude;
		Vector3 direction = velocity.normalized;


		GameObject projectile = Instantiate ((GameObject)Resources.Load ("Kamakaze"), transform.position, Quaternion.identity) as GameObject;
		projectile.GetComponent<Rigidbody2D> ().velocity = new Vector3 (velocity.x, velocity.y, velocity.z);

		Vector3 up = Quaternion.Euler(0, 0, 5) * direction;
		Vector3 upVel = up * magnitude;
		GameObject projectile2 = Instantiate ((GameObject)Resources.Load ("Kamakaze"), transform.position, Quaternion.identity) as GameObject;
		projectile2.GetComponent<Rigidbody2D> ().velocity = new Vector3 (upVel.x, upVel.y, upVel.z);

		Vector3 down = Quaternion.Euler(0, 0, -5) * direction;
		Vector3 downVel = down * magnitude;
		GameObject projectile3 = Instantiate ((GameObject)Resources.Load ("Kamakaze"), transform.position, Quaternion.identity) as GameObject;
		projectile3.GetComponent<Rigidbody2D> ().velocity = new Vector3 (downVel.x, downVel.y, downVel.z);


	}




}
