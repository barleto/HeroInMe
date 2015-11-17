using UnityEngine;
using System.Collections;

public class TriggerObjectController : MonoBehaviour {

	public GameObject target;
	private bool triggered = false;

	//Função chamada quando o trigger é ativado
	public void Action () {
		if(triggered == false){
//			Debug.Log("to pegando");
//			triggered = true;
//			target.GetComponent<IPlayerController>().EquipItem();
//			Destroy (this.gameObject);
		}
	}
}
