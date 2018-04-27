using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level : MonoBehaviour {

	//ArrowSelection arrowSelection;
	public int[] arrows = new int[ 6 ];
	public List<Projectile> activeArrows = new List<Projectile>();
	public int reward = 0;

	public GameObject[] targets;
	int buttonSize;
	int curWidth;
	int maxWidth;
	bool isOpen = false;

	// Use this for initialization
	void Start () {
		//arrowSelection = gameObject.GetComponent<ArrowSelection>();

		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Arrow"));
	}

	void Update(){

		for (int i = 0; i < activeArrows.Count; ++i) {
			Projectile proj = activeArrows [i];
			if(!proj.isMoving()){
				activeArrows.Remove (proj);
			}
		}


		buttonSize = Screen.width / 12;
		maxWidth = buttonSize * 4;

		if (curWidth <= maxWidth && isOpen) {
			curWidth += 50;
		} else if (curWidth >= 0 && !isOpen) {
			curWidth -= 50;
		}
		checkIfGameOver (); //this should be called when shooting arrow
	}
		
	void OnGUI(){

//		if (gameWon) {
//			if (GUI.Button (new Rect (Screen.width / 2, Screen.height / 2, 60, 30), "nice!")) {
//				SceneManager.LoadScene ("WinScene");
//			}
//		}


		pauseButton ();
	}

	public bool checkIfGameOver(){
		int numArrows = 0;
		for (int i = 0; i < arrows.Length; i++) {
			numArrows += arrows [i];
		}
		if (numArrows == 0 && getActiveArrows().Count == 0)
			return true;
		return false;
			
	}

	public bool checkIfWon(){
		Debug.Log ("chcking if won:");
		foreach(GameObject obj in targets){
			Debug.Log (obj.GetComponent<Target> ().getIsHit ());
			if (!obj.GetComponent<Target> ().getIsHit ()) {
				return false;
			}
		} 
		return true;
	}

	private void pauseButton(){
		return;

		GUI.backgroundColor = Color.red;

		Vector2 buttonPos = new Vector2(Screen.width/12 ,Screen.height/16 + buttonSize + Screen.height/16 );
		if(GUI.Button(new Rect(buttonPos.x, buttonPos.y, buttonSize, buttonSize), "pause")){
			if(isOpen) isOpen = false;
			else if(!isOpen) isOpen = true;

			if (GameControl.controller.pause) {
				GameControl.controller.pause = false;

			} else {
				GameControl.controller.pause = true;
			}
		}


		int selectionPos = (int) buttonPos.x + buttonSize;
		if (curWidth > 5) {
			GUI.Box (new Rect (selectionPos, buttonPos.y, curWidth, buttonSize), "");
		}





		for (int i = 0; i < 6; i++) {
			int button0 = selectionPos + i * buttonSize;
			int button0W = buttonSize;
			if ((selectionPos + curWidth) >= button0) {
				if ((selectionPos + curWidth) - button0 < buttonSize) {
					button0W = (selectionPos + curWidth) - button0;
				}
				if (GUI.Button (new Rect (button0, buttonPos.y + 5, button0W, buttonSize), i.ToString())) {
					if (i == 0) {
						GameControl.controller.changeToScene ("0_" + GameControl.controller.getCurrentLevel().ToString());
					}else if (i == 1) {
						GameControl.controller.changeToScene ("menu");

					}else if (i == 2) {
						Debug.Log ("help");
					}else if (i == 3) {
						Debug.Log ("toggle sound");
					}else if (i == 4) {
						GameControl.controller.save ();
					}else if (i == 5) {
						GameControl.controller.Load ();
					}
					isOpen = false;
					GameControl.controller.pause = false;

				}

			}
		}

	}



	public void setGameWon(){
		//gameWon = true;

	}

	public int[] getNumArrows(){
		return arrows;
	}

	public List<Projectile> getActiveArrows(){
		return activeArrows;
	}

	public void addArrow(Projectile obj){
		activeArrows.Add (obj);
	}

	public bool isArrowInMotion(){
		foreach(Projectile proj in activeArrows){
			if (proj.isMoving ()) {
				return true;
			}
		}

		return false;
	}

}
