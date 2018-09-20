using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	public ScoreData scoreData;
	public Text scoreText;
	// Use this for initialization
	void Start () {
		scoreData.Reset();
		UpdateScoreText();
	}
	
	public void OnScoreEvent() {
		scoreData.AddScore(1);
		UpdateScoreText();
	}

	public void OnPerfectEvent() {
		scoreData.AddScore(1);
		UpdateScoreText();
	}

	void UpdateScoreText() {
		scoreText.text = scoreData.GetScore().ToString();
		scoreText.gameObject.GetComponent<Animator>().SetTrigger("Scoring");
	}
}
