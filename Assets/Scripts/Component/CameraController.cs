using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform follower;
	public Vector3 distanceToFollower;
	public float smoothValue = 0.05f;
	private Coroutine chasingCoroutine = null;
	private 
	// Use this for initialization
	void Start () {
	}

	public void StartShaking() {
		StartCoroutine(Shaking());
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
		Vector3 velocity = Vector3.zero;
		while(destPos != transform.position) {
			transform.position = Vector3.SmoothDamp(transform.position, destPos, ref velocity, smoothValue);
			yield return null;
		}
		Debug.Log("Done Chasing Follower");
	}
}
