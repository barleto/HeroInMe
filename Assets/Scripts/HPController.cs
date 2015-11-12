using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPController : MonoBehaviour {
	
	public GameObject[] hearts;
	public Sprite blackHeartSprite;
	public Sprite heartSprite;

	private Image[] heartsImage;

	// Use this for initialization
	void Start () {

		heartsImage = new Image[hearts.Length];

		for (int i = 0; i < hearts.Length; i++) {
			heartsImage[i] = hearts[i].GetComponent<Image>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateHP (int currentHP) {

		for (int i = 0; i < hearts.Length; i++) {
			if (i + 1 > currentHP) {
				heartsImage[i].sprite = blackHeartSprite;
			} else {
				heartsImage[i].sprite = heartSprite;
			}
		}
	}
}
