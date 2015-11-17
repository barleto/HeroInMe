using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour {

	public int damage;

	void OnTriggerEnter2D(Collider2D col2D){
		if(col2D.gameObject.CompareTag("Destructable")){
			col2D.gameObject.GetComponent<DestructableController>().TakeDamage(damage);
		}
	}
}
