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
	public SpriteRenderer spriteRenderer;
	public ParticleSystem trailParticleSystem;
	[Space]
	public GameObject DeadFX;
	[Space]
	[Tooltip("Waiting Time for switching from landing to idling")]
	public float waitTime = 0.5f;
	[Space]
	public GameEvent ScoreEvent;
	public GameEvent PerfectEvent;
	public GameEvent HitGroundEvent;
	public FloatEvent PowerChangedEvent;
	public GameEvent GameOverEvent;
	public GameEvent HitMovingPlatform;
	public GameEvent PlayerJump;

	private Rigidbody2D rigBody2D;
	private STATES state;
	private float previousY;
	private Animator animator;

	// Use this for initialization
	void Start () {
		this.rigBody2D = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();
		this.SetState(STATES.FALLING);
	}

	private void OnEnable() {
		spriteRenderer.color = data.color;
	}

	void StartWaitForIdling() {
		StartCoroutine(WaitForIdling());
	}

	IEnumerator WaitForIdling() {
		yield return new WaitForSeconds(waitTime);
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
				SetForceRatio(0);
				HitGroundEvent.Raise();
				StartWaitForIdling();
				if(trailParticleSystem.isPlaying) trailParticleSystem.Stop();
				break;

			case STATES.JUMPING:
				if(!trailParticleSystem.isPlaying) trailParticleSystem.Play();
				PlayerJump.Raise();
				animator.SetTrigger("Jump");
				this.previousY = this.transform.position.y;
				this.rigBody2D.AddForce(data.maxForce * Mathf.Max(data.forceRatio, data.minForceRatio));
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

	public bool IsJumping() { return this.state == STATES.JUMPING; }
	public bool IsFalling() { return this.state == STATES.FALLING; }
	public bool IsLanding() { return this.state == STATES.LANDING; }
	public bool IsIdling() { return this.state == STATES.IDLE; }
	public bool IsMoving() { return this.state == STATES.MOVING; }
	public bool IsReady() { return this.state == STATES.READY; }

	public bool IsDead() { return this.state == STATES.DEAD; }

	void ResetAllTriggers() {
		animator.ResetTrigger("Ready");
		animator.ResetTrigger("Land");
		animator.ResetTrigger("Jump");
		animator.ResetTrigger("Fall");
		animator.ResetTrigger("Move");
		animator.ResetTrigger("Idle");
	}

	public void Ready() {
		if(IsIdling()) SetState(STATES.READY);
	}

	public void Jump() {
		if(IsReady()) SetState(STATES.JUMPING);
	}

	public void SetForceRatio(float value) {
		data.forceRatio = Mathf.Min(value, 1);
		PowerChangedEvent.Raise(data.forceRatio);
	}

	public void IncreaseForceRatio() {
		if(IsReady()) SetForceRatio(data.forceRatio + data.speed * Time.deltaTime);
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

	private void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag.Equals("Ground")) {
			Platform hitPlatform = other.gameObject.GetComponentInParent<Platform>();
			hitPlatform.HidePerfectZone();
			if(hitPlatform.GetComponent<MovingPattern>() != null)
				HitMovingPlatform.Raise();
			SetState(STATES.LANDING);
			ScoreEvent.Raise();
		} 
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag.Equals("PerfectZone")) {
			other.gameObject.GetComponentInParent<Platform>().StartColoring(data.color, 1.0f);
			PerfectEvent.Raise();
		} else if (other.gameObject.tag.Equals("DeadZone") && !IsDead()) {
			SetState(STATES.DEAD);
		}
	}
}
