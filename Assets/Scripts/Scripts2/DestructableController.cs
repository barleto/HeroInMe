using UnityEngine;
using System.Collections;

public class DestructableController : MonoBehaviour {

	private Material material;
	private bool isActive = true;
	private int hp = 4;

	// Use this for initialization
	void Start () {
		material = GetComponent<Material>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col2D){
		if(col2D.gameObject.CompareTag("Weapon")){
			if (hp <= 0){
				gameObject.SetActive(false);
			} else {
				hp -= 1;
			}
		}
	}

}
