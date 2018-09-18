using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ScoreData : ScriptableObject {
#if UNITY_EDITOR
    [MenuItem("Menu/Scriptable/ScoreData")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ScoreData>();
    }
#endif
    private int score;
    private int bestScore;
    const string BEST_SCORE = "BestScore";
    public void Init() {
        this.bestScore = PlayerPrefs.GetInt(BEST_SCORE, 0);
        this.Reset();
    }

    public void SaveScore() {
        PlayerPrefs.SetInt(BEST_SCORE, this.bestScore);
    }

    public void AddScore(int score) {
        this.score += score;
        if (this.score > this.bestScore)
            this.bestScore = this.score;
        Debug.Log("New Score: " + this.score);
    }

    public void Reset() {
        this.score = 0;
    }

    public int GetScore() { return this.score; }
    public int GetBestScore() { return this.bestScore; }
    public bool IsBestScore() { return this.score == this.bestScore; }
}
