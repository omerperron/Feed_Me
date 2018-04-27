using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public const int ARROW = 0;
	public const int KAMAKAZE = 1;
	public const int CLUSTER = 2;
	public const int BOUNCY = 3;
	public const int ANTI_GRAVITY = 4;
	public const int CONTROL = 5;

	protected Vector2 direction;
	protected int power = 1;
	protected float speed = 12;
	protected Rigidbody2D rb;
	protected bool moving = true;
	private int rotationSpeed;

	private GameObject pointLight;

	List<GameObject> path = new List<GameObject> ();


	protected virtual void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		rb.bodyType = RigidbodyType2D.Dynamic;
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Arrow"));
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Arrow"), LayerMask.NameToLayer("Arrow"));

		pointLight = Instantiate ((GameObject)Resources.Load ("PointLight"), Vector3.zero, Quaternion.identity) as GameObject;
		pointLight.transform.parent = gameObject.transform;
		pointLight.transform.position = new Vector3 (transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x/2,
			transform.position.y, -1);
	}


		

	public virtual void launch (Vector2 dir, float launchSpeed) {
		this.speed = launchSpeed;
		direction = dir;
		direction.Normalize();
		rb.velocity = direction*speed;

		rotationSpeed = Random.Range ( -150 , 150 );



	}

	// Update is called once per frame
	protected virtual void Update () {
		Vector3 rotationVector = transform.rotation.eulerAngles;
		rotationVector.z += rotationSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(rotationVector);
		
		if (moving) {
			//rotateArrow ();
			//pointLight.transform.position = new Vector3 (transform.position.x, transform.position.y, -1);
		}


		if (transform.position.y < -20) {
			if (Camera.main.GetComponent<CameraCode> ())
				Camera.main.GetComponent<CameraCode> ().setCurrentArrow (null);
			Destroy (gameObject);
			deletePath ();
		}
	}

	private void rotateArrow(){
		float x = rb.velocity.x;
		float y = rb.velocity.y;
		float angle = y / x;

		float fAngle = (Mathf.Atan (angle) * Mathf.Rad2Deg);
		if (x < 0) fAngle += 180;
		
		gameObject.transform.rotation = Quaternion.Euler(0, 0, fAngle);


	}

	public void drawPath(){
		if (path.Count == 0) {
			GameObject newPathA = Instantiate ((GameObject)Resources.Load ("ProjectilePath"), transform.position, transform.rotation) as GameObject;
			path.Add (newPathA);
			return;
		}

		GameObject lastPath = path[path.Count-1];
		if (Vector3.Distance (lastPath.transform.position, gameObject.transform.position) > 3) {
			GameObject newPath = Instantiate ((GameObject)Resources.Load ("ProjectilePath"), transform.position, transform.rotation) as GameObject;
			path.Add (newPath);
		}
	
	}
		
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Balloon") {
			hitBalloon (col.gameObject);

		} else if (col.gameObject.tag == "Portal") {
			hitPortal (col.gameObject);
		} else if (col.gameObject.tag == "Package") {
			hitPackage (col.gameObject);
		} else if (col.gameObject.tag == "Booster") {
			hitBooster (col.gameObject);
		}else if (col.gameObject.tag == "Cannon") {
			hitCannon (col.gameObject);
		}else if (col.gameObject.tag == "Pipe") {
			hitPipe (col.gameObject);
		}
			
	}



	void OnCollisionEnter2D (Collision2D col){
		Bouncy bouncy = this.GetComponent<Bouncy>();
		if (bouncy) {
			hitReflector (col);
			if (col.gameObject.tag != "Reflector")
				bouncy.bounce (col);
		} else {
			//reduce magnitude
		}

		if (col.gameObject.tag == "Target") {
			targetHit (col.gameObject);

		} else if (col.gameObject.tag == "Reflector") {
			hitReflector (col);

		} else if (col.gameObject.tag == "Button") {
			hitButton (col.gameObject);
		}else {
			if(!bouncy) setStationary (col.gameObject);
			Debug.Log ("hit something else " + col.gameObject.name);

		}




	}


	protected void setStationary(GameObject hit){
		moving = false;	
		//gameObject.GetComponent<BoxCollider2D> ().isTrigger  = true;
//		transform.parent = hit.transform;
//
//		moving = false;
//		rb.bodyType = RigidbodyType2D.Static;
//		GetComponent<BoxCollider2D> ().isTrigger = true;
//		removeLight ();
	}

	public void deletePath(){
		foreach (GameObject projectile in path) {
			Destroy (projectile);

		}
	}



	void targetHit(GameObject target){
//		transform.parent = target.transform;
		Debug.Log ("target hit");
		target.GetComponent<Target> ().hit();

		GameControl.controller.checkIfWon ();
		moving = false;
	}

	void hitBooster(GameObject booster){
		booster.GetComponent<Booster> ().boost (this);
	}

	void hitButton(GameObject button){
		Debug.Log ("button hit");
		Button but = button.GetComponent<Button> ();
		but.buttonPressed ();
		setStationary (button);
	}

	void hitBalloon(GameObject balloon){
		int origPower = power;
		power -= balloon.GetComponent<Balloon> ().getHealth ();
		if (power <= 0) {
			moving = false;
			rb.bodyType = RigidbodyType2D.Static;
			GetComponent<SpriteRenderer> ().enabled = false;
			//removeLight ();

			//Destroy (gameObject);
		}
		balloon.GetComponent<Balloon> ().hit (origPower);
	}


	void hitPackage(GameObject pack){

		string type = pack.GetComponent<Package> ().getObject ();
		Camera.main.GetComponent<CameraCode> ().setCurrentArrow (null);
		this.moving = false;

		if (type == "Cluster") {
			GameObject cluster = new GameObject ("cluster");
			cluster.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			cluster.AddComponent<Cluster>();
			cluster.GetComponent<Cluster>().launch(rb.velocity);
		} else {
			GameObject projectile = Instantiate ((GameObject)Resources.Load (type), 
				transform.position, Quaternion.identity) as GameObject;
			Camera.main.GetComponent<CameraCode> ().setCurrentArrow (projectile);
			projectile.GetComponent<Rigidbody2D> ().velocity = new Vector2(rb.velocity.x, rb.velocity.y);
			GameControl.controller.addArrow (projectile.GetComponent<Projectile>());

		}
		Destroy (pack);
		DestroyObject (gameObject);


	}

	void hitCannon(GameObject cannon){
		Cannon cannonScript = cannon.GetComponent<Cannon> ();
		cannonScript.setArrow (gameObject);
			
		
	}

	void hitPortal(GameObject portal){
		Portal portalScript = portal.GetComponent <Portal> ();
		portalScript.teleport (this.gameObject);

	}

	void hitPipe(GameObject pipe){
		Debug.Log ("hello");
		Pipe pipeScript = pipe.GetComponent <Pipe> ();
		GameObject partner = pipeScript.getCompanion();
		Vector3 partnerPos = partner.transform.position;
		Quaternion partnerRot = partner.transform.rotation;
		Vector3 partnerRotE = partnerRot.eulerAngles;
		Debug.Log("partner rot: " +  partnerRot);
		Debug.Log("partner rotE: " +  partnerRotE);
		float zAngle = partnerRotE.z * Mathf.Deg2Rad;
		Debug.Log("z: " + zAngle);
		if (partnerPos != Vector3.zero) {
			transform.position = new Vector3 (partnerPos.x, partnerPos.y, partnerPos.z);
			transform.rotation = partnerRot;
			float x = Mathf.Cos(zAngle);
			float y = Mathf.Sin (zAngle);
			rb.velocity = (new Vector2 (x, y) * rb.velocity.magnitude);
	
			//rb.velocity = direction * 100;
		}
	}

	void hitReflector(Collision2D col){
		Vector2 normal = col.contacts [0].normal;
		// reflect our old velocity off the contact point's normal vector
		Vector3 reflectedVelocity = Vector3.Reflect (oldVelocity, normal);        

		// assign the reflected velocity back to the rigidbody
		rb.velocity = reflectedVelocity;
		// rotate the object by the same ammount we changed its velocity
		Quaternion rotation = Quaternion.FromToRotation (oldVelocity, reflectedVelocity);
		transform.rotation = rotation * transform.rotation;
	}

	private Vector3 oldVelocity;
	void FixedUpdate () {
		oldVelocity = rb.velocity;
	}

	public bool isMoving(){
		return moving;
	}


	public static string getProjectileName(int i){
		switch (i) {
		case 0:
			return "Arrow";
		case 1:
			return "Kamakaze";
		case 2: 
			return "Cluster";
		case 3: 
			return "Bouncy";
		case 4: 
			return "AntiGravity";
		case 5: 
			return "Controlled";

		}
		return "";

	}





}