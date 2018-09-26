using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform follower;
	public Vector3 distanceToFollower;
	public float smoothValue = 0.5f;
	[Space]
	public float shakeTimer = 0.25f;
	public float shakeAmount = 0.15f;
	[Space]
	public Vector3Event PositionChangedEvent; 
	public GameEvent DoneChasingEvent;
	private Coroutine coroutine = null;
	
	[SerializeField]
	private bool isHitMovingPlatform = false;
	// Use this for initialization
	void Start () {
	}

	public void HitMovingPlatform() {
		isHitMovingPlatform = true;
	}

	public void StopFollowingPlayer() {
		if(coroutine != null)
			StopCoroutine(coroutine);
		isHitMovingPlatform = false;
	}

	public void StartShaking() {
		if(coroutine != null)
			StopCoroutine(coroutine);
		coroutine = StartCoroutine(ShakeBeforeChaseFollower());
	}

	IEnumerator ShakeBeforeChaseFollower() {
		yield return StartCoroutine(Shaking());
		yield return StartCoroutine(ChaseToFollower());
		coroutine = null;
	}

	IEnumerator Shaking() {
		float timer = shakeTimer;
		Vector3 oldPosition = transform.position;
		while(timer > 0) {
			timer -= Time.deltaTime;
			Vector3 position = oldPosition;
			position.y += shakeAmount * Mathf.Cos(Random.Range(-Mathf.PI, Mathf.PI));
			transform.position = position;
			yield return null;
		}
		transform.position = oldPosition;
	}

	IEnumerator ChaseToFollower(){
		Vector3 destPosition = follower.position + distanceToFollower;
		float distance = Vector3.Distance(destPosition, transform.position);
		float ticker = 0;
		Vector3 oldPosition = transform.position;
		while(Vector3.Distance(destPosition, transform.position) > 0.01f || isHitMovingPlatform) {
			ticker += Time.deltaTime * smoothValue;
			transform.position = Vector3.Lerp(oldPosition, destPosition, Mathf.Min(ticker/distance));
			PositionChangedEvent.Raise(transform.position);
			if(isHitMovingPlatform) 
				destPosition = follower.position + distanceToFollower;
			yield return null;
		}
		DoneChasingEvent.Raise();
	}
}
