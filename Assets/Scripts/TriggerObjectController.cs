using UnityEngine;
using System.Collections;

public class TriggerObjectController : MonoBehaviour {

	public GameObject target;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Função chamada quando o trigger é ativado
	public void Action () {
		target.GetComponent<MainCharacterController>().EquipItem(sr.sprite);
		Destroy();
	}

	void Destroy () {
		gameObject.SetActive(false);
	}
}
