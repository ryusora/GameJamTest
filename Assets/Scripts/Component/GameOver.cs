using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
	public Text score;
	public Text bestScore;

	public ScoreData scoreData;
	const string YOUR_BEST = "Your best: ";
	private void OnEnable() {
		score.text = scoreData.GetScore().ToString();
		bestScore.text = YOUR_BEST + scoreData.GetBestScore();
		scoreData.SaveScore();
	}

	public void OnHomePressed() {
		SceneManager.LoadScene("Game");
	}
}
