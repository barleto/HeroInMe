using UnityEngine;
using System.Collections;

public class LeapOfFaithEvent : MonoBehaviour {

	public GameObject csSystemObject;

	private bool activeted = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (activeted) {
			return;
		}
		Debug.Log("Hey");
		csSystemObject.GetComponent<CutSceneSystem>().playScene(gameObject.GetComponent<CutScene>());
		activeted = true;
		//gameObject.SetActive(false);
	}
}
