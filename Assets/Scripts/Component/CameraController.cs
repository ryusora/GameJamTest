using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform follower;
	public Vector3 distanceToFollower;
	public float smoothValue = 0.05f;
	private Coroutine chasingCoroutine = null;
	// Use this for initialization
	void Start () {
		
	}

	public void StartChasingFollower(){
		if(chasingCoroutine != null)
			StopCoroutine(chasingCoroutine);
		chasingCoroutine = StartCoroutine(ChaseToFollower());
	}
	
	IEnumerator ChaseToFollower(){
		Vector3 destPos = follower.position + distanceToFollower;
		Vector3 velocity = Vector3.zero;
		while(destPos != transform.position) {
			transform.position = Vector3.SmoothDamp(transform.position, destPos, ref velocity, smoothValue);
			yield return null;
		}
	}
}
