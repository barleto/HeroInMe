using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class AnimationNode : CutSceneNodes {
	public Animation animation = null;

	public override void createUIDescription(CutScene cutScene){
		AnimationNode node = this;
		GUILayout.Label("<<Animation>>");
		node.animation = (Animation)EditorGUILayout.ObjectField ("Animation: ",node.animation, typeof(Animation), true);
	}

	public override void start(){
		animation.Play ();
	}
	
	public override  void update(){
		if(!(animation.isPlaying)){
			hasExecutionEnded = true;
		}
	}
	
	public override  void end(){

	}
}
