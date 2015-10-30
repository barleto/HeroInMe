using UnityEngine;
using System.Collections;

public interface IPlayerController {

	//moves Player according to inputs
	void MovePlayer(Vector2 movement);
	
	//make player attack
	void Attack();

	void EquipItem (Sprite sprite);
}
