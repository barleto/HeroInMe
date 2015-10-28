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
		//CutSceneSystem
		cutScene.css = GameObject.Find("CutSceneSystem").GetComponent<CutSceneSystem>();
		cutScene.css = (CutSceneSystem)EditorGUILayout.ObjectField ("Cut Scene System",cutScene.css, typeof(CutSceneSystem), true);
		//draw bool options
		bool pauseGame = GUILayout.Toggle (cutScene.pauseGame,"Pause The Game",GUILayout.Width(300));
		cutScene.pauseGame = pauseGame;
		//draw buttons for adding and subtracting the cutscene sequence:
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("+Dialogo",GUILayout.Width(100))){
			cutScene.nodeList.Add(new Dialogue());
		}
		if(GUILayout.Button("+Animation",GUILayout.Width(100))){
			cutScene.nodeList.Add(new AnimationNode());
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
				GUILayout.Label("<<Dialog>>");
				if(!cutScene.pauseGame){
					node.target = (GameObject)EditorGUILayout.ObjectField ("Target: ",node.target, typeof(GameObject), true);
					node.timeToLive = EditorGUILayout.FloatField("Time To Live",node.timeToLive);
				}
				GUILayout.BeginHorizontal();
				node.characterImage = (Sprite)EditorGUILayout.ObjectField ("Icon: ",node.characterImage, typeof(Sprite), true);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("Text: ");	
				node.text = EditorGUILayout.TextArea(((Dialogue)node).text,GUILayout.Width(300),GUILayout.Height(60));
				GUILayout.EndHorizontal();
			}else{
				AnimationNode node = (AnimationNode)nodeS;
				GUILayout.BeginVertical("box");
				if(GUILayout.Button("- Delete",GUILayout.Width(100))){
					cutScene.nodeList.RemoveAt(i);
				}
				GUILayout.Label("<<Animation>>");
				node.animation = (Animation)EditorGUILayout.ObjectField ("Animation: ",node.animation, typeof(Animation), true);
				node.waitToFinish = EditorGUILayout.Toggle("Wait To Finish: ",node.waitToFinish);
			}
			nodeS.playWithNext = EditorGUILayout.Toggle("Play With Next: ",nodeS.playWithNext);
			GUILayout.EndVertical();
		}

	}

	[MenuItem("Cut Scene System/Add CutScene Component to gameObject")]
	static void addCutsceneComponentToGameObject(){
		GameObject obj = (GameObject)Selection.activeGameObject;
		if(obj!=null){
			Undo.AddComponent(obj,typeof(CutScene));
		}
	}
	
	// Validate the menu item defined by the function above.
	// The menu item will be disabled if this function returns false.
	[MenuItem ("Cut Scene System/Add CutScene Component to gameObject", true)]
	static bool ValidateaddCutsceneComponentToGameObject () {
		// Return false if no transform is selected.
		return (Selection.activeGameObject != null);
	}

	[MenuItem("Cut Scene System/Add Cut Scene System To Scene")]
	static void CreateCutSceneSytem (MenuCommand command) {
		if(GameObject.Find("CutSceneSystem") == null){
			GameObject sys= Instantiate(Resources.Load("CutSceneSystem", typeof(GameObject))) as GameObject;
			sys.name = "CutSceneSystem";
			Undo.RegisterCreatedObjectUndo(sys, "Create " + sys.name);
		}
	}

	[MenuItem("Cut Scene System/Add Cut Scene System To Scene",true)]
	static bool ValidateCreateCutSceneSytem (MenuCommand command) {
		if(GameObject.Find("CutSceneSystem") == null){
			return true;
		}
		return false;
	}
}
