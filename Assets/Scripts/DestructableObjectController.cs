using UnityEngine;
using System.Collections;

public class DestructableObjectController : MonoBehaviour {

	//quantidade de dano antes de morrer
	public int hp = 2;

	//receber dano
	//param:
	//	dmg: quantidade de dano recebido
	public void TakeDamage(){
		hp -= 1;

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (hp <= 0) {
			Destroy();
		}
	}

	//DIE!!!!
	void Destroy() {
		gameObject.SetActive(false);
	}

	//Trigger triggered
	void OnTriggerEnter2D(Collider2D col2D) {
		if (col2D.gameObject.CompareTag("CharacterWepaon")) { //Tag a definir
			TakeDamage();
		}
	}
}
