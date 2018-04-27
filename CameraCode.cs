using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCode : MonoBehaviour {

	private GameObject currentArrow;
	private Vector3 startPos;

	private bool manualCam = false;
	private bool introCam = true;
	public List<Vector3> introPositions = new List<Vector3> ();

	public List<GameObject> Layer0 = new List<GameObject>();
	public List<GameObject> Layer1 = new List<GameObject>();
	public List<GameObject> Layer2 = new List<GameObject>();

	private Vector3 clickPosition = Vector3.zero;
	private bool isDragged = false;
	private bool isDraggable = true;

	private bool timerOn = true;
	private float timeLeft = 1.5f;

	public GameObject backgroundImage;
	private Vector2 LTVertex;
	private Vector2 RBVertex;
	public float minZoom;
	public float maxZoom;

	// Use this for initialization
	void Start () {
		if (backgroundImage != null) {
			SpriteRenderer render = backgroundImage.GetComponent<SpriteRenderer> ();
			Vector3 center = render.bounds.center;
			float x = render.bounds.size.x;
			float y = render.bounds.size.y;
			LTVertex = new Vector2 (center.x - x / 2, center.y + y / 2);
			RBVertex = new Vector2 (center.x + x / 2, center.y - y / 2);
		} else {
			LTVertex = new Vector2 (-40, 40);
			RBVertex = new Vector2 (40, -40);
		}


		Player player = GameControl.controller.getPlayer ();
		startPos = transform.position;
		if (player != null) {
			startPos = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
		} 
		introPositions.Add (new Vector3(startPos.x, startPos.y, -10));
		transform.position = new Vector3 (introPositions[0].x, introPositions[0].y, transform.position.z);
	}



	// Update is called once per frame
	void LateUpdate () {
		//Debug.Log (introCam + " " + manualCam + " " + isDragged + isDraggable);
		if (introCam) {
			isDraggable = false;
			if (Vector2.Distance(introPositions[0], transform.position) < 0.5f  && !timerOn) {
				Debug.Log ("turning timer on");
				timerOn = true;
				timeLeft = 0.5f;
			}
			if (timerOn) {
				timeLeft -= Time.deltaTime;
				if (timeLeft <= 0) {
					Debug.Log ("turning timer off");
					timerOn = false;
					introPositions.RemoveAt (0);
					if (introPositions.Count == 0) {
						introCam = false;
						isDraggable = true;
					}

				}
			} else {
				transform.position = moveTowards (introPositions[0]);
			}

		}
		if (manualCam == true || introCam == true) return;

		if(currentArrow)
		Debug.Log (currentArrow.GetComponent<Projectile>().isMoving() + " " + isDragged);

		if (currentArrow == null) {
			transform.position = moveTowards (startPos);
		}else if(currentArrow.GetComponent<Projectile>().isMoving() == false && isDragged == false){
			//if the arrow has landed the user has the option to drag the camera again, if he hasnt,
			//keep moving the camera towards the arrow.
			transform.position = moveTowards (currentArrow.transform.position);

			//if timer is not on, turn it on. else, if the time has run out remove the current arrow and 
			//turn timer off. if the user has dragged the screen after the arrow has landed, turn of the timer.
			if (!timerOn) {
				timerOn = true;
				timeLeft = 2;
			} else {
				timeLeft -= Time.deltaTime;
				if (isDragged) {
					timerOn = false;
					timeLeft = 2;
				}else if (timeLeft < 0) {
					currentArrow = null;
					timerOn = false;
					timeLeft = 2;

				}
			}
				
		//if current arrow is not null, move towards it.
		}else if (currentArrow != null && !isDragged) {
			transform.position = moveTowards (currentArrow.transform.position);
		}

	}



	public void dragCamera(){
		if(isDraggable){
			if(Input.GetMouseButtonDown(0)){
				isDragged = true;
				manualCam = true;

				clickPosition = Input.mousePosition;

			}else if(Input.GetMouseButtonUp(0)){

				clickPosition = Vector3.zero;
				isDragged = false;
			}

			if (clickPosition != Vector3.zero) {

				float worldDistX = Camera.main.ScreenToViewportPoint (new Vector3(clickPosition.x - Input.mousePosition.x, 0, 0)).x;
				float worldDistY = Camera.main.ScreenToViewportPoint (new Vector3(clickPosition.y - Input.mousePosition.y, 0, 0)).x; //why is this x?

				float newX = transform.position.x + worldDistX * 35;
				float newY = transform.position.y + worldDistY * 35;

				updateParallax (transform.position.x - newX, transform.position.y - newY);
				gameObject.transform.position = new Vector3 (newX, newY, transform.position.z);
				Debug.Log ("new position: " + newX + " " + newY);
				clickPosition = Input.mousePosition;

			}

			if (Input.touchCount == 1) {
				float rightBorder = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, 0, Camera.main.nearClipPlane)).x;
				float leftBorder = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane)).x;
				float topBorder = Camera.main.ScreenToWorldPoint (new Vector3 (0, Camera.main.pixelHeight, Camera.main.nearClipPlane)).y;
				float botBorder = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane)).y;

				isDragged = true;
				manualCam = true;

				Touch touch = Input.GetTouch (0);
				Vector2 change = touch.deltaPosition * Time.deltaTime;
				if (leftBorder - change.x > LTVertex.x && rightBorder - change.x < RBVertex.x &&
					botBorder - change.y > RBVertex.y && topBorder - change.y < LTVertex.y) {
					updateParallax (change.x, change.y);
					gameObject.transform.position = new Vector3 (transform.position.x - change.x, transform.position.y - change.y, 
						transform.position.z);
				}
			}





		}
	}
		
	//moves the camera towards the arrow at a speed corresponding to the distance of the camera
	//from the arrow.


	Vector3 moveTowards(Vector3 pos){
		float distanceX = pos.x - transform.position.x;
		float distanceY = pos.y - transform.position.y;
		if (distanceX < 0.1f && distanceX > -0.1f) distanceX = (pos.x - transform.position.x);
		if (distanceY < 0.1f && distanceY > -0) distanceY = (pos.y - transform.position.y);

		distanceX *= Time.deltaTime*2;
		distanceY *= Time.deltaTime*2;

 		//updates the background
		updateParallax (distanceX, 0);
		return new Vector3 (transform.position.x + distanceX, transform.position.y + distanceY, transform.position.z);
	}

	//moves the background objects relative to their distance from the camera.
	private void updateParallax(float distanceX, float distanceY){
		float speed;

		//moves the quickly
		foreach (GameObject bg in Layer0) {
			speed = 0.4f;
			Vector3 position = bg.transform.position;
			bg.transform.position = new Vector3 (position.x + distanceX*speed, position.y + distanceY * speed/4, position.z);
		}

		//moves medium
		foreach (GameObject bg in Layer1) {
			speed = 0.2f;
			Vector3 position = bg.transform.position;
			bg.transform.position = new Vector3 (position.x + distanceX*speed, position.y + distanceY * speed/4, position.z);
		}

		//moves at a slow speed.
		foreach (GameObject bg in Layer2) {
			speed = 0.05f;
			Vector3 position = bg.transform.position;
			bg.transform.position = new Vector3 (position.x + distanceX*speed, position.y + distanceY * speed/4, position.z);
		}
	}

	public void changeZoom(){
		float orthoZoomSpeed = 0.5f;

		// If there are two touches on the device...
		if (Input.touchCount == 2)
		{

			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// ... change the orthographic size based on the change in distance between the touches.
			Camera.main.orthographicSize += (deltaMagnitudeDiff * orthoZoomSpeed * Time.deltaTime);

			// Make sure the orthographic size never drops below zero.
			Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, minZoom);
			Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, maxZoom);


			float rightBorder = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, 0, Camera.main.nearClipPlane)).x;
			float leftBorder = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane)).x;
			float topBorder = Camera.main.ScreenToWorldPoint (new Vector3 (0, Camera.main.pixelHeight, Camera.main.nearClipPlane)).y;
			float botBorder = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane)).y;

			if (leftBorder < LTVertex.x) {
				float dif = LTVertex.x - leftBorder;

				transform.position = new Vector3 (transform.position.x + dif, transform.position.y, transform.position.z);
			}if(rightBorder > RBVertex.x){
				Debug.Log ("right border is bigger");
				float dif = RBVertex.x - rightBorder;
				transform.position = new Vector3 (transform.position.x + dif, transform.position.y, transform.position.z);

			}if(botBorder < RBVertex.y){
				Debug.Log ("bot border is bigger");
				float dif = RBVertex.y - botBorder;
				transform.position = new Vector3 (transform.position.x, transform.position.y + dif, transform.position.z);

			}if(topBorder > LTVertex.y){
				Debug.Log ("top border is bigger");
				float dif = LTVertex.y - topBorder;
				transform.position = new Vector3 (transform.position.x, transform.position.y + dif, transform.position.z);

			}
		}
	
	}


	//set current arrow
	public void setCurrentArrow(GameObject arrow){
		currentArrow = arrow;
		if(currentArrow != null)
			manualCam = false;

	}

	public void setIsDragged(bool b){
		isDragged = b;
	}
	public bool getIsDragged(){
		return isDragged;
	}

	public void setIsDraggable(bool b){
		isDraggable = b;
	}
	public bool getIsDraggable(){
		return isDraggable;
	}

	public void setManualCam(bool b){
		manualCam = b;
	}
	public bool getManualCam(){
		return manualCam;
	}



}
