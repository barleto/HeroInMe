using UnityEngine;
using System.Collections;

public class MainCharacterController : MonoBehaviour {
	
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
	public GameObject Weapon;
	
	
	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	private SpriteRenderer weaponSpriteRenderer;
	private BoxCollider2D weaponBoxCollider2D;
	
	
	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		weaponSpriteRenderer = Weapon.GetComponent<SpriteRenderer>();
		weaponBoxCollider2D = Weapon.GetComponent<BoxCollider2D>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		
		if (Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
		}

		if (Input.GetKeyDown(KeyCode.Z)) {
			anim.SetTrigger("Attack");
		}
	}
	
	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");

		if(h == 0){
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		}
		
		anim.SetFloat("Speed", Mathf.Abs(h));
		
		if (h * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * h * moveForce);
		
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		
		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
		
		if (jump)
		{
			anim.SetTrigger("Jump");
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
		}
	}

	//Função para o personagem andar
	//Param:
	//	h: Input horizontal 
	public void Walk(float h) {
		if(h == 0){
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);
		}
		
		anim.SetFloat("Speed", Mathf.Abs(h));
		
		if (h * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * h * moveForce);
		
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
		
		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
	}

	//Inverte o personagem
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D col2D){
		if(col2D.gameObject.CompareTag("Coin")){
			col2D.gameObject.GetComponent<CoinController>().Action();
		} else if (col2D.gameObject.CompareTag("DeathTrigger")) {
			transform.position = new Vector3 (0, 2, 0);
		} else if (col2D.gameObject.CompareTag("PickUp")) {
			col2D.gameObject.GetComponent<TriggerObjectController>().Action();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if(other.transform.CompareTag("Platform")) {
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D (Collision2D other) {
		if (other.transform.CompareTag ("Platform")) {
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