using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IPlayerController {

	public float speed;

	private Rigidbody2D myBody;
	private Animator animator;
	private bool facingRight = true;
	private bool inAir = false;

	void Awake () {
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	// Will move the player given a movement vector
	public void movePlayer(Vector2 movement){
		movement.Normalize (); //normalizes the vector
		//Horizontal movement or stop walking movement
		if (Mathf.Abs (movement.x) > movement.y || (movement.x == 0 && movement.y == 0)) {
			myBody.velocity = new Vector2 (speed * movement.x, myBody.velocity.y);
			if (movement.x == 0) {
				animator.SetBool ("Walking", false);
			} else {
				animator.SetBool ("Walking", true);
			
				if ((facingRight && movement.x < 0) || (!facingRight && movement.x > 0)) {
					Flip ();
				}
			}
		} else if(inAir == false){ //Vertical movement
			myBody.velocity = new Vector2(myBody.velocity.x, 5*speed);
			animator.SetTrigger("Jumping");
			inAir = true;
		}
	}

	// Flips player sprites scale
	void Flip (){
		facingRight = !facingRight;
		Vector3 newScale = transform.localScale;
		newScale.x *= -1;
		transform.localScale = newScale;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			inAir = false;
			animator.SetTrigger("Landing");
			animator.SetBool("Falling", false);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			inAir = true;
			animator.SetBool("Falling", true);
		}
	}
}
