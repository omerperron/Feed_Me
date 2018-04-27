using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour {

	int maxWidth;
	int curWidth = 1;
	bool isOpen = false;
	Texture arrowImage;
	int buttonSize;
	int openSpeed = 50;
	public List<GameObject> objects = new List <GameObject>();
	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (0, 0, 0);
		arrowImage = (Texture) Resources.Load ("arrowIcon");
	}

	// Update is called once per frame
	void Update () {
		
		buttonSize = Screen.width / 12;

		if (curWidth <= maxWidth && isOpen) {
			curWidth += openSpeed;
		} else if (curWidth >= 0 && !isOpen) {
			curWidth -= openSpeed;
		}

	}

	void OnGUI(){
		return;
		int originalButtonSize = 1920 / 12;
		int originalFont = 40;



		GUIStyle customButton = new GUIStyle("button");
		customButton.fontSize = originalFont * (buttonSize / originalButtonSize);



		GUI.backgroundColor = Color.black;
		Vector2 buttonPos = new Vector2(Screen.width/12 ,Screen.height*((float) 3/16) + 2*buttonSize);

		if(GUI.Button(new Rect(buttonPos.x, buttonPos.y, buttonSize, buttonSize), arrowImage)){
			if(isOpen) isOpen = false;
			else if(!isOpen) isOpen = true;
		}


		int selectionPos = (int) buttonPos.x + buttonSize;
		if (curWidth > 5) {
			GUI.Box (new Rect (selectionPos, buttonPos.y, curWidth, buttonSize), "");
		}



		//Level level = GameControl.controller.getLevel ();
		maxWidth = objects.Count * buttonSize;

		for (int i = 0; i < objects.Count; i++) {
			int button0 = selectionPos + i * buttonSize;
			int button0W = buttonSize;
			if ((selectionPos + curWidth) >= button0) {
				if ((selectionPos + curWidth) - button0 < buttonSize) {
					button0W = (selectionPos + curWidth) - button0;
				}

				if (GUI.Button (new Rect (button0, buttonPos.y, button0W, buttonSize), i.ToString(), customButton)) {
					GameObject obj = objects[i];
					if (GameControl.controller.score >= obj.GetComponent<GObject> ().price) {
						Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
						obj.SetActive (true);
						isOpen = false;
						Debug.Log ("this cost: " + obj.GetComponent<GObject> ().price);
						GameControl.controller.score -= obj.GetComponent<GObject> ().price;
					}else{
						Debug.Log("not enough money");
					}


				}

			}
		}

	}
}

