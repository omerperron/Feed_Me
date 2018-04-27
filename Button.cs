using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	bool isHit = false;
	public List<GameObject> puzzleObjects = new List<GameObject> ();




	public void buttonPressed(){
		//puzzleObject pObject = puzzleObject.GetComponent<puzzleObject> ();
		if (isHit == false) {
			foreach(GameObject gObject in puzzleObjects){
				puzzleObject pObject = gObject.GetComponent<puzzleObject> ();
				pObject.performAction (true);
			}
			isHit = true;
		} else if (isHit == true) {
			foreach(GameObject gObject in puzzleObjects){
				puzzleObject pObject = gObject.GetComponent<puzzleObject> ();
				pObject.performAction (true);
			}			
			isHit = false;
		}
	}

}
