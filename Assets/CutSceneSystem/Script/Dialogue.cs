using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : CutSceneNodes {
	public Sprite characterImage;
	public string text;
	public GameObject target = null;
	public float timeToLive = 1.0f;


	public Dialogue(){
		
	}
}
