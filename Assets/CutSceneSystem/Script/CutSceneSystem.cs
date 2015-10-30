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
		chatBox.gameObject.SetActive (!chatBox.gameObject.activeSelf);
		talkImage.gameObject.SetActive (!talkImage.gameObject.activeSelf);
		playScene (currentCutScene);//TODO remove this
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playScene(CutScene newScene){
		if(newScene.nodeList.Count < 1){
			return;
		}
		currentCutScene = newScene;
		toggleUIVisibility ();
		jumpToNodeAndPLay (currentIndexNode);
	}

	public void stopScene(){
		toggleUIVisibility ();
	}

	public void pauseScene(){

	}

	public void jumpToNodeAndPLay(int index){
		if(index >= currentCutScene.nodeList.Count){
			currentCutScene = null;
			toggleUIVisibility ();
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

	void toggleUIVisibility(){
		chatBox.gameObject.SetActive (!chatBox.gameObject.activeSelf);
		talkImage.gameObject.SetActive (!talkImage.gameObject.activeSelf);
	}
}