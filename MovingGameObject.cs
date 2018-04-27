using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGameObject : MonoBehaviour {

	public List<Vector3> path = new List<Vector3> ();
	public float speed = 2f;
	// Use this for initialization
	void Start () {
		path.Add (transform.position);

	}

	// Update is called once per frame
	void Update () {
//		transform.position = new Vector3 (transform.position.x, transform.position.y + 0.02f, transform.position.z);

		float step = speed * Time.deltaTime;
		Vector3 movingTo = path [0];
		transform.position = Vector3.MoveTowards(transform.position, movingTo, step);
		if (Mathf.Abs (Vector3.Distance(transform.position, movingTo)) < Mathf.Epsilon) {
			path.Add(movingTo);
			path.RemoveAt(0);
		}
	}

	public void addPosition(Vector3 pos){
		path.Add (pos);
	}
}
