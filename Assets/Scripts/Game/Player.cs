using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
	public enum STATES {
		LANDING,
		READY, // READY to jump
		JUMPING,
		FALLING,
		DEAD,
		MOVING,
		IDLE
	}

	public PlayerData data;
	public GameObject DeadFX;
	public SpriteRenderer spriteRenderer;
	[Space]
	public GameEvent ScoreEvent;
	public GameEvent PerfectEvent;
	public GameEvent HitGroundEvent;
	public FloatEvent PowerChangedEvent;
	public GameEvent GameOverEvent;

	// Use this for initialization
	private float forceLength;
	private Rigidbody2D rigBody2D;
	private STATES state;
	private float previousY;
	private Animator animator;
	private Vector3 destPos;
	void Start () {
		this.rigBody2D = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();
		this.forceLength = 0;
		this.SetState(STATES.FALLING);
	}

	private void OnEnable() {
		spriteRenderer.color = data.color;
	}

	void StartMovingToCenter() {
		StartCoroutine(MoveToCenter());
	}

	IEnumerator MoveToCenter() {
		yield return new WaitForSeconds(0.5f);
		// float distance = (destPos - transform.position).normalized.magnitude;
		// Vector3 oldPos = transform.position;
		// while(Mathf.Abs(destPos.x - transform.position.x) > 0.1f) {
		// 	Debug.Log("[Distance] " + Mathf.Abs(destPos.x - transform.position.x));
		// 	if(IsDead()) {
		// 		Debug.Log("Break Move To Center");
		// 		yield break; // return when dead
		// 	}
		// 	if(!IsMoving()) {
		// 		SetState(STATES.MOVING);
		// 	}
		// 	ticker += Time.deltaTime;
		// 	Vector3 newPos = Vector3.Lerp(oldPos, destPos, Mathf.Min(ticker/distance, 1));
		// 	transform.Translate(newPos.x - transform.position.x, 0, 0);
		// 	animator.ResetTrigger("Move");
		// 	animator.SetTrigger("Move");
		// 	yield return null;
		// }
		SetState(STATES.IDLE);
	}

	void SetState(STATES state) {
		if(this.state == state) return;

		ResetAllTriggers();
		this.state = state;
		switch(state) {
			case STATES.IDLE:
				animator.SetTrigger("Idle");
				break;

			case STATES.LANDING:
				animator.SetTrigger("Land");
				this.forceLength = 0;
				SetForceRatio(0);
				HitGroundEvent.Raise();
				StartMovingToCenter();
				break;

			case STATES.JUMPING:
				animator.SetTrigger("Jump");
				this.previousY = this.transform.position.y;
				break;

			case STATES.FALLING:
				animator.SetTrigger("Fall");
				break;
			
			case STATES.READY:
				animator.SetTrigger("Ready");
				break;

			case STATES.DEAD:
				Vector2 pos = transform.position;
				GameObject.Instantiate(DeadFX, pos, Quaternion.identity);
				gameObject.SetActive(false);
				GameOverEvent.Raise();
				break;

			case STATES.MOVING:
				break;
		}
	}

	void ResetAllTriggers() {
		animator.ResetTrigger("Ready");
		animator.ResetTrigger("Land");
		animator.ResetTrigger("Jump");
		animator.ResetTrigger("Fall");
		animator.ResetTrigger("Move");
		animator.ResetTrigger("Idle");
	}

	public void Ready() {
		SetState(STATES.READY);
	}

	public void StartJump() {
		if(IsReady()) {
			SetState(STATES.JUMPING);
			AddJumpForce(data.maxForce * Mathf.Max(data.forceRatio, data.minForceRatio));
		}
	}

	public void SetForceRatio(float value) {
		data.forceRatio = Mathf.Min(value, 1);
		PowerChangedEvent.Raise(data.forceRatio);
	}

	public void IncreaseForceRatio() {
		if(IsReady())
			SetForceRatio(data.forceRatio + data.speed * Time.deltaTime);
	}

	public bool IsJumping() { return this.state == STATES.JUMPING; }
	public bool IsFalling() { return this.state == STATES.FALLING; }
	public bool IsLanding() { return this.state == STATES.LANDING; }
	public bool IsIdling() { return this.state == STATES.IDLE; }
	public bool IsMoving() { return this.state == STATES.MOVING; }
	public bool IsReady() { return this.state == STATES.READY; }

	public bool IsDead() { return this.state == STATES.DEAD; }

	void AddJumpForce(Vector2 force) {
		this.forceLength += force.magnitude;
		this.rigBody2D.AddForce(force);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag.Equals("Ground")) {
			Platform hitPlatform = other.gameObject.GetComponentInParent<Platform>();
			hitPlatform.HidePerfectZone();
			hitPlatform.SetColor(data.color);
			destPos = new Vector3(hitPlatform.transform.position.x, transform.position.y, transform.position.z);
			Debug.Log("Dest Pos " + destPos);
			SetState(STATES.LANDING);
			ScoreEvent.Raise();
		} else if (other.gameObject.tag.Equals("DeadZone") && !IsDead()) {
			SetState(STATES.DEAD);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag.Equals("PerfectZone")) {
			PerfectEvent.Raise();
		}
	}

	void Update() {
		if(IsFalling()) {
			this.rigBody2D.AddForce(data.dropForce);
		} else if(IsJumping()) {
			if(this.previousY > this.transform.position.y) {
				SetState(STATES.FALLING);
			}
			this.previousY = this.transform.position.y;
		}
	}
}
