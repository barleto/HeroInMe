using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class CutSceneNodes : ScriptableObject{

	public bool hasExecutionEnded = false;
	public CutScene cutScene;

	/*In this function, you define what will appear in the UI of the CutScene.
	Just populate with GUILayout funcitons*/
	virtual public void createUIDescription(CutScene cutScene){

	}

	//executed once to initialize the node
	virtual public void start(){
		cutScene.css.toggleUIVisibility (true);
	}

	//executed each frame
	virtual public void update(){
	}

	//executed when the node finished executing. Finalize thng if you want to.
	virtual public void end(){
		cutScene.css.toggleUIVisibility (false);
	}

	//executed each time the chatbox of the cutscene system is taped/clicked
	virtual public void tapAtScreen(){
		hasExecutionEnded = true;
	}

}


