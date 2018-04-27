using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Balloon : MonoBehaviour {

	protected int health;
//
	public Sprite redBalloon;
	public Sprite blueBalloon;	
	public Sprite yellowBalloon;
//
	public Vector2 startPos;
	public bool floatingUp;

	protected virtual void Awake(){
		setColour();
		startPos = transform.position;
		floatingUp = true;
			
	}

	protected virtual void Update(){

		if (floatingUp == true) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + 0.8f * Time.deltaTime, transform.position.z);
		} else {
			transform.position = new Vector3 (transform.position.x, transform.position.y - 0.8f * Time.deltaTime, transform.position.z);
		}

		if (transform.position.y > startPos.y + 0.1f) {
			floatingUp = false;
		}else if (transform.position.y < startPos.y - 0.1f) {
			floatingUp = true;
		}


	}

	protected void setColour(){
		Sprite finalSprite = redBalloon;

		switch (health) {
		case 1:
			finalSprite = redBalloon;
			break;
		case 2: 
			finalSprite = blueBalloon;
			break;
		case 3: 
			finalSprite = yellowBalloon;
			break;
		default:
			Destroy (gameObject);
			return;

		}
		GetComponent<SpriteRenderer> ().sprite = finalSprite;

	}

	

	public int getHealth(){
		return health;
	}

	public void hit(int damage){
		Debug.Log ("destroying balloon");
		health -= damage;
		if (health <= 0) {
			Destroy (gameObject);
		} else {
			setColour ();
		}
	}





}
