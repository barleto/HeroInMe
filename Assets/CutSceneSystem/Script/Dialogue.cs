using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class Dialogue : CutSceneNodes {
	public Sprite characterImage;
	public string text;
	[Range(0.1f,5f)]
	public float letterPause = 0.1f;

	[SerializeField]
	private Text textBox;
	[SerializeField]
	private bool hasFinishedWritingText = false;
	[SerializeField]
	private Coroutine textRoutine;

	#if UNITY_EDITOR
	public override void createUIDescription(CutScene cutScene,SerializedObject serializedObject){
		Dialogue node = this;
		GUILayout.Label("<<Dialogue>>");
		float time = EditorGUILayout.Slider("Show Speed",node.letterPause,0,1f);
		if(time >= 0f && time <= 10){
			node.letterPause = time;
		}
		GUILayout.BeginHorizontal();
		node.characterImage = (Sprite)EditorGUILayout.ObjectField ("Icon: ",node.characterImage, typeof(Sprite), true);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Text: ");	
		node.text = EditorGUILayout.TextArea(((Dialogue)node).text,GUILayout.Width(300),GUILayout.Height(60));
		GUILayout.EndHorizontal();
	}
#endif

	public override void start(){
		base.start ();
		cutScene.css.talkImage.sprite = characterImage;
		textBox = cutScene.css.textBox;
		textBox.text = "";
		hasFinishedWritingText = false;
		cutScene.css.toggleUIVisibility (true);
		textRoutine = cutScene.StartCoroutine (showText ());
	}
	
	public override  void update(){

	}
	
	public override  void end(){
		base.end ();
		cutScene.StopCoroutine (textRoutine);
		cutScene.css.toggleUIVisibility (false);
		hasFinishedWritingText = false;
	}

	public override void tapAtScreen ()
	{
		if(hasFinishedWritingText){
			hasExecutionEnded = true;
		}
	}

	public IEnumerator showText () {
		foreach (char letter in text.ToCharArray()) {
			textBox.text += letter;
			yield return new WaitForSeconds (letterPause);
		}
		hasFinishedWritingText = true;
	}
}
