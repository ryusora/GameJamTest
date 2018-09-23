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
		Vector3 position = new Vector3(lastPosition.x + Random.Range(minPosition.x, maxPosition.x), lastPosition.y + Random.Range(minPosition.y, maxPosition.y), lastPosition.z);
		platform.transform.position = position;
		platform.ShowPerfectZone();
		UpdateColor(platform);
		lastPosition = position;
	}

	void UpdateColor(Platform platform) {
		platform.SetColor(colorsPool[Random.Range(0, colorsPool.Length - 1)]);
	}

	Platform GetPlatform() {
		Platform platform = platformsPool[currentIndex];
		currentIndex = (currentIndex + 1)%platformsPool.Length;
		return platform;
	}
	public void OnDoneChasing() { // listener for camera done chasing
		if(platfromsSkip-- > 0) return;
		SpawnNextPosition();
	}

}
