using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	public ScoreData scoreData;
	public Text scoreText;
	public Text bestScoreText;
	// Use this for initialization
	void Start () {
		scoreData.Init();
		UpdateScoreText();
		UpdateBestScore();
	}

	public void OnEndGame() {
		scoreData.SaveScore();
	}

	public void UpdateScores() {
		UpdateScoreText();
		UpdateBestScore();
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

	const string BEST_SCORE = "Best Score: ";
	void UpdateBestScore() {
		int bestScore = scoreData.GetBestScore();
		bestScoreText.text = (bestScore > 0)?BEST_SCORE + bestScore:"";
	}
}
