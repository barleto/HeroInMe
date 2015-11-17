using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IPlayerController {
	
	public float speed;
	public float shotSpeed;
	public float walkingDuration;
	public int hp = 3;
	public GameObject Weapon;
	public GameObject projectile;
	public GameObject meleeWeapon;
	public GameObject castingHands;
	public GameObject hpUI;
	public GameObject deathAnimationBody;

	private Rigidbody2D myBody;
	private Animator animator;
	private CircleCollider2D sphereCollider;
	private HPController hpController;
	private bool pause = false;
	private bool facingRight = true;
	private bool isWalking = false;
	private bool isCastingRangedAttack = false;
	private bool grounded = true;
	private bool isDead = false;
	private bool inCombo = false;
	private int comboCount = -1;
	private float comboWindow = 0.5f;
	private float comboTimer;
	private float currentSpeed;
	private float walkingTimer;
	private GameObject deathAnimation;

	void Awake () {

		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sphereCollider = GetComponent<CircleCollider2D>();
		animator.SetBool ("Grounded", true);
		currentSpeed = speed;
		walkingTimer = walkingDuration;
		hpController = hpUI.GetComponent<HPController>();
	}

	void FixedUpdate(){
		//Timer until player starts running
		if (isWalking) {
			walkingTimer -= Time.deltaTime;
			if(walkingTimer <= 0 && currentSpeed < 7 ){
				walkingTimer = walkingDuration;
				currentSpeed++;
			}
		}
		//Timer for players combo
		if (comboCount >= 0) {
			comboTimer -= Time.deltaTime;
			if (comboTimer <= 0) {
				comboCount = -1;
				comboTimer = comboWindow;
				animator.SetBool ("Combo1", false);
				animator.SetBool ("Combo2", false);
				inCombo = false;
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
				animator.SetTrigger ("Jump");
				myBody.velocity = new Vector2 (myBody.velocity.x, 3 * speed);
			}
		}
	}
	
	public void AttackMelee(){
		if (pause == false && !isDead) {
			comboCount++;
			if(animator.GetCurrentAnimatorStateInfo(1).IsName("PlayerAttacking1")){
				comboCount = 1;
			} else if(animator.GetCurrentAnimatorStateInfo(1).IsName("PlayerAttacking2")){
				comboCount = 2;
			}
			if (comboCount > 2 && inCombo) {

				comboCount = 3;
			} else if (comboCount > 2) {
				comboCount = 0;
				inCombo = false;
			}

			if (comboCount == 0) {
				animator.SetTrigger ("Attack");
			} else if (comboCount == 1) {
				comboTimer = animator.GetCurrentAnimatorStateInfo (1).length;
				animator.SetBool ("Combo1", true);
				inCombo = true;
			} else if (comboCount == 2) {
				comboTimer = animator.GetCurrentAnimatorStateInfo (1).length;
				animator.SetBool ("Combo2", true);
				inCombo = true;
			}
		}
	}

	public 	void AttackRanged(Vector2 direction){

		direction.Normalize ();
		GameObject clone = (GameObject) Instantiate(projectile, castingHands.transform.position, castingHands.transform.rotation);
		Rigidbody2D shotRigidbody = clone.GetComponent<Rigidbody2D>();

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		clone.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		shotRigidbody.velocity = new Vector2(direction.x*shotSpeed, direction.y*shotSpeed);
		animator.SetTrigger("Shot");
		animator.SetFloat("Casting", 0f);
	}

	public void CastRangedAttack (Vector2 direction, float duration) {

		if (isDead) {
			return;
		}

		animator.SetFloat("Casting", duration);
//		if(Weapon.transform.childCount >0){
//			Debug.Log("achei");
//		}
		//Vira o player para o sentido em que está mirando
		if ((facingRight && direction.x < 0) || (!facingRight && direction.x > 0)) {
			Flip ();
		}

		//Para o player caso esteja em movimento
		if ( isWalking) {
			MovePlayer(new Vector2(0, 0));
		}

		if(animator.GetCurrentAnimatorStateInfo(1).IsName("PlayerReadyToShoot")){
			float angle = Mathf.Atan2(direction.y, direction.x);
			float sin = Mathf.Sin(angle);
			animator.SetFloat("CastAngle", sin);
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
			Destroy(col.gameObject);
			this.EquipItem();
		} else if (col.gameObject.CompareTag("DeathTrigger") && !isDead) {
			DeathAnimation();
			Invoke ("Resurrect", 3);
		} else if(col.gameObject.CompareTag("Key")){
			col.gameObject.GetComponent<CoinController>().Action();
		} 
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground" || col.gameObject.CompareTag ("Platform")) {
			grounded = true;
			animator.SetBool ("Grounded", grounded);
//		} else if (col.gameObject.CompareTag("PickUp")) {
//			col.gameObject.GetComponent<TriggerObjectController>().Action();
		} else if (col.gameObject.CompareTag("DeathTrigger") && !isDead) {
			DeathAnimation();
			Invoke ("Resurrect", 3);
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
		Weapon.GetComponent<SwordScript>().damage = sword.GetComponent<SwordScript>().damage;
		swordCollider2D.enabled = false;

	}

	public void TakeDamage(){
		hp--;
		if(hp == 0){
			//Kill Player
		}
	}

	public void Pause(){
		pause = !pause;
		if(pause == true){
			//REMINDER: Setar os triggers do animator
			animator.SetFloat("Speed", 0f);
			animator.SetFloat("Casting", 0f);
			animator.SetFloat("CastAngle", 0f);
			animator.SetBool("Combo1", false);
			animator.SetBool("Combo2", false);
		}
	}

	private void TakeDamage(int damage) {
		hp -= damage;

		hpController.UpdateHP(hp);

		if(hp <= 0) {
			DeathAnimation();
			Invoke ("Resurrect", 3);
		}
	}

	//Função teste para testar o recebimento teste de dano testável
	public void takeDamageTest() {
		hp -= 1;
		
		hpController.UpdateHP(hp);
		
		if(hp <= 0 && !isDead) {

			DeathAnimation();
			Invoke ("Resurrect", 3);
		}
	}

	private void DeathAnimation() {
		isDead = true;
		
		//REMINDER: Setar os triggers do animator
		Debug.Log("morri");
		isCastingRangedAttack = false;
		animator.SetFloat("Speed", 0f);
		animator.SetFloat("Casting", 0f);
		animator.SetFloat("CastAngle", 0f);
		animator.SetBool("Combo1", false);
		animator.SetBool("Combo2", false);
		GetComponent<PlayerControls>().ResetControlls();

		deathAnimation = (GameObject) Instantiate(deathAnimationBody, gameObject.transform.position, gameObject.transform.rotation);
		deathAnimation.transform.localScale = gameObject.transform.localScale;
		transform.SetParent(null);
		gameObject.SetActive(false);
	}

	private void Resurrect () {
		isDead = false;
		Destroy(deathAnimation);
		gameObject.SetActive(true);
		transform.position = new Vector3 (0, 3, 0);
		hp = 3;
		hpController.UpdateHP(hp);
		animator.SetTrigger("JustDied");
		Reset();
		castingHands.GetComponent<SpriteRenderer>().enabled = false;
		//TODO: resetar maos
	}

	private void Reset(){
		animator.SetFloat("Speed", 0f);
		animator.SetFloat("Casting", 0f);
		animator.SetFloat("CastAngle", 0f);
		animator.SetBool("Combo1", false);
		animator.SetBool("Combo2", false);
		pause = false;
		isWalking = false;
		isCastingRangedAttack = false;
		grounded = true;
		isDead = false;
		inCombo = false;
		comboCount = -1;
		comboWindow = 0.5f;

	}
}