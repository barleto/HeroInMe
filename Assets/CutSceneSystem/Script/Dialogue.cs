using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : CutSceneNodes {
	public Sprite characterImage;
	public string text;
	public GameObject target = null;
	public float timeToLive = 1.0f;
	[Range(0.1f,5f)]
	public float letterPause = 0.1f;
}
