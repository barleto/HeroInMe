using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IPlayerController {
	
	public float speed;
	public float shotSpeed;
	public float walkingDuration;
	public GameObject Weapon;
	public GameObject projectile;
	public GameObject meleeWeapon;

	private Rigidbody2D myBody;
	private Animator animator;
	private CircleCollider2D sphereCollider;
	private bool pause = false;
	private bool facingRight = true;
	private bool inAir = false;
	private bool isWalking = false;

	private bool alreadyStopped = false;

	private bool isCastingRangedAttack = false;

	private bool grounded = true;
	private int comboCount = -1;
	private float comboWindow = 1.0f;
	private float comboTimer;
	private float currentSpeed;
	private float walkingTimer;

	void Awake () {

		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sphereCollider = GetComponent<CircleCollider2D>();
		animator.SetBool ("Grounded", true);
		currentSpeed = speed;
		walkingTimer = walkingDuration;
	}

	void FixedUpdate(){
		//Timer until player starts running
		if (isWalking) {
			walkingTimer -= Time.deltaTime;
			if(walkingTimer <= 0 && currentSpeed < 7 ){
				walkingTimer = walkingDuration;
				currentSpeed++;
			}
		//Timer for players combo
		} else if (comboCount >= 0) {
			comboTimer -= Time.deltaTime;
			if (comboTimer <= 0) {
				comboCount = -1;
				comboTimer = comboWindow;
				animator.SetBool ("Combo1", false);
				animator.SetBool ("Combo2", false);
			}
		}
	}

	// Will move the player given a movement vector
	public void MovePlayer(Vector2 movement){

		if (pause == false) {
			//Horizontal movement
			if (movement.y == 0) {
				// Sets player movement
				isWalking = true;
				myBody.velocity = new Vector2 (currentSpeed * movement.x, myBody.velocity.y);
				animator.SetFloat ("Speed", Mathf.Abs (movement.x) * currentSpeed);

				if ((facingRight && movement.x < 0) || (!facingRight && movement.x > 0)) {
					Flip ();
				} else if (movement.x == 0) { // Player stoped
					isWalking = false;
					walkingTimer = walkingDuration;
					currentSpeed = speed;
				}

			} else if (grounded == true) { //Vertical movement
				myBody.velocity = new Vector2 (myBody.velocity.x, 3 * speed);
				animator.SetTrigger ("Jump");
				grounded = false;
			}
		}
	}
	
	public void AttackMelee(){
		if (pause == false) {
			comboCount++;
			if (comboCount >= 2 && animator.GetCurrentAnimatorStateInfo (0).IsName ("PlayerAttacking3")) {
				comboCount = 2;
				animator.SetBool ("Combo1", false);
				animator.SetBool ("Combo2", false);
			} else if (comboCount > 2) {
				comboCount = 0;
			}

			if (comboCount == 0) {
				animator.SetTrigger ("Attack");
			} else if (comboCount == 1) {
				comboTimer = animator.GetCurrentAnimatorStateInfo (1).length;
				animator.SetBool ("Combo1", true);
			} else if (comboCount == 2) {
				comboTimer += animator.GetCurrentAnimatorStateInfo (1).length;
				animator.SetBool ("Combo2", true);
			}
		}
	}

	public 	void AttackRanged(Vector2 direction){

		isCastingRangedAttack = false;

		direction.Normalize ();
		GameObject clone = (GameObject) Instantiate(projectile, transform.position, transform.rotation);
		Rigidbody2D shotRigidbody = clone.GetComponent<Rigidbody2D>();

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		clone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		
		shotRigidbody.velocity = new Vector2(direction.x*shotSpeed, direction.y*shotSpeed);
	}

	public void CastRangedAttack (Vector2 direction) {

		isCastingRangedAttack = true;

		//Vira o player para o sentido em que está mirando
		if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0)) {
			Flip ();
		}

		//Para o player caso esteja em movimento
		if ( isWalking) {
			//currentSpeed = 0;
			MovePlayer(new Vector2(0, 0));
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

		if (col.gameObject.CompareTag("PickUp")) {
			col.gameObject.GetComponent<TriggerObjectController>().Action();
		} else if (col.gameObject.CompareTag("DeathTrigger")) {
			transform.position = new Vector3 (0, 2, 0);
		} else if(col.gameObject.CompareTag("Key")){
			col.gameObject.GetComponent<CoinController>().Action();
		} 
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground" || col.gameObject.CompareTag ("Platform")) {
			grounded = true;
			animator.SetBool ("Grounded", grounded);
		} else if (col.gameObject.CompareTag("PickUp")) {
			col.gameObject.GetComponent<TriggerObjectController>().Action();
		} else if (col.gameObject.CompareTag("DeathTrigger")) {
			transform.position = new Vector3 (0, 2, 0);
		} else if(col.gameObject.CompareTag("Key")){
			col.gameObject.GetComponent<CoinController>().Action();
		} 
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "ground" || col.gameObject.CompareTag("Platform")) {
			if(!col.IsTouching(sphereCollider)){
				grounded = false;
				animator.SetBool("Grounded", grounded);
			}
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
	
	public void EquipItem () {

		GameObject sword = (GameObject) Instantiate(meleeWeapon, Weapon.transform.position, Weapon.transform.rotation);
		sword.transform.parent = Weapon.transform;

		BoxCollider2D swordCollider2D = sword.GetComponent<BoxCollider2D> ();	
		BoxCollider2D weaponBoxCollider2D = Weapon.GetComponent<BoxCollider2D>();
		weaponBoxCollider2D.size = swordCollider2D.size;
		weaponBoxCollider2D.offset = swordCollider2D.offset;
		swordCollider2D.enabled = false;

	}

	public void Pause(){
		pause = !pause;
		if(pause == true){
			animator.SetFloat("Speed", 0f);
			animator.SetBool("Combo1", false);
			animator.SetBool("Combo2", false);
		}
	}
}