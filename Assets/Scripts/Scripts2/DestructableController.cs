using UnityEngine;
using System.Collections;

public class DestructableController : MonoBehaviour {

	public int hp;
	
	public void TakeDamage(){
		hp--;
		Debug.Log("take damage");
		if(hp <= 0){
			Destroy(this.gameObject);
		}
	}
}
