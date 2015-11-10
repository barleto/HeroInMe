using UnityEngine;
using System.Collections;

public class DestructableController : MonoBehaviour {

	private int hp = 3;
	
	public void TakeDamage(){
		hp--;
		Debug.Log("take damage");
		if(hp <= 0){
			Destroy(this.gameObject);
		}
	}
}
