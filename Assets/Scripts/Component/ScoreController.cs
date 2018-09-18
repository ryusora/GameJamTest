using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {
	public ScoreData scoreData;
	// Use this for initialization
	void Start () {
		scoreData.Init();
	}
	
	public void OnScoreEvent() {
		scoreData.AddScore(1);
	}

	public void OnPerfectEvent() {
		scoreData.AddScore(1);
	}
}
