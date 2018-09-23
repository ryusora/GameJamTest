using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
	public SpriteRenderer cubeSR;
	public GameObject perfectZone;
	// Use this for initialization
	public void HidePerfectZone() {
		perfectZone.SetActive(false);
	}
	public void ShowPerfectZone() {
		perfectZone.SetActive(true);
	}
	public void SetColor(Color color) {
		cubeSR.color = color;
	}
}
