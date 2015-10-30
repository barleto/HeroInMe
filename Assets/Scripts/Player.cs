using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IPlayerController {
	
	public float speed;
	public GameObject Weapon;

	private float currentSpeed;
	private float walkingTimer = 2;
	private Rigidbody2D myBody;
	private Animator animator;
	private bool facingRight = true;
	private bool inAir = false;
	private bool isWalking = false;
	private bool isRunning = false;
	private SpriteRenderer weaponSpriteRenderer;
	private BoxCollider2D weaponBoxCollider2D;

	void Awake () {
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		weaponSpriteRenderer = Weapon.GetComponent<SpriteRenderer>();
		weaponBoxCollider2D = Weapon.GetComponent<BoxCollider2D>();
		currentSpeed = speed;
	}

	void FixedUpdate(){
		if (isWalking) {
			walkingTimer -= Time.deltaTime;
			if(walkingTimer <= 0){
				walkingTimer = 0;
				isRunning = true;
			}
		}
	}

	// Will move the player given a movement vector
	public void MovePlayer(Vector2 movement){
		movement.Normalize (); //normalizes the vector
		//Horizontal movement or stop walking movement
		if (Mathf.Abs (movement.x) > movement.y || (movement.x == 0 && movement.y == 0)) {
			// Changes through player animations
			if (movement.x == 0) {
				if(isWalking){
					animator.SetBool ("Walking", false);
					isWalking = false;
				} else if(isRunning){
					animator.SetBool("Running", false);
					isRunning = false;
					currentSpeed = speed;
					walkingTimer = 2;
				}
			} else if(!isRunning){
				animator.SetBool ("Walking", true);
				isWalking = true;
				
				if ((facingRight && movement.x < 0) || (!facingRight && movement.x > 0)) {
					Flip ();
				}
			} else {
				animator.SetBool("Running", true);
				currentSpeed = speed + 2;
				if ((facingRight && movement.x < 0) || (!facingRight && movement.x > 0)) {
					Flip ();
				}
			}
			// Sets player movement
			myBody.velocity = new Vector2 (currentSpeed * movement.x, myBody.velocity.y);

		} else if(inAir == false){ //Vertical movement
			myBody.velocity = new Vector2(myBody.velocity.x, 3*speed);
			animator.SetTrigger("Jumping");
			inAir = true;
		}
	}
	
	public void Attack(){
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
		if (col.gameObject.CompareTag("PickUp")) {
			col.gameObject.GetComponent<TriggerObjectController>().Action();
		} else if (col.gameObject.CompareTag("DeathTrigger")) {
			transform.position = new Vector3 (0, 2, 0);
		} else if(col.gameObject.CompareTag("Key")){
			Debug.Log("to aqui");
			col.gameObject.GetComponent<CoinController>().Action();
		} 
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground" || col.gameObject.CompareTag ("Platform")) {
			if (inAir) {
				animator.SetTrigger ("Landing");
			}
			inAir = false;
			animator.SetBool ("Falling", false);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground" || col.gameObject.CompareTag("Platform")) {
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