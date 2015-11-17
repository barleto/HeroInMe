using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public float lifeSpan;
	public int damage;

	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifeSpan -= Time.deltaTime;
		if(lifeSpan <= 0){
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		//Colisao com inimigo
		if (col.gameObject.CompareTag("Enemy")) {
			Destroy (col.gameObject); //TODO: Aplicar dano
		} else if(col.gameObject.CompareTag("Destructable")){
			col.gameObject.GetComponent<DestructableController>().TakeDamage(damage);
		}

		//Destroi o projetil em qualquer colisão exceto com o jogador
		if(!col.gameObject.CompareTag("Player")){
			Destroy (gameObject);
		}
	}
}
