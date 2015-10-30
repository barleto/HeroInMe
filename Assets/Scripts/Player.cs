using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IPlayerController {

	public float speed;
	public GameObject Weapon;

	private Rigidbody2D myBody;
	private Animator animator;
	private bool facingRight = true;
	private bool inAir = false;
	private SpriteRenderer weaponSpriteRenderer;
	private BoxCollider2D weaponBoxCollider2D;

	void Awake () {
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		weaponSpriteRenderer = Weapon.GetComponent<SpriteRenderer>();
		weaponBoxCollider2D = Weapon.GetComponent<BoxCollider2D>();
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

	public void attack(){
		animator.SetTrigger ("Attacking");
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
		} else if (col.gameObject.CompareTag("PickUp")) {
			col.gameObject.GetComponent<TriggerObjectController>().Action();
		} else if (col.gameObject.CompareTag("DeathTrigger")) {
			transform.position = new Vector3 (0, 2, 0);
		} else if(col.gameObject.CompareTag("Key")){
			col.gameObject.GetComponent<CoinController>().Action();
		} 
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground") {
			inAir = true;
			animator.SetBool("Falling", true);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if(col.transform.CompareTag("Platform")) {
			transform.parent = col.transform;
		}
	}
	
	void OnCollisionExit2D (Collision2D col) {
		if (col.transform.CompareTag ("Platform")) {
			transform.SetParent(null);
		}
	}

	public void EquipItem (Sprite sprite) {
		
		weaponSpriteRenderer.sprite = sprite;
		weaponBoxCollider2D.size = weaponSpriteRenderer.bounds.size;
		weaponBoxCollider2D.offset = weaponSpriteRenderer.bounds.center - weaponBoxCollider2D.bounds.center;
		weaponBoxCollider2D.enabled = false;
	}
}
