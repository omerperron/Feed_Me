using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WinScene : MonoBehaviour {



	// Use this for initialization
	void Start () {
		//calls this function in the specified time
		Invoke("RandomBalloon", 0.5f);
		Invoke("RandomArrow", 0.5f);
		//GameControl.controller.setCurrentLevel (-1); //-1 for winscene

	}

	// Update is called once per frame
	void Update () {

	}

	//spawns a random balloon
	void RandomBalloon(){

		float randomTime = Random.Range ( 0.5f , 3 );
		spawnBalloon ();
		Invoke("RandomBalloon", randomTime);
	}

	//spawns a random arrow
	void RandomArrow(){

		float randomTime = Random.Range ( 0.5f , 3 );
		spawnArrow ();
		Invoke("RandomArrow", randomTime);
	}

	//chooses the position of random balloon on screen
	void spawnBalloon(){
		Camera cam = Camera.main;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		var y = -height / 2;
		var x = Random.Range(-width/2, width/2);
		Instantiate ((GameObject)Resources.Load ("DisplayBalloon"), new Vector3(x, y), Quaternion.identity);
		Debug.Log ("balloon created");

	}

	//chooses the position and direction of random arrow on screen
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

		GameObject projectile = Instantiate ((GameObject)Resources.Load ("Arrow"), new Vector3(x, y), Quaternion.identity);
		projectile.GetComponent<Projectile> ().launch (vector, 15);


	}

	void OnGUI(){
//		GUIStyle textStyle = new GUIStyle();
//		textStyle.normal.textColor = Color.white;
//
//		float resolution =  Screen.width * Screen.height;
//		float originalTextSize = 70;
//		textStyle.fontSize = (int)((originalTextSize * resolution) / GameControl.RESOLUTION);
//		Rect rect = new Rect (Screen.width / 2 - textStyle.fontSize / 2, Screen.height / 2, textStyle.fontSize, 20);
//			
//
//		float buttonWidth = Screen.width / 8;
//		float buttonHeight = Screen.height / 12;
//
//
//		GUI.Label (new Rect (Screen.width/2 - textStyle.fontSize/2, Screen.height/2, textStyle.fontSize, 20), "You Win!", textStyle);
//
//		if(GUI.Button(new Rect(Screen.width * 0.25f - buttonWidth/2, Screen.height * 0.8f, buttonWidth, buttonHeight), "restart")){
//			SceneManager.LoadScene ("0_" + GameControl.controller.getCurrentLevel());
//
//		}else if(GUI.Button(new Rect(Screen.width * 0.5f - buttonWidth/2, Screen.height * 0.8f, buttonWidth, buttonHeight), "home")){
//			SceneManager.LoadScene ("LevelSelection");
//
//		}else if(GUI.Button(new Rect(Screen.width * 0.75f - buttonWidth/2, Screen.height * 0.8f, buttonWidth, buttonHeight), "next level")){
//			SceneManager.LoadScene ("0_" + (GameControl.controller.getCurrentLevel()+1).ToString());
//			GameControl.controller.setCurrentLevel (GameControl.controller.getCurrentLevel()+1);
//		}

	}
}
