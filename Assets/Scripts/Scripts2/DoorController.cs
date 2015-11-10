using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OpenDoor(){
		Hashtable config =  iTween.Hash("y", 10,
		                                "speed", 6,
		                                "easetype", iTween.EaseType.easeInOutCubic);

		iTween.MoveTo(gameObject, config);
	}
}
