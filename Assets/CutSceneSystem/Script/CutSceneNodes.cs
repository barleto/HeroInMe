using UnityEngine;
using System.Collections;
using UnityEditor;


public class CutSceneNodes : ScriptableObject{

	public bool hasExecutionEnded = false;
	public CutScene cutScene;

	virtual public void createUIDescription(CutScene cutScene){

	}

	virtual public void start(){
		cutScene.css.toggleUIVisibility (true);
	}

	virtual public void update(){
	}

	virtual public void end(){
		cutScene.css.toggleUIVisibility (false);
	}

	virtual public void tapAtScreen(){
		hasExecutionEnded = true;
	}

}


