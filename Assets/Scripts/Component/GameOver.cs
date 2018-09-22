using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
	public Text score;
	public Text bestScore;

	public ScoreData scoreData;

	private void OnEnable() {
		score.text = scoreData.GetScore().ToString();
		bestScore.text = "Your best: " + scoreData.GetBestScore();
		scoreData.SaveScore();
	}

	public void OnHomePressed() {
		SceneManager.LoadScene(0);
	}
}
