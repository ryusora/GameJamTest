using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public enum STATES {
		IDLING,
		LANDING,
		JUMPING,
		FALLING
	}

	public PlayerData data;


	// Use this for initialization
	private float forceLength;
	private Rigidbody2D rigBody2D;
	private STATES state;
	private float startY;
	private float previousY;
	private float halfWayY;
	void Start () {
		this.rigBody2D = this.gameObject.GetComponent<Rigidbody2D>();
		this.forceLength = 0;
		this.state = STATES.IDLING;
		this.startY = this.previousY = this.halfWayY = this.transform.position.y;
	}

	void SetState(STATES state) {
		this.state = state;
		switch(state) {
			case STATES.LANDING:
				this.forceLength = 0;
				break;

			case STATES.JUMPING:
				this.startY = this.previousY = this.transform.position.y;
				break;

			case STATES.FALLING:
				break;
		}
		Debug.Log("Set State: " + state.ToString());
	}

	public void StartJump() {
		if(IsLanding()) {
			SetState(STATES.JUMPING);
			AddJumpForce(this.data.startJumpForce);
		}
	}

	public void ContinueJumping() {
		if(IsJumping() && this.forceLength < this.data.MAX_FORCE_LENGTH)
			AddJumpForce(this.data.additionalJumpForce);
	}

	public bool IsJumping() { return this.state == STATES.JUMPING; }
	public bool IsFalling() { return this.state == STATES.FALLING; }
	public bool IsLanding() { return this.state == STATES.LANDING; }

	void AddJumpForce(Vector2 force) {
		this.forceLength += force.magnitude;
		this.rigBody2D.AddForce(force);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag.Equals("Ground")) {
			SetState(STATES.LANDING);
		}
	}

	void Update() {
		if(IsFalling()) {
			if(this.transform.position.y <= this.halfWayY) {
				this.rigBody2D.AddForce(data.hulkForce);
				Debug.Log("Add Hulk Force");
			}
		} else if(IsJumping()) {
			if(this.previousY > this.transform.position.y) {
				SetState(STATES.FALLING);
				this.halfWayY = (this.previousY + this.startY)/2;
			}
			this.previousY = this.transform.position.y;
		}
	}
}
