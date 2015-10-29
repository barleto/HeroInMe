using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CutScene))]
public class CutSceneEditor : Editor {

	CutScene cutScene;
	private int index1,index2;
	private bool grouping = false;

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
		grouping = false;
		foreach(CutSceneNodes nodeS in cutScene.nodeList){
			GUILayout.BeginVertical("box");

			if(nodeS is Dialogue){
				Dialogue node = (Dialogue)nodeS;
				if(GUILayout.Button("- Delete",GUILayout.Width(100))){
					cutScene.nodeList.Remove(node);
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
				if(cutScene.nodeList.IndexOf(nodeS)+1 < cutScene.nodeList.Count){
					if(GUILayout.Button("Group With Next Node")){
						grouping = true;
						index1 = cutScene.nodeList.IndexOf(nodeS);
						index2 = index1+1;
					}
				}
			}else if(nodeS is AnimationNode){
				AnimationNode node = (AnimationNode)nodeS;

				if(GUILayout.Button("- Delete",GUILayout.Width(100))){
					cutScene.nodeList.Remove(node);
				}
				GUILayout.Label("<<Animation>>");
				node.animation = (Animation)EditorGUILayout.ObjectField ("Animation: ",node.animation, typeof(Animation), true);
				if(cutScene.nodeList.IndexOf(node)+1 < cutScene.nodeList.Count){
					if(GUILayout.Button("Group With Next Node")){
						grouping = true;
						index1 = cutScene.nodeList.IndexOf(node);
						index2 = index1+1;
					}
				}
			}else{
				CompositeCutSceneNode nodeC = (CompositeCutSceneNode)nodeS;
				if(nodeC.children.Count>0){
					for(int j=0;j<nodeC.children.Count;j++){
						CutSceneNodes nodeSC = nodeC.children[j];
						if(nodeSC is Dialogue){
							Dialogue nodeI = (Dialogue)nodeSC;
							if(GUILayout.Button("- Delete",GUILayout.Width(100))){
								nodeC.children.RemoveAt(j);
							}
							GUILayout.Label("<<Dialog>>");
							if(!cutScene.pauseGame){
								nodeI.target = (GameObject)EditorGUILayout.ObjectField ("Target: ",nodeI.target, typeof(GameObject), true);
								nodeI.timeToLive = EditorGUILayout.FloatField("Time To Live",nodeI.timeToLive);
							}
							GUILayout.BeginHorizontal();
							nodeI.characterImage = (Sprite)EditorGUILayout.ObjectField ("Icon: ",nodeI.characterImage, typeof(Sprite), true);
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
							GUILayout.Label("Text: ");	
							nodeI.text = EditorGUILayout.TextArea(((Dialogue)nodeI).text,GUILayout.Width(300),GUILayout.Height(60));
							GUILayout.EndHorizontal();
						}else if(nodeSC is AnimationNode){
							AnimationNode nodeSA = (AnimationNode)nodeSC;
							
							if(GUILayout.Button("- Delete",GUILayout.Width(100))){
								nodeC.children.RemoveAt(j);
							}
							GUILayout.Label("<<Animation>>");
							nodeSA.animation = (Animation)EditorGUILayout.ObjectField ("Animation: ",nodeSA.animation, typeof(Animation), true);

						}
						if(j+1 < nodeC.children.Count){
							if(GUILayout.Button("-- Break Group Here --")){
								CompositeCutSceneNode newComp = new CompositeCutSceneNode();
								newComp.children = nodeC.children.GetRange(j+1,nodeC.children.Count - 1 - j);
								nodeC.children.RemoveRange(j+1,nodeC.children.Count - 1 - j);
								int index = cutScene.nodeList.IndexOf(nodeS);
								cutScene.nodeList.Insert(index+1,newComp);
							}
						}
					}
				}

				if(cutScene.nodeList.IndexOf(nodeS)+1 < cutScene.nodeList.Count){
					if(GUILayout.Button("Group With Next Node")){
						grouping = true;
						index1 = cutScene.nodeList.IndexOf(nodeS);
						index2 = index1+1;
					}
				}

			}
			//nodeS.playWithNext = EditorGUILayout.Toggle("Play With Next: ",nodeS.playWithNext);
			GUILayout.EndVertical();
		}

		if(grouping){
			CutSceneNodes g1  = cutScene.nodeList[index1];
			CutSceneNodes g2 = cutScene.nodeList[index2];
			if(g1 is CompositeCutSceneNode){
				if(g2 is CompositeCutSceneNode){
					((CompositeCutSceneNode)g1).children.AddRange(((CompositeCutSceneNode)g2).children);
					cutScene.nodeList.Remove(g2);
				}else{
					((CompositeCutSceneNode)g1).children.Add(g2);
					cutScene.nodeList.Remove(g2);
				}
			}else{
				if(g2 is CompositeCutSceneNode){
					((CompositeCutSceneNode)g2).children.Insert(0,g1);
					cutScene.nodeList.Remove(g1);
				}else{
					CompositeCutSceneNode composite = new CompositeCutSceneNode();
					composite.children.Add(g1);
					composite.children.Add(g2);
					cutScene.nodeList.Insert(index2+1,composite);
					cutScene.nodeList.Remove(g1);
					cutScene.nodeList.Remove(g2);
				}
			}
			grouping = false;
		}

		foreach(CutSceneNodes nodeS in cutScene.nodeList){
			if(nodeS is CompositeCutSceneNode){
				if(((CompositeCutSceneNode)nodeS).children.Count < 1){
					cutScene.nodeList.Remove(nodeS);
				}else if(((CompositeCutSceneNode)nodeS).children.Count == 1){
					int index = cutScene.nodeList.IndexOf(nodeS);
					cutScene.nodeList.Insert(index+1,((CompositeCutSceneNode)nodeS).children[0]);
					cutScene.nodeList.Remove(nodeS);
				}
			}
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
