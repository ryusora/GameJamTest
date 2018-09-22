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
		for(int i = 1, length = platformsPool.Length; i < length; ++i) {
			SpawnNextPosition();
		}
	}

	void InitFirstPlatform() {
		Platform platform = GetNextPlatform(); // first platform
		platform.transform.position = lastPosition;
		Debug.Log("First platform position " + platform.transform.position);
		platform.HidePerfectZone();
		UpdateColor(platform); 
	}

	void SpawnNextPosition() {
		Platform nextPlatform = GetNextPlatform();
		Vector3 nextPosition = new Vector3(lastPosition.x + Random.Range(minPosition.x, maxPosition.x), lastPosition.y + Random.Range(minPosition.y, maxPosition.y), lastPosition.z);
		nextPlatform.transform.position = nextPosition;
		nextPlatform.ShowPerfectZone();
		UpdateColor(nextPlatform);
		Debug.Log("Platform " + currentIndex + " position " + nextPlatform.transform.position);
		lastPosition = nextPosition;
	}

	void UpdateColor(Platform platform) {
		platform.SetColor(colorsPool[Random.Range(0, colorsPool.Length - 1)]);
	}

	Platform GetNextPlatform() {
		Platform platform = platformsPool[currentIndex];
		currentIndex = (currentIndex + 1)%platformsPool.Length;
		return platform;
	}
	public void OnDoneChasing() { // listener for camera done chasing
		if(platfromsSkip-- > 0) return;
		SpawnNextPosition();
	}

}
