using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CutScene : MonoBehaviour {

	[HideInInspector]
	public GameSwitch gameSwitch;
	[SerializeField]
	public List<CutSceneNodes> nodeList = new List<CutSceneNodes>();
	[SerializeField]
	public bool pauseGame = true;
	[SerializeField]
	public CutSceneSystem css;
	[HideInInspector]
	[SerializeField]
	public int indexOfSwitch = 0;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
