using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsController : MonoBehaviour {

	public Platform[] platformsPool;
	public Color[] colorsPool;
	[Tooltip("Min point between platforms")]
	public Vector2 minPosition;
	[Tooltip("Max point between platforms")]
	public Vector2 maxPosition;
	public int platfromsSkip = 2;
	[Space]
	public ScoreData scoreData;

	private Vector3 lastPosition = Vector3.zero;
	private int currentIndex = 0;

	// Use this for initialization

	void Start () {
		lastPosition = this.transform.position;
		InitFirstPlatform();
		for(int i = currentIndex, length = platformsPool.Length; i < length; ++i) {
			SpawnNextPosition();
		}
	}

	void InitFirstPlatform() {
		Platform platform = GetPlatform();
		platform.transform.position = lastPosition;
		platform.HidePerfectZone();
		UpdateColor(platform); 
	}

	void SpawnNextPosition() {
		Platform platform = GetPlatform();
		MovingPattern movingComponent = platform.GetComponent<MovingPattern>();
		if(movingComponent) movingComponent.Release();
		
		Vector3 position = new Vector3(lastPosition.x + Random.Range(minPosition.x, maxPosition.x), lastPosition.y + Random.Range(minPosition.y, maxPosition.y), lastPosition.z);
		platform.transform.position = position;
		platform.ShowPerfectZone();
		UpdateColor(platform);
		lastPosition = position;
		isMovingPreviously = false;
	}

	void SpawnMovingPlatform() {
		Platform platform = GetPlatform();
		platform.transform.position = new Vector3(lastPosition.x + minPosition.x, lastPosition.y + Random.Range(minPosition.y, maxPosition.y), lastPosition.z);
		lastPosition = new Vector3(lastPosition.x + maxPosition.x, lastPosition.y + Random.Range(minPosition.y, maxPosition.y), lastPosition.z);
		platform.ShowPerfectZone();
		// moving pattern
		MovingPattern component = platform.gameObject.AddComponent<MovingPattern>();
		component.SetUpPattern(platform.transform.position, lastPosition, Random.Range(1.0f, 2.5f), Random.Range(1.5f, 3.5f));
		component.StartMoving();
		UpdateColor(platform);
		isMovingPreviously = true;
	}

	void UpdateColor(Platform platform) {
		platform.SetColor(colorsPool[Random.Range(0, colorsPool.Length - 1)]);
		platform.StopColoringCoroutine();
		platform.ResetHighLight();
	}

	Platform GetPlatform() {
		Platform platform = platformsPool[currentIndex];
		currentIndex = (currentIndex + 1)%platformsPool.Length;
		return platform;
	}
	private bool isMovingPreviously = false;
	public void OnDoneChasing() { // listener for camera done chasing
		if(platfromsSkip-- > 0) return;
		int percentage = Random.Range(0, 100);
		if(percentage < scoreData.GetBestScore() && !isMovingPreviously) SpawnMovingPlatform();
		else SpawnNextPosition();
	}

}
