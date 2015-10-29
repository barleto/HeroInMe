using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompositeCutSceneNode : CutSceneNodes {
	public List<CutSceneNodes> children = new List<CutSceneNodes>();
}
