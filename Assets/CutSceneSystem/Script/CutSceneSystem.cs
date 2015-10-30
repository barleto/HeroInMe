using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CutSceneSystem : MonoBehaviour {

	enum NodeType {Animation, Dialogue};

	public Image talkImage;
	public Image chatBox;
	public Text textBox;
	public GameObject mainCharacaterScript;
	public CutScene currentCutScene;
	public string SONZINHO_TODO = "SONZINHU DAiS lETRA TODO";//TODO

	private CutSceneNodes currentNode = null;
	private int currentIndexNode = 0;
	private List<AnimationNode> anims =  new List<AnimationNode>();
	private Dialogue dialogue;
	private bool isCompositeNode = false;
	private bool hasNodeFinished = false;
	private bool hasDialogueFinished = false;

	// Use this for initialization
	void Start () {
		playScene (currentCutScene);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playScene(CutScene newScene){
		if(newScene.nodeList.Count < 1){
			return;
		}
		currentCutScene = newScene;
		jumpToNodeAndPLay (currentIndexNode);
	}

	public void stopScene(){
		//HIDE DIALOGUE UI HERE TODO
	}

	public void pauseScene(){

	}

	public void jumpToNodeAndPLay(int index){
		if(index >= currentCutScene.nodeList.Count){
			currentCutScene = null;
			return;
		}
		currentNode = currentCutScene.nodeList[index];
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
		//SHOW DIALOGUE UI TODO
		runCurrentNode();

	}

	private void runCurrentNode(){
		textBox.text = "";
		talkImage.sprite = dialogue.characterImage;
		hasDialogueFinished = false;
		StartCoroutine ("showText");
		//play anims HERE TODO

	}

	IEnumerator showText () {
		foreach (char letter in dialogue.text.ToCharArray()) {
			textBox.text += letter;
			yield return new WaitForSeconds (dialogue.letterPause);
		}
		hasDialogueFinished = true;
		checkForEndOfNode ();
	}

	 void checkForEndOfNode(){
		if(hasDialogueFinished){
				jumpToNodeAndPLay(++currentIndexNode);
		}

	}
}