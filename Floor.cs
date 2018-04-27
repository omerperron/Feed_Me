using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int numItems = Random.Range (0, 5);
		Vector2 min = this.gameObject.GetComponent<SpriteRenderer> ().bounds.min;
		Vector2 max = this.gameObject.GetComponent<SpriteRenderer> ().bounds.max;

		for (int i = 0; i < numItems; i++) {
			float x = Random.Range (min.x, max.x);
			float y = Random.Range (min.y, max.y);
			Instantiate ((GameObject)Resources.Load ("RedBalloon"), new Vector2(x, y), Quaternion.identity);

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
