using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D col2D){
		Debug.Log("lala");
		if(col2D.gameObject.CompareTag("Destructable")){
			col2D.gameObject.GetComponent<DestructableController>().TakeDamage();
		}
	}
}
