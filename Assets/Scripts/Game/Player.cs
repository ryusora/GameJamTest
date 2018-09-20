using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
	public enum STATES {
		LANDING,
		JUMPING,
		FALLING
	}

	public PlayerData data;
	public GameEvent ScoreEvent;
	public GameEvent PerfectEvent;
	public GameEvent HitGroundEvent;
	public GameObject DeadFX;

	// Use this for initialization
	private float forceLength;
	private Rigidbody2D rigBody2D;
	private STATES state;
	private float previousY;
	private Animator animator;

	void Start () {
		this.rigBody2D = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();
		this.forceLength = 0;
		this.SetState(STATES.FALLING);
	}

	void SetState(STATES state) {
		this.state = state;
		switch(state) {
			case STATES.LANDING:
				animator.ResetTrigger("Jump");
				animator.SetTrigger("Land");
				this.forceLength = 0;
				break;

			case STATES.JUMPING:
				animator.ResetTrigger("Land");
				animator.SetTrigger("Jump");
				this.previousY = this.transform.position.y;
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
			ScoreEvent.Raise();
			HitGroundEvent.Raise();
		} else if(other.gameObject.tag.Equals("PerfectZone")) {
			SetState(STATES.LANDING);
			PerfectEvent.Raise();
		} else if (other.gameObject.tag.Equals("DeadZone")) {
			Vector2 pos = this.transform.position;
			GameObject.Instantiate(this.DeadFX, pos, Quaternion.identity);
			gameObject.SetActive(false);
		}
	}

	void Update() {
		if(IsFalling()) {
			this.rigBody2D.AddForce(data.hulkForce);
		} else if(IsJumping()) {
			if(this.previousY > this.transform.position.y) {
				SetState(STATES.FALLING);
			}
			this.previousY = this.transform.position.y;
		}
	}
}
