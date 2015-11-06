﻿using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	protected IPlayerController player = null;
	private Vector2 startPos;
	private Vector2 endPos;
	private Vector2 direction;
	private Vector2 normalizedDirection;
	private Vector2 movement = new Vector2(0, 0);
	private bool attack = false;
<<<<<<< HEAD
	private bool alreadyMoved = false;
=======
	private bool longPressDetected = false;
	private bool jumpDetected = false;
	private bool castingRangedAttack = false;
	private bool alreadyStopped = false;
	private int comboCount = -1;
	private float comboWindow = 1.0f;
	private float comboTimer;
	private float touchTime;

	public void MoveRight () {
		movement = new Vector2(1, 0);
		alreadyStopped = false;
		//TODO: Falar para o movment controller que o botão mova para a direita está pressionado
	}

	public void MoveLeft () {
		movement = new Vector2(-1, 0);
		alreadyStopped = false;
		//TODO: Falar para o movment controller que o botão mova para a esquerda está pressionado
	}

	public void StopMove () {
		movement = new Vector2(0, 0);
		//TODO: Falar para o movment controller que nenhum dos botões de movimento está pressionado
	}
>>>>>>> dekkoh

	void Awake () {
		// Gets the script associated with the player controller interface
		player = GetComponent<IPlayerController>();
	}

	void Update() {

		if (comboCount >= 0) {
			comboTimer -= Time.deltaTime;
			if (comboTimer <= 0) {
				comboCount = -1;
				comboTimer = comboWindow;
			}
		}

		// Track a single touch as a direction control.
		if (Input.touchCount > 0) {
			var touch = Input.GetTouch(0);

			// Handle finger movements based on touch phase.
			switch (touch.phase) {
				// Record initial touch position.
			case TouchPhase.Began:
				startPos = touch.position;
<<<<<<< HEAD
				directionChosen = false;
				alreadyMoved = false;
=======
				touchTime = Time.time;
>>>>>>> dekkoh
				break;

			case TouchPhase.Stationary:
				if(Time.time - touchTime > 1 && !castingRangedAttack && touch.position.x > Screen.width / 2){
					longPressDetected = true;
					jumpDetected = false;

				}
				break;

				// Determine direction by comparing the current touch position with the initial one.
			case TouchPhase.Moved:
				//Mathf.Abs(startPos.magnitude - touch.position.magnitude) > 15
				if(touch.position.x > Screen.width / 2) {
					direction = touch.position - startPos;
					normalizedDirection = direction.normalized;

					if(!longPressDetected && !castingRangedAttack){
						jumpDetected = true;
					}
				}
				break;
				
				// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				if(touch.position.x > Screen.width / 2){
					if (!jumpDetected) {
						attack = true;
						comboCount++;

						if (comboCount > 2){
							comboCount = 0;
						}

					} else {
						jumpDetected = false;
					}
				} else if (castingRangedAttack){
					attack = true;
				} 

				break;
			}
		}
<<<<<<< HEAD
=======
		// After user released his finger will decide which action to do
<<<<<<< HEAD
		if (directionChosen) {
			if (willMove) { // swipe
				player.MovePlayer (direction);
			} else if(alreadyMoved == false){
				player.MovePlayer (new Vector2 (0, 0));
				player.AttackRanged(direction);
				alreadyMoved = true;
			}
		} else if (attack) {
			player.AttackMelee (0);
=======

>>>>>>> ab074f02d0d523be374d85aa19e76fad179c383b
		if (attack) { 
			if (castingRangedAttack) {
				castingRangedAttack = false;
				comboCount = -1;
				Debug.Log ("Release");
				player.AttackRanged(direction);

			} else {
				print (comboCount);
				player.AttackMelee (comboCount);
			}

			comboTimer = comboWindow;
>>>>>>> dekkoh
			attack = false;

		} else if (longPressDetected) {
			castingRangedAttack = true;
			longPressDetected = false;
			Debug.Log ("Hold");
			//player.StartRangedAnimation();

		} else if (direction.y > 80 && normalizedDirection.x > -0.5f && normalizedDirection.x < 0.5f && !castingRangedAttack) {
			player.MovePlayer (new Vector2 (0, 1));
			direction = new Vector2 (0, 0);
			
		} else if (!alreadyStopped) {
			player.MovePlayer (movement);
			if(movement.magnitude == 0){
				alreadyStopped = true;
			}
		}
	}
}
