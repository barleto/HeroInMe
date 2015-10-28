using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	protected IPlayerController player = null;
	private Vector2 startPos;
	private Vector2 direction;
	private bool directionChosen;
	private bool willMove;
	private bool attack = false;

	void Awake () {
		// Gets the script associated with the player controller interface
		player = GetComponent<IPlayerController>();
	}
	
	void Update() {
		// Track a single touch as a direction control.
		if (Input.touchCount > 0) {
			var touch = Input.GetTouch(0);
			
			// Handle finger movements based on touch phase.
			switch (touch.phase) {
				// Record initial touch position.
			case TouchPhase.Began:
				startPos = touch.position;
				directionChosen = false;
				break;
				
				// Determine direction by comparing the current touch position with the initial one.
			case TouchPhase.Moved:
				direction = touch.position - startPos;
				willMove = true;
				directionChosen = true;
				break;
				
				// Report that a direction has been chosen when the finger is lifted.
			case TouchPhase.Ended:
				willMove = false;
				if(Input.touchCount == 2){ // double tap
					attack = true;
				}
				break;
			}
		}
		// After user released his finger will decide which action to do
		if (directionChosen) {
			if (willMove) { // swipe
				player.movePlayer (direction);
			} else {
				player.movePlayer(new Vector2(0, 0));
			}
		} else if (attack) {
			player.attack();
			attack = false;
		}
	}
}
