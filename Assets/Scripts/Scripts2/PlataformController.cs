using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlataformController : MonoBehaviour {

	public Vector3 finalPosition;
	public float speed = 2;
	public float delay = 1;


	// Use this for initialization
	void Start () {
		float x = finalPosition.x;
		float y = finalPosition.y;


		Hashtable config =  iTween.Hash("x", x, 
		         						"y", y, 
		         						"speed", speed,
		          						"delay", delay,
		                                "easetype", iTween.EaseType.easeInOutCubic,
										"looptype", iTween.LoopType.pingPong);

		iTween.MoveTo(gameObject, config);

	}
	
	// Update is called once per frame
	void Update () {

	}
}
