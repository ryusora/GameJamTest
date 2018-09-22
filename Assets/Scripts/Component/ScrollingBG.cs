using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour {
	public MeshRenderer meshRenderer;
	public float speed = 1.0f;
	public void OnPositionChanged(Vector3 pos) {
		Vector3 diff = pos - transform.position;
		if(Mathf.Abs(diff.x) > 0 || Mathf.Abs(diff.y) > 0) {
			Vector2 saveOffset = meshRenderer.material.mainTextureOffset;
			meshRenderer.material.mainTextureOffset = new Vector2(saveOffset.x - diff.x * speed, saveOffset.y - diff.y * speed);
			Debug.Log("=====Before=======");
			Debug.Log(transform.position);
			transform.Translate(diff.x, diff.y, 0.0f, Space.World);
			Debug.Log(transform.position);
			Debug.Log("=====After=======");
		}
	}
}
