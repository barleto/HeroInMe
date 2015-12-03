using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {

	public GameObject door;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Action(){
		door.GetComponent<DoorController>().OpenDoor();
		gameObject.SetActive(false);
	}
}
