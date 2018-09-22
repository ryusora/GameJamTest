using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
	Player player;

	// Use this for initialization
	void Start () {
		player = transform.GetComponent<Player>();

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			if(player.IsIdling())
				player.Ready();
		}
		else if(Input.GetMouseButton(0)) {
			player.IncreaseForceRatio();
		} else if(Input.GetMouseButtonUp(0)) {
			player.StartJump();
		}
	}
}
