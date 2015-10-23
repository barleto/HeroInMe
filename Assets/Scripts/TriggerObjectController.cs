using UnityEngine;
using System.Collections;

public class TriggerObjectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Função chamada quando o trigger é ativado
	void Action() {

	}

	void Destroy () {
		gameObject.SetActive(false);
	}
}
