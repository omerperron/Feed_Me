using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {

	public GameObject background;

	int buttonWidth = 50;
	int buttonHeight = 30;
	int rowX = 4;
	int rowY = 4;


	
	// Update is called once per frame
	void Update () {
		Color color = background.GetComponent<SpriteRenderer>().material.color;
		if(color.a > 0.2f) color.a -= 0.01f;
		background.GetComponent<SpriteRenderer>().material.color = color; 

		buttonWidth = Screen.width / 10;
		buttonHeight = Screen.height / 15;
	}

	void OnGUI(){

		float width = background.GetComponent<SpriteRenderer>().bounds.size.x;
		float height = background.GetComponent<SpriteRenderer>().bounds.size.y;


		Vector3 ws = Camera.main.WorldToScreenPoint(new Vector3(background.transform.position.x - width/2, background.transform.position.y));
		Vector3 we = Camera.main.WorldToScreenPoint(new Vector3(background.transform.position.x + width/2, background.transform.position.y));
		float pWidth = we.x - ws.x;

		Vector3 hs = Camera.main.WorldToScreenPoint(new Vector3(background.transform.position.x, background.transform.position.y - height/2));
		Vector3 he = Camera.main.WorldToScreenPoint(new Vector3(background.transform.position.x, background.transform.position.y + height/2));
		float pHeight = he.y - hs.y;

		float gapW = (pWidth - (buttonWidth * rowX)) / (rowX+1);
		float gapH = (pHeight - (buttonHeight * rowY)) / (rowY+1);

		Vector3 startingPosition = new Vector3(background.transform.position.x - width/2, -background.transform.position.y - height/2);
		startingPosition = Camera.main.WorldToScreenPoint (startingPosition);
		Vector3 curPos = new Vector3(startingPosition.x, startingPosition.y);

		for (int i = 0; i < rowY; i++) {
			curPos.x = startingPosition.x;

			curPos.y += gapH;
			for (int j = 0; j < rowX; j++) {
				curPos.x += gapW;
	
				int levInt = ((int)((i * rowX) + j + 1));
				if (levInt > 10)
					return;
				string levString = levInt.ToString();
				if( GameControl.controller.isCompleted(levInt)){
					levString += "f";
				}
				GUIStyle customButton = new GUIStyle("button");
				customButton.fontSize = 30;
				if(GUI.Button(new Rect(curPos.x, curPos.y ,buttonWidth, buttonHeight), levString, customButton)){
					GameControl.controller.setCurrentLevel (levInt);
					GameControl.controller.changeToScene (GameControl.controller.getCurrentPack() + "_" + levInt.ToString ());
				}
				curPos.x += buttonWidth;

			}
			curPos.y += buttonHeight;

		}

		//Debug.Log("pack: " + GameControl.controller.getCurrentPack() + " level: " + GameControl.controller.getCurrentLevel()); 	
	}


}

