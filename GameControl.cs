using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

	public static GameControl controller;

	private Player player; 
	private Level level;
	private Canvas canvas;


	private float time;
	private float lastTouch = -1;
	private Touch previousTouch;
	private GameObject prevClickedOn;
	private bool isInGame = false;

	public int score;


	int currentPack;
	int currentLevel;
	int lastCompletedPack = -1;
	//Dictionary<int, List<int>> completedLevels = new Dictionary<int, List<int>>();
	List<List<int>> completed = new List<List<int>> ();
	public bool pause = false;

	// Use this for initialization
	void Awake () {
		

		if (controller == null) {
			//if there is no controller this is set to be the controller
			Debug.Log("controller is null, initialising");
			DontDestroyOnLoad (gameObject);
			controller = this;
			setUpLevelsCompleted ();
			controller.score = 0;


		} else if (controller != this) {
			
			Destroy (gameObject);
		}
		string lvl = SceneManager.GetActiveScene().name;
		if (lvl.Equals ("Menu") || lvl.Equals ("LevelSelection") || lvl.Equals ("PackSelection") || lvl.Equals ("WinScene")) {
			controller.isInGame = false;
		} else {
			controller.isInGame = true;
		}
		if(controller.isInGame){
			findGameObjects ();
			newLevel ();
		}

	}



	void Update () {
		time += Time.deltaTime;
		if(controller.isInGame) getUserInput ();

		Time.timeScale = 1;
		if (pause) Time.timeScale = 0;
		if(isInGame)
			checkIfLost ();
	}



	void findGameObjects(){
		Debug.Log ("finding objects and shiz");
		bool isSuccessful = true;
		if (GameObject.FindGameObjectWithTag ("Player") != null)
			controller.player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		else {
			Debug.Log ("could not find player");
			isSuccessful = false;
		}
		
		if (GameObject.FindGameObjectWithTag ("Level") != null)
			controller.level = GameObject.FindGameObjectWithTag ("Level").GetComponent<Level> ();
		else {
			Debug.Log ("could not find Level");
			isSuccessful = false;

		}
		
		if (GameObject.FindGameObjectWithTag ("Canvas") != null) {
			controller.canvas = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<Canvas> ();
			controller.canvas.gameObject.SetActive (false);
		} else {
			Debug.Log ("could not find canvas");
			isSuccessful = false;
		}
		if (Camera.main.GetComponent<CameraCode> ().backgroundImage == null) {
			isSuccessful = false;
			Debug.Log("could not find background image in main camera");
		}
		//do this for everything else eg. background in maincamera
		if (isSuccessful == false) {
			throw new Exception("throwing exception: could not find an element");
		}

		

	}


	public void newLevel(){
		time = 0;
		controller.player.setNumArrows (controller.level.getNumArrows());


	}



	private void getUserInput(){

		if (Input.touchCount == 1) { //if one touch
			oneTouch ();

		} else if (Input.touchCount == 2) {
			Camera.main.GetComponent<CameraCode> ().changeZoom ();

		} else {
			GameObject objA = null;
			if (prevClickedOn != null) {
				objA = prevClickedOn;
			}else if (Input.GetMouseButtonDown (0)) {
				objA = clickedOn (Input.mousePosition);
			}

			if (Input.GetMouseButtonUp (0)) {
				if (objA != null && objA.tag != "Untagged") {
					Debug.Log ("touch end!!!!");
					touchEnd (objA);
				}
				prevClickedOn = null;
				objA = null;


			}
			if (prevClickedOn == null) {
				Camera.main.GetComponent<CameraCode> ().dragCamera ();
			}


			if (objA != null && objA.tag != "Untagged") {
				prevClickedOn = objA;
				if (Input.GetMouseButtonDown (0)) {
					Debug.Log ("touch began!");
					touchBegan (objA);
				}
			}

		}
	}

	void oneTouch(){


		Touch[] touches = Input.touches;
		Touch touch = touches [0];

		GameObject obj = null;
		if (prevClickedOn != null) { //if we have already pressed on something continue with that object
			obj = prevClickedOn;
		} else if(touch.phase == TouchPhase.Began){ //if the press just began, check if we pressed on an object
			obj = clickedOn (touch.position);

		}

		if (obj != null && obj.tag != "Untagged") { //if we are dealing with an object, call the necessary function for that object
			prevClickedOn = obj;
			if (touch.phase == TouchPhase.Began) {
				touchBegan (obj);
			}else if(touch.phase == TouchPhase.Ended) {
				touchEnd (obj);
			}

		} else { //if we are not dealing with an object, we need to check if we pressed or if we dragged.

			if (touch.phase == TouchPhase.Began) {
				lastTouch = time;
			} else if (touch.phase == TouchPhase.Ended) {
				Debug.Log (time - lastTouch);
				if (time - lastTouch < 0.3f) {
					tappedOnScreen (touch);
				} 

				lastTouch = -1;
			} else if(time - lastTouch >= 0.3f){
				Camera.main.GetComponent<CameraCode> ().dragCamera ();
			}

		}

		if (touch.phase == TouchPhase.Ended) {
			Debug.Log ("turning off isDragged");
			prevClickedOn = null;
			Camera.main.GetComponent<CameraCode> ().setIsDragged (false);

		}
	}

	private void touchBegan(GameObject obj){
		string type = obj.tag;
		GObject gObj = obj.GetComponent<GObject> ();
		if(gObj != null)
			gObj.onClickBegan ();
	}

	private void touchEnd(GameObject obj){
		string type = obj.tag;

		GObject gObj = obj.GetComponent<GObject> ();
		if(gObj != null)
			gObj.onClickRelease ();
	}

	private void tappedOnScreen(Touch touch){
		Debug.Log ("tapped on screen");
	}


	public bool checkIfWon(){
		
		if (!level.checkIfWon () || level == null) return false;

		canvas.gameObject.SetActive(true);
		levelCompleted (currentLevel);
		level.setGameWon ();
		return true;



	}

	public bool checkIfLost(){
		if (!level.checkIfGameOver ())return false;

		changeToScene ("GameOver");
		return true;

	}

	public bool isInLevel(){
		if (controller.isInGame)
			return true;
		return false;
	}

	public GameObject clickedOn(Vector2 position){
		RaycastHit2D hit = Physics2D.CircleCast (Camera.main.ScreenToWorldPoint (position), 0.3f, Vector2.zero);
		if (hit) {
			GameObject obj = hit.collider.gameObject;
			Debug.Log (obj.tag);
			return obj;
		}
		return null;

		//CODE FOR NON PHYSICS2D
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//
//		if(Physics.Raycast (ray, hit)){
//			Debug.L 
	}

	public Level getLevel(){
		return level;
	}
	public Player getPlayer(){
		return player;
	}

	public int getCurrentLevel(){
		return controller.currentLevel;
	}
	public void setCurrentLevel(int level){
		controller.currentLevel = level;
		Debug.Log ("set current level: " + GameControl.controller.currentLevel);
	}

	public int getCurrentPack(){
		return GameControl.controller.currentPack;
	}
	public void setCurrentPack(int pack){
		GameControl.controller.currentPack = pack;
		Debug.Log ("set current pack: " + GameControl.controller.currentPack);

	}

	public void levelCompleted(int level){
		Debug.Log ("reward: " + controller.level.reward);
		controller.score += controller.level.reward;

		completed[controller.getCurrentPack()].Add(controller.getCurrentLevel());
		if (completed [controller.getCurrentPack ()].Count == 5 && lastCompletedPack != controller.currentPack)
			controller.lastCompletedPack = currentPack;
	}

	public int getLastCompletedPack(){
		return lastCompletedPack;
	}

	public bool isCompleted(int level){


		List<int> currentPack = new List<int>(controller.completed [controller.getCurrentPack()]);
		if(currentPack.Contains(level))
			return true;

		return false;
	}

	public bool isPaused(){
		if (pause)
			return true;
		return false;
	}

	public void changeToScene(string level){
		Debug.Log ("changing scene");
		if (level == "restart") {
			
			SceneManager.LoadScene ("0_" + currentLevel.ToString ());
		} else if (level == "next") {
			Debug.Log ("current scene" + SceneManager.GetActiveScene().name);
			GameControl.controller.setCurrentLevel (controller.getCurrentLevel()+1);
			SceneManager.LoadScene (controller.currentPack + "_" + controller.getCurrentLevel().ToString());
		}else {
			SceneManager.LoadScene (level);

		}
	}

	void setUpLevelsCompleted(){
//		for (int i = 0; i < 16; i++) {
//			List<int> newList = new List<int>();
//			controller.completedLevels.Add (i, newList);
//		}
		for (int i = 0; i < 16; i++) {
			List<int> newList = new List<int>();
			controller.completed.Add (newList);
		}
	}

	public void addArrow(Projectile arrow){
		level.addArrow (arrow);
	}

	void OnGUI(){

		GUI.Label (new Rect (10, 160, 100, 30), "score: " + controller.score.ToString());

	}

	public void save(){
		Debug.Log ("saving");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/gameData.dat");

		GameData data = new GameData ();
		data.currentLevel = controller.getCurrentLevel ();
		data.currentPack = controller.getCurrentPack ();
		data.score = controller.score;
		data.completed = controller.completed;
		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load(){
		Debug.Log ("loading");
		if (File.Exists (Application.persistentDataPath + "/gamedata.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/gameData.dat", FileMode.Open);

			GameData data = (GameData) bf.Deserialize (file);
			file.Close ();
			controller.setCurrentLevel (data.currentLevel);
			controller.setCurrentPack (data.currentPack);
			controller.score = data.score;

			controller.completed = data.completed;

		}

	}
}

[Serializable]
class GameData{
	public int currentLevel;
	public int currentPack;
	public List<List<int>> completed = new List<List<int>>();
	public int score;

}
