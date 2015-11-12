using UnityEngine;
using System.Collections;

public class DeathEffector : MonoBehaviour {

	public float explosionTime = 0.2f;

	// Use this for initialization
	void Start () {
		Invoke("Deactivate", explosionTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Deactivate () {
		gameObject.SetActive(false);
	}
}
