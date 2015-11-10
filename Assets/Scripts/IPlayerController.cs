using UnityEngine;
using System.Collections;

public interface IPlayerController {

	//Moves Player according to inputs
	void MovePlayer(Vector2 movement);
	
	//Make Player attack in melee range
	void AttackMelee(int state);

	//Make Player attack in distance
	void AttackRanged(Vector2 direction);

	//Make Player aim ranged attack
	void CastRangedAttack (Vector2 direction);

	//Makes Player equip item
	void EquipItem ();

}
