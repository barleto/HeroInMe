using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IPlayerController {
	
	public float speed;
	public float shotSpeed;
	public GameObject Weapon;
	public GameObject projectile;
	public GameObject meleeWeapon;

	private float currentSpeed;
	private float walkingTimer = 2;
	private Rigidbody2D myBody;
	private Animator animator;
	private bool facingRight = true;
	private bool inAir = false;
	private bool isWalking = false;
	private bool isRunning = false;

	void Awake () {
		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
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
//		movement.Normalize (); //normalizes the vector
		//Horizontal movement or stop walking movement
		if (movement.y == 0) {
			// Changes through player animations
			if (movement.x == 0) {
				if(isWalking){
					animator.SetBool ("Walking", false);
					isWalking = false;
				} else if(isRunning){
					animator.SetBool("Running", false);
					isRunning = false;
					currentSpeed = speed;
				}
				walkingTimer = 2;

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
	
	public void AttackMelee(int state){
		if (state == 0) {
			animator.SetTrigger ("Attacking");
		} else if (state == 1) {
			animator.SetTrigger ("Combo1");
		} else {
			animator.SetTrigger ("Combo2");
		}
	}

	public 	void AttackRanged(Vector2 direction){
		direction.Normalize ();
		GameObject clone = (GameObject) Instantiate(projectile, transform.position, transform.rotation);
		Rigidbody2D shotRigidbody = clone.GetComponent<Rigidbody2D>();

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		clone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		
		shotRigidbody.velocity = new Vector2(direction.x*shotSpeed, direction.y*shotSpeed);
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
			animator.SetTrigger("Landing");
			animator.SetBool("Falling", false);
		} else if (col.gameObject.CompareTag("PickUp")) {
			col.gameObject.GetComponent<TriggerObjectController>().Action();
		} else if (col.gameObject.CompareTag("DeathTrigger")) {
			transform.position = new Vector3 (0, 2, 0);
		} else if(col.gameObject.CompareTag("Key")){
			col.gameObject.GetComponent<CoinController>().Action();
		} 
			animator.SetBool ("Falling", false);
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
<<<<<<< HEAD
	
	public void EquipItem () {

		GameObject sword = (GameObject) Instantiate(meleeWeapon, Weapon.transform.position, Weapon.transform.rotation);
		sword.transform.parent = Weapon.transform;

		BoxCollider2D swordCollider2D = sword.GetComponent<BoxCollider2D> ();	
		BoxCollider2D weaponBoxCollider2D = Weapon.GetComponent<BoxCollider2D>();
		weaponBoxCollider2D.size = swordCollider2D.size;
		weaponBoxCollider2D.offset = swordCollider2D.offset;
		swordCollider2D.enabled = false;
=======


	public void EquipItem (Sprite sprite) {
		
		weaponSpriteRenderer.sprite = sprite;
		weaponBoxCollider2D.size = weaponSpriteRenderer.bounds.size;
		weaponBoxCollider2D.offset = weaponSpriteRenderer.bounds.center - weaponBoxCollider2D.bounds.center;
		weaponBoxCollider2D.enabled = false;
>>>>>>> dekkoh
	}

}