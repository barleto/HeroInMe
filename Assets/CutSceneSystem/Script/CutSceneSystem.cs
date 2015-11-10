using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class CutSceneSystem : MonoBehaviour {
	
	enum NodeType {Animation, Dialogue};
	public Canvas canvas;
	public Image talkImage;
	public Image chatBox;
	public Text textBox;
	public GameObject mainCharacaterScript;
	public CutScene currentCutScene;
	public UnityEvent onCutScenePause;
	
	private static CutSceneSystem singletonInstanece = null;
	
	private CutSceneNodes currentNode = null;
	private int currentIndexNode = 0;
	private bool tapOccured = false;
	private bool continuePlaying = true;
	
	// Use this for initialization
	void Start () {
		toggleUIVisibility (false);
		textBox.text = "";
		playScene (currentCutScene);//TODO remove this
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public static CutSceneSystem getInstanceInGame(){
		if (singletonInstanece == null) {
			singletonInstanece = GameObject.FindWithTag ("CutSceneSystem").GetComponent<CutSceneSystem>();
		}
		return singletonInstanece;
		
	}
	
	public void playScene(CutScene newScene){
		if(newScene.nodeList.Count < 1){
			return;
		}
		currentCutScene = newScene;
		if(currentCutScene.pauseGame){
			onCutScenePause.Invoke ();
		}
		toggleUIVisibility (true);
		currentIndexNode = 0;
		continuePlaying = true;
		StartCoroutine ("jumpToNodeAndPLay");
	}
	
	public void stopScene(){
		toggleUIVisibility (false);
	}
	
	public void pauseScene(){
		
	}
	
	public IEnumerator jumpToNodeAndPLay(){
		int index = currentIndexNode;
		if (index >= currentCutScene.nodeList.Count || !continuePlaying) {
			foreach (CutSceneNodes CSN in currentCutScene.nodeList) {
				CSN.hasExecutionEnded = false;
			}
			currentCutScene = null;
			toggleUIVisibility (false);
			yield return null;
		} else {
			currentNode = currentCutScene.nodeList[index];
			
			currentNode.start ();
			while(!currentNode.hasExecutionEnded && continuePlaying){
				currentNode.update();
				yield return null;
			}
			currentNode.end ();
			
			if(continuePlaying){
				++currentIndexNode;
				StartCoroutine ("jumpToNodeAndPLay");
			}
		}

	}
	
	public void toggleUIVisibility(bool b){
		canvas.gameObject.SetActive (b);
	}
	
	public void tapAtChatBox(){
		currentNode.tapAtScreen ();
	}

	public void debugCall(){
		for(int i = 0 ; i< 10000;i++){
			Debug.Log ("Funfou");
		}
	}
}
/*
public class CutSceneSystem : MonoBehaviour {

	enum NodeType {Animation, Dialogue};

	public Image talkImage;
	public Image chatBox;
	public Text textBox;
	public GameObject mainCharacaterScript;
	public CutScene currentCutScene;
	public string SONZINHO_TODO = "SONZINHU DAiS lETRA TODO";//TODO

	private static CutSceneSystem singletonInstanece = null;

	private CutSceneNodes currentNode = null;
	private int currentIndexNode = 0;
	private List<AnimationNode> anims =  new List<AnimationNode>();
	private Dialogue dialogue;
	private bool isCompositeNode = false;
	private bool hasNodeFinished = false;
	private bool hasDialogueFinished = false;
	private bool tapOccured = false;

	// Use this for initialization
	void Start () {
		chatBox.gameObject.SetActive (!chatBox.gameObject.activeSelf);
		talkImage.gameObject.SetActive (!talkImage.gameObject.activeSelf);
		playScene (currentCutScene);//TODO remove this
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static CutSceneSystem getInstanceInGame(){
		if (singletonInstanece == null) {
			singletonInstanece = GameObject.FindWithTag ("CutSceneSystem").GetComponent<CutSceneSystem>();
		}
		return singletonInstanece;

	}

	public void playScene(CutScene newScene){
		if(newScene.nodeList.Count < 1){
			return;
		}
		currentCutScene = newScene;
		toggleUIVisibility (true);
		currentIndexNode = 0;
		jumpToNodeAndPLay (currentIndexNode);
	}

	public void stopScene(){
		toggleUIVisibility (false);
	}

	public void pauseScene(){

	}

	public void jumpToNodeAndPLay(int index){
		if(index >= currentCutScene.nodeList.Count){
			foreach(CutSceneNodes CSN in currentCutScene.nodeList){
				CSN.executionHasEnded = false;
			}
			currentCutScene = null;
			toggleUIVisibility (false);
			return;
		}
		currentNode = currentCutScene.nodeList[index];
		anims.Clear ();
		dialogue = null;
		if (currentNode is CompositeCutSceneNode) {
			isCompositeNode = true;
			CompositeCutSceneNode composite = (CompositeCutSceneNode)currentNode;
			foreach (CutSceneNodes node in composite.children) {
				if (node is AnimationNode) {
					anims.Add ((AnimationNode)node);
				} else {
					dialogue = (Dialogue)node;
				}
			}

		} else {
			isCompositeNode = false;
			if (currentNode is AnimationNode) {
				anims.Add ((AnimationNode)currentNode);
			} else {
				dialogue = (Dialogue)currentNode;
			}
		}
		if(dialogue == null){
			toggleUIVisibility(false);
		}

		foreach (AnimationNode node in anims) {
			node.animation.Play ();
			//foreach (AnimationState state in node.animation) {
				//state.normalizedTime = 0.5f;
			//}
		}
		runCurrentNode();

	}

	private void runCurrentNode(){
		textBox.text = "";
		tapOccured = false;
		if (dialogue != null) {
			talkImage.sprite = dialogue.characterImage;
			hasDialogueFinished = false;
			StartCoroutine ("showText");
		} else {
			hasDialogueFinished = true;
		}
		StartCoroutine ("checkForEndOfNode");

	}

	IEnumerator showText () {
		foreach (char letter in dialogue.text.ToCharArray()) {
			textBox.text += letter;
			yield return new WaitForSeconds (dialogue.letterPause);
		}
		hasDialogueFinished = true;
		checkForEndOfNode ();
	}

	 IEnumerator checkForEndOfNode(){
		bool allAnimationFinished = true;
		while(true){
			if(currentCutScene.pauseGame){
				foreach (AnimationNode node in anims) {
					if(node.animation.isPlaying){
						allAnimationFinished = false;
						break;
					}
				}

				if(allAnimationFinished && hasDialogueFinished){
					if(dialogue == null){
						tapOccured = false;
						break;
					}else{
						if(tapOccured){
							tapOccured = false;
							break;
						}
					}

				}
					
			}else{

			}
			yield return null;
			allAnimationFinished = true;
		}
		jumpToNodeAndPLay(++currentIndexNode);

	}

	void toggleUIVisibility(bool b){
		chatBox.gameObject.SetActive (b);
		talkImage.gameObject.SetActive (b);
	}

	public void tapAtChatBox(){
		if(hasDialogueFinished && currentCutScene.pauseGame){
			tapOccured = true;
		}
	}
}*/