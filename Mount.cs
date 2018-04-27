using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : MonoBehaviour {

	public bool isMounted = false;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log ("mounting");
		if (col.gameObject.GetComponent<GObject>() && !isMounted) {
			setMounted (col.gameObject);
			Camera.main.GetComponent<CameraCode> ().setIsDraggable (true);

		}
	}


	public void setMounted(GameObject obj){
		if (obj == null) {
			isMounted = false;
			return;
		}
		isMounted = true;
		obj.transform.position = new Vector3 (transform.position.x, transform.position.y + 4, transform.position.z);
		obj.GetComponent<GObject> ().isDragged = false;
		obj.GetComponent<GObject> ().isDraggable = false;
		obj.GetComponent<GObject>().mount = this.gameObject;


	}
	// Update is called once per frame
	void Update () {
		
	}
}
