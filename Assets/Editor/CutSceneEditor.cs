using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CutScene))]
public class CutSceneEditor : Editor {

	CutScene cutScene;

	public override void OnInspectorGUI(){
		cutScene = (CutScene)target;
		if(cutScene.nodeList == null){
			cutScene.nodeList = new List<CutSceneNodes>();
		}
		//draw bool options
		bool pauseGame = GUILayout.Toggle (cutScene.pauseGame,"Pause The Game",GUILayout.Width(300));
		cutScene.pauseGame = pauseGame;
		//draw buttons for adding and subtracting the cutscene sequence:
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("+Dialogo",GUILayout.Width(100))){
			cutScene.nodeList.Add(new Dialogue());
		}
		if(GUILayout.Button("+Animation",GUILayout.Width(100))){
			
		}
		GUILayout.EndHorizontal ();

		//show list
		for(int i=0;i<cutScene.nodeList.Count;i++){
			CutSceneNodes nodeS = cutScene.nodeList[i];
			if(nodeS is Dialogue){
				Dialogue node = (Dialogue)nodeS;
				GUILayout.BeginVertical("box");
				if(GUILayout.Button("- Delete",GUILayout.Width(100))){
					cutScene.nodeList.RemoveAt(i);
				}
				GUILayout.Label("Dialog");
				GUILayout.BeginHorizontal();
				node.characterImage = (Sprite)EditorGUILayout.ObjectField ("Icon",node.characterImage, typeof(Sprite), true);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("text: ");	
				node.text = EditorGUILayout.TextArea(((Dialogue)node).text,GUILayout.Width(300),GUILayout.Height(60));
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}else{

			}
		}

	}
}
