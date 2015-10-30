using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CutScene : MonoBehaviour {
	
	public List<CutSceneNodes> nodeList = new List<CutSceneNodes>();
	public bool pauseGame = true;
	public CutSceneSystem css;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
