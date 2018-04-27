using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelection : MonoBehaviour {
	public List<Texture2D> textures = new List<Texture2D> ();
	public List<GameObject> objects = new List <GameObject>();

	float maxWidthX;
	float maxWidthY;
	float maxWidthObject;
	float curWidthX = 1;
	float curWidthY = 1;
	float curWidthObject = 1;
	bool isOpen = false;
	Texture arrowImage;
	float buttonSize;
	float openSpeedX;
	float openSpeedY;
	float openSpeedObject;
	int speed = 10;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (0, 0, 0);
		arrowImage = (Texture) Resources.Load ("arrowIcon");


	}
	
	// Update is called once per frame
	void Update () {
		buttonSize = Screen.width / 12;
		maxWidthY = 2 * buttonSize;
		openSpeedY = maxWidthY / speed;

		if (curWidthX <= maxWidthX && isOpen) {
			curWidthX += openSpeedX;
		} else if (curWidthX >= 0 && !isOpen) {
			curWidthX -= openSpeedX;
		}

		if (curWidthY <= maxWidthY && isOpen) {
			curWidthY += openSpeedY;
		} else if (curWidthY >= 0 && !isOpen) {
			curWidthY -= openSpeedY;
		}

		if (curWidthObject <= maxWidthObject && isOpen) {
			curWidthObject += openSpeedObject;
		} else if (curWidthObject >= 0 && !isOpen) {
			curWidthObject -= openSpeedObject;
		}
	}

	void OnGUI(){

		GUIStyle customButton = new GUIStyle("button");
		customButton.fontSize = 40;
		GUI.backgroundColor = Color.black;

		Vector2 buttonPos = new Vector2(Screen.width/12 ,Screen.height/16 );

		if(GUI.Button(new Rect(buttonPos.x, buttonPos.y, buttonSize, buttonSize), arrowImage)){
			if (isOpen) {
				isOpen = false;
				GameControl.controller.pause = false;
			}
			else if(!isOpen){
				isOpen = true;
				GameControl.controller.pause = true;
			}
		}



		//ARROWS-------------------------------
		float selectionPosX = buttonPos.x + buttonSize;




		Level level = GameControl.controller.getLevel ();
		int[] arrows = level.getNumArrows ();
		maxWidthX = arrows.Length * buttonSize;
		openSpeedX = maxWidthX / speed;
		for (int i = 0; i < arrows.Length; i++) {
			float button0 = selectionPosX + i * buttonSize;
			float button0W = buttonSize;
			if ((selectionPosX + curWidthX) >= button0) {
				if ((selectionPosX + curWidthX) - button0 < buttonSize) {
					button0W = (selectionPosX + curWidthX) - button0;
				}

				if (GUI.Button (new Rect (button0, buttonPos.y, button0W, buttonSize), textures[i], customButton)) {
					GameControl.controller.getPlayer ().setCurProjectile (i);
					isOpen = false;
					GameControl.controller.pause = false;

				}
				int[] numArrows = GameControl.controller.getLevel ().arrows;
				GUI.Label (new Rect(button0, buttonPos.y, button0W, buttonSize), numArrows[i].ToString(), customButton);

			}
		}


		//MENU --------------------------------
		float selectionPosY = (int) buttonPos.y + buttonSize;


		for (int i = 0; i < 2; i++) {
			float yPos = selectionPosY + i * buttonSize;
			float buttonH = buttonSize;
			if ((selectionPosY + curWidthY) >= yPos) {
				if ((selectionPosY + curWidthY) - yPos < buttonSize) {
					buttonH = (selectionPosY + curWidthY) - yPos;
				}

				if (GUI.Button (new Rect (buttonPos.x, yPos, buttonSize, buttonH), textures[6 + i], customButton)) {
					if (i == 0) {
						GameControl.controller.changeToScene (GameControl.controller.getCurrentPack().ToString() + "_"
							+ GameControl.controller.getCurrentLevel().ToString());
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

		//ITEMS-----------------------------------------------
		maxWidthObject = objects.Count * buttonSize;
		openSpeedObject = maxWidthObject / speed;
		float posY = (int) buttonPos.y + buttonSize;
		float itemYPos = selectionPosY + buttonSize;

		for (int i = 0; i < objects.Count; i++) {
			float button0 = selectionPosX + i * buttonSize;
			float button0W = buttonSize;
			float button0H = buttonSize;
			if ((selectionPosX + curWidthObject) >= button0 && (curWidthY + buttonSize) >= posY - buttonSize/2) {
				if ((selectionPosX + curWidthObject) - button0 < buttonSize) {
					button0W = (selectionPosX + curWidthObject) - button0;
				}
				if ((selectionPosY + curWidthY  + buttonSize) - itemYPos < buttonSize) {
					button0H = (selectionPosY + curWidthY  + buttonSize) - itemYPos;
				}


				Texture tex = objects [i].GetComponent<SpriteRenderer> ().sprite.texture;
				if (GUI.Button (new Rect (button0, posY, button0W, button0H), tex, customButton)) {
					GameObject obj = objects[i];
					if (GameControl.controller.score >= obj.GetComponent<GObject> ().price) {
						obj.SetActive (true);
						isOpen = false;
						GameControl.controller.pause = false;
						objects.RemoveAt (i);
						Debug.Log ("this cost: " + obj.GetComponent<GObject> ().price);
						GameControl.controller.score -= obj.GetComponent<GObject> ().price;
					}else{
						Debug.Log("not enough money");
					}

				}
				if(objects.Count > i && objects[i])
					GUI.Label (new Rect(button0, posY, button0W, button0H), 
						objects[i].GetComponent<GObject>().price.ToString(), customButton);


			}
		}
		if(curWidthX > 1)
			GUI.Box (new Rect(buttonPos.x, buttonPos.y, curWidthX + buttonSize, curWidthY +  buttonSize), "");



	}
}
