using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	//REMINDER: Se alterar castingDuration, alterar a transição no animator
	public float castingDuration = 1f;
	public float longPressDuration = 0.4f;

	protected IPlayerController player = null;
	private Vector2 startPos;
	private Vector2 endPos;
	private Vector2 direction;
	private Vector2 normalizedDirection;
	private Vector2 movement = new Vector2(0, 0);
	private bool attack = false;
	private bool swipeUpDetected = false;
	private bool longPressDetected = false;
	private bool isMoving = false;
	private float touchTime;
	private float castingTime = 0;
	private float touchingBounds;

	//move o player para a direita
	public void MoveRight () {
		movement = new Vector2(1, 0);
		isMoving = true;
	}

	//move o player para a esquerda
	public void MoveLeft () {
		movement = new Vector2(-1, 0);
		isMoving = true;
	}

	//para de mover o player
	public void StopMove () {
		isMoving = false;
		movement = new Vector2(0, 0);
	}
	
	void Awake () {
		// Gets the script associated with the player controller interface
		player = GetComponent<IPlayerController>();
		touchingBounds = Screen.width / 2;
	}

	void Update() {

		// Track a single touch as a direction control.

		int count = Input.touchCount;

		//Reconhece no máximo 2 toques simultaneos
		if (count > 2) {
			count = 2;
		}
		if (longPressDetected && !isMoving){
			count = 1;
		}

		for (int i = 0; i < count; i++) {

			var touch = Input.GetTouch(i);

			// Handle finger movements based on touch phase.
			switch (touch.phase) {

				// Record initial touch position.
			case TouchPhase.Began:
				startPos = touch.position;
				if(touch.position.x > touchingBounds && !longPressDetected){
					touchTime = Time.time;
				}
				break;

				//Detecta long press, onde o tempo necessário é decidido no inspetor segundo
			case TouchPhase.Stationary:
				if(!longPressDetected && Time.time - touchTime > longPressDuration && touch.position.x > touchingBounds){
					longPressDetected = true;
					castingTime = Time.time - touchTime;
				} else if(longPressDetected && touch.position.x > touchingBounds){
					castingTime = Time.time - touchTime;
				}
				break;

				// Determine direction by comparing the current touch position with the initial one.
			case TouchPhase.Moved:
				//Mathf.Abs(startPos.magnitude - touch.position.magnitude) > 15 função para margem de erro, se necessário
				//TODO: aumentar tela de alcance do player
				if(touch.position.x > touchingBounds) {
					direction = touch.position - startPos;

					//Impede que o personagem pule enquando carrega a magia
					if(!longPressDetected){
						swipeUpDetected = true;
					}
				}
				break;
				
				// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				if (touch.position.x > touchingBounds) {
					if (!swipeUpDetected || longPressDetected) {
						attack = true;
					} else {
						swipeUpDetected = false;
					}
				}
				castingTime = 0f;
				break;
			}
		}
	}

	void FixedUpdate() {

		int count = Input.touchCount;

		//Reconhece de zero a 2 toques na tela

		if(count == 0){
			count = 1;
		} else if (count > 2) {
			count = 2;
		}

		for (int i = 0; i < count; i++) {

			//Player atacando, seja melee ou ranged
			if (attack) { 
				//ranged attack
				if (longPressDetected) {
					//Impede que a magia fique parada no lugar
					if (direction.magnitude == 0) {
						direction = new Vector2(1, 0);
					}
					if(Time.time - touchTime > castingDuration){
						player.AttackRanged(direction);
					}
					direction = new Vector2 (0, 0);
					longPressDetected = false;
					touchTime = Time.time;//Impede que o player ataque duas vezes
				//melee attack
				} else {
					player.AttackMelee ();
				}
				attack = false;

			//Reconhece um long press
			} else if (longPressDetected){
				//Manda a direção que o player está mirando
				player.CastRangedAttack(direction, castingTime);

			//Pulo
			}else if (direction.y > 80 && normalizedDirection.x > -0.5f && normalizedDirection.x < 0.5f && !longPressDetected) {
				player.MovePlayer (new Vector2 (0, 1));
				direction = new Vector2(0, 0);

			//Movimentação
			} else {
				//Se não existir toque na tela pare o player
				if(count == 0){
					movement = new Vector2(0, 0);
				}
				player.MovePlayer(movement);
			}
		}
	}
}
