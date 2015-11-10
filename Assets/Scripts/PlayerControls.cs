using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	protected IPlayerController player = null;
	private Vector2 startPos;
	private Vector2 endPos;
	private Vector2 direction;
	private Vector2 normalizedDirection;
	private Vector2 movement = new Vector2(0, 0);
	private bool attack = false;
	private bool swipeUpDetected = false;
	private bool longPressDetected = false;
	private bool alreadyMoved = false;
	private float touchTime;

	//move o player para a direita
	public void MoveRight () {
		movement = new Vector2(1, 0);
	}

	//move o player para a esquerda
	public void MoveLeft () {
		movement = new Vector2(-1, 0);
	}

	//para de mover o player
	public void StopMove () {

		movement = new Vector2(0, 0);
	}
	
	void Awake () {
		// Gets the script associated with the player controller interface
		player = GetComponent<IPlayerController>();
	}

	void Update() {

		// Track a single touch as a direction control.

		int count = Input.touchCount;

		//Reconhece no máximo 2 toques simultaneos
		if (count > 2) {
			count = 2;
		}

		for (int i = 0; i < Input.touchCount; i++) {

			var touch = Input.GetTouch(i);

			// Handle finger movements based on touch phase.
			switch (touch.phase) {

				// Record initial touch position.
			case TouchPhase.Began:
				startPos = touch.position;

				alreadyMoved = false;

				touchTime = Time.time;

				break;

				//Detecta long press, onde o tempo necessário é 0.7 segundo
			case TouchPhase.Stationary:
				if(!longPressDetected && Time.time - touchTime > 0.7 && touch.position.x > Screen.width / 2){
					longPressDetected = true;

				}
				break;

				// Determine direction by comparing the current touch position with the initial one.
			case TouchPhase.Moved:
				//Mathf.Abs(startPos.magnitude - touch.position.magnitude) > 15 função para margem de erro, se necessário
				if(touch.position.x > Screen.width / 2) {
					direction = touch.position - startPos;
					normalizedDirection = direction.normalized;

					//Impede que o personagem pule enquando carrega a magia
					if(!longPressDetected){
						swipeUpDetected = true;
					}
				}
				break;
				
				// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				if (touch.position.x > Screen.width / 2) {
					if (!swipeUpDetected || longPressDetected) {
						attack = true;

					} else {
						swipeUpDetected = false;
					}
				}
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
					touchTime = Time.time;//Impede que o player ataque duas vezes
					//Impede que a magia fique parada no lugar
					if (direction.magnitude == 0) {
						direction = new Vector2(1, 0);
					}

					player.AttackRanged(direction);

					direction = new Vector2 (0, 0);
					longPressDetected = false;
				//melee attack
				} else {
					player.AttackMelee ();
				}
				attack = false;

			//Reconhece um long press
			} else if (longPressDetected){
				//Manda a direção que o player está mirando
				player.CastRangedAttack(direction);

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
