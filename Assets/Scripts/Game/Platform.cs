using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
	public SpriteRenderer cubeSR;
	public GameObject perfectZone;
	public MeshRenderer meshRenderer;

	private Coroutine coroutine;
	// Use this for initialization
	public void HidePerfectZone() {
		perfectZone.SetActive(false);
	}
	public void ShowPerfectZone() {
		perfectZone.SetActive(true);
	}
	public void SetColor(Color color) {
		cubeSR.color = color;
		meshRenderer.material.SetColor("_mainColor", color);
	}

	public void StopColoringCoroutine() {
		if(coroutine != null)
			StopCoroutine(coroutine);
	}

	public void StartColoring(Color secondColor, float timer) {
		StopColoringCoroutine();
		meshRenderer.material.SetColor("_secondColor", secondColor);
		coroutine = StartCoroutine(Coloring(timer));
	}

	public void ResetHighLight() {
		meshRenderer.material.SetFloat("_HighlightBegin", 0);
	}

	IEnumerator Coloring(float timer) {
		float ticker = 0;
		while(ticker < timer) {
			ticker += Time.deltaTime;
			meshRenderer.material.SetFloat("_HighlightBegin", ticker/timer);
			yield return null;
		}
		coroutine = null;
	}
}