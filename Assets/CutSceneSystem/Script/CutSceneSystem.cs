using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CutSceneSystem : MonoBehaviour {

	enum NodeType {Animation, Dialogue};

	public GameObject talkImage;
	public Image chatBox;
	public Text textBox;
	public GameObject mainCharacaterScript;
	public CutScene currentCutScene;
	public string SONZINHO_TODO = "SONZINHU DAiS lETRA";
	private List<OnGoingNode> onGoingNodes;
	private CutSceneRunTime runTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playScene(CutScene newScene){

	}

	public void stopScene(){

	}

	public void pauseScene(){

	}

	public void jumpToNextNode(){

	}

	private class OnGoingNode{
		public CutSceneNodes cutScene;
		public NodeType type = NodeType.Dialogue;
	}

	private class CutSceneRunTime{

	}
}