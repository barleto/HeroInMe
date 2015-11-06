using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class ExecuteFunctionNode : CutSceneNodes {
	public UnityEvent eventCallees ;

	public override void createUIDescription(CutScene cutScene,SerializedObject serializedObject){
		ExecuteFunctionNode node = this;
		GUILayout.Label("<<Execute Function>>");
		SerializedProperty prop = (new SerializedObject(this)).FindProperty ("eventCallees");
		EditorGUIUtility.LookLikeControls();
		EditorGUILayout.PropertyField (prop);
	}

	public override void start(){

	}
	
	public override  void update(){

	}
	
	public override  void end(){

	}
}
