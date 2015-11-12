using UnityEngine;
using System.Collections;

public class TriggerObjectController : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Função chamada quando o trigger é ativado
	public void Action () {
		target.GetComponent<IPlayerController>().EquipItem();
		Destroy (this.gameObject);
	}

//	void Destroy () {
//		gameObject.SetActive(false);
//	}
}
