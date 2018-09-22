using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform follower;
	public Vector3 distanceToFollower;
	public float smoothValue = 0.5f;
	public Vector3Event PositionChangedEvent; 
	public GameEvent DoneChasingEvent;
	private Coroutine coroutine = null;
	private 
	// Use this for initialization
	void Start () {
	}

	public void StartShaking() {
		if(coroutine != null)
			StopCoroutine(coroutine);
		coroutine = StartCoroutine(Shaking());
	}

	IEnumerator Shaking() {
		float timer = 0.25f;
		float shakeValue = 0.15f;
		Vector3 defaultPosition = transform.position;
		while(timer > 0) {
			timer -= Time.deltaTime;
			Vector3 position = defaultPosition;
			position.y += shakeValue * Mathf.Cos(Random.Range(-Mathf.PI, Mathf.PI));
			transform.position = position;
			yield return null;
		}
		transform.position = defaultPosition;
		yield return StartCoroutine(ChaseToFollower());
	}

	IEnumerator ChaseToFollower(){
		Vector3 destPos = follower.position + distanceToFollower;
		float distance = (destPos - transform.position).normalized.magnitude;
		float ticker = 0;
		Vector3 oldPos = transform.position;
		while(Vector3.Distance(destPos, transform.position) > 0.01f) {
			ticker += Time.deltaTime;
			transform.position = Vector3.Lerp(oldPos, destPos, Mathf.Min(ticker/distance));
			PositionChangedEvent.Raise(transform.position);
			yield return null;
		}
		Debug.Log("Done Chasing Follower");
		DoneChasingEvent.Raise();
		coroutine = null;
	}
}
