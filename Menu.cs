using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Invoke("RandomArrow", 0.5f);


	}

	void RandomArrow(){

		float randomTime = Random.Range ( 0.5f , 3 );
		spawnArrow ();
		Invoke("RandomArrow", randomTime);
	}


	void spawnArrow(){
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		int side = Random.Range(0, 2);

		Vector2 vector;
		float x;
		var y = Random.Range(-height/2, height/2);
		if (side == 0) {
			x = -width / 2;
			Vector2 point = new Vector2 (x+ 2, y + Random.Range(0,2));
			vector = point - new Vector2 (x, y);
		} else {
			x = width / 2;
			Vector2 point = new Vector2 (x - 2, y + Random.Range(0,2));
			vector = point - new Vector2 (x, y);
		}

		string[] candy = new string[] {"Arrow", "Bouncy" };
		string toShoot = candy[Random.Range (0, candy.Length)];
		GameObject projectile = Instantiate ((GameObject)Resources.Load (toShoot), new Vector3(x, y), Quaternion.identity);
		projectile.GetComponent<Projectile> ().launch (vector, 15);


	}


}
