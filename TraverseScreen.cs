using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TraverseScreen : MonoBehaviour {

	public int screenToLoad;

	void OnGUI(){

		GUI.Label (new Rect(Screen.width/2 - 50, Screen.height - 80, 100, 30), "current screen" + 
			(System.Convert.ToInt32(SceneManager.GetActiveScene().name) + 1));
		if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height - 50, 100, 40), "load screen" + (screenToLoad + 1))){
			SceneManager.LoadScene (screenToLoad);
		}

	}
}
