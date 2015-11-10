using UnityEngine;
using System.Collections;

public interface IPlayerController {

	//Moves Player according to inputs
	void MovePlayer(Vector2 movement);
	
	//Make Player attack in melee range
	void AttackMelee();

	//Make Player attack in distance
	void AttackRanged(Vector2 direction);
	
	//Makes Player equip item
	void EquipItem ();

	//Pauses Player Movement
	void Pause ();
}
