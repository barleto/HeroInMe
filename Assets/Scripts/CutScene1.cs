using UnityEngine;
using System.Collections;

public class CutScene1 : MonoBehaviour {
	
	public GameObject csSystemObject;
	public GameObject highlightImage;
	public GameObject player;
	
	private bool activated = false;
	private bool highlightActive = false;
	private Player playerScript;
	
	// Use this for initialization
	void Start () {
		playerScript = player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (activated) {
			return;
		}
		playerScript.Pause();
		csSystemObject.GetComponent<CutSceneSystem>().playScene(gameObject.GetComponentInParent<CutScene>());
		activated = true;
	}

	public void ActivateHighlight() {
		if(!highlightActive) {
			highlightImage.SetActive(true);
			highlightActive = true;
		}
	}

	public void DeactivateHighlight() {
		if(highlightActive){
			playerScript.Pause();
			highlightImage.SetActive(false);
			highlightActive = false;
		}
	}
}