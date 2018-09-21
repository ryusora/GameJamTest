using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour {
	public RectTransform FillBar;
	public float height;
	
	public void SetNextProgress (float progress) {
		SetProgress(progress);
	}

	public void SetProgress(float progress) {
		FillBar.anchoredPosition = new Vector2(FillBar.anchoredPosition.x, Mathf.Min(progress * height, height));
	}
}
