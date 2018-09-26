using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPattern : MonoBehaviour {

	private Vector2 minPosition;
	private Vector2 maxPosition;
	private float movingTime;
	private float waitingTime;
	private Coroutine coroutine;
	public void SetUpPattern(Vector2 minPosition, Vector2 maxPosition, float waitTime, float movingTime) {
		this.minPosition = minPosition;
		this.maxPosition = maxPosition;
		this.movingTime = movingTime;
		this.waitingTime = waitTime;
	}

	public void StartMoving() {
		StopMoving();
		coroutine = StartCoroutine(Moving());
	}

	IEnumerator Moving() {
		float ticker = 0;
		int direction = 1;
		while(true) {
			int previousDirection = direction;
			ticker += Time.deltaTime * direction;
			if(ticker < 0) { ticker = 0; direction *= -1; }
			if(ticker > movingTime) { ticker = movingTime; direction *= -1; }
			transform.position = Vector3.Lerp(minPosition, maxPosition, ticker/movingTime);
			if(previousDirection != direction)
				yield return new WaitForSeconds(waitingTime);
			yield return null;
		}
	}

	public void StopMoving() {
		if(coroutine != null)
			StopCoroutine(coroutine);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag.Equals("Player")) {
			other.transform.parent = this.transform;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag.Equals("Player")) {
			other.transform.parent = null;
		}
	}

	public void Release() {
		StopMoving();
		Destroy(this);
	}
}
