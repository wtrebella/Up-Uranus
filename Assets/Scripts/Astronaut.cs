using UnityEngine;
using System.Collections;
using GifRecorder;
using InControl;

public class Astronaut : MonoBehaviour {
	public bool shouldHitMines = true;
	public float forwardSpeed = 7;
	public float diveAcceleration = -500;
	public float maxDiveSpeed = -100;
	public float reboundSpeed = 14;
	public float maxDeathTorque = 100;
	public GameManager gameManager;
	
	private float timeOfLastFloorHit = 0;
	private bool isDiving = false;
	private bool isAlive = true;
	private Vector3 velocity;
	private CharacterController charController;

	void Start () {
		//Recorder.Record(1024, 768, 4, 30, "upUranusGif.gif");
		velocity = new Vector3(forwardSpeed, 0, 0);
		charController = GetComponent<CharacterController>();
	}

	void Update() {
		if (!isDiving && InputManager.ActiveDevice.Action1.WasPressed) StartCoroutine("Dive");
	}

	void FixedUpdate() {
		if (charController.isGrounded) StopDive();

		if (isAlive) {
			UpdateVelocityX();
			UpdateVelocityY();

			charController.Move(velocity * Time.deltaTime);
		}
		else {
			Vector3 v = rigidbody.velocity;
			if (v.x < 0) v.x = 0;
			rigidbody.velocity = v;
		}
	}

	void UpdateVelocityX() {
		velocity.x = forwardSpeed;
	}

	void UpdateVelocityY() {
		if (charController.isGrounded) velocity.y = reboundSpeed;
		else velocity.y += Physics.gravity.y * Time.deltaTime;
	}

	IEnumerator Dive() {
		isDiving = true;

		float ds = velocity.y;

		StartCoroutine(ChangeTrailColor(new Color(0.1f, 1, 0.2f, 0.5f), 0.2f));

		while (Mathf.Abs(ds) < Mathf.Abs(maxDiveSpeed)) {
			ds = Mathf.Max(ds + diveAcceleration * Time.deltaTime, maxDiveSpeed);

			velocity.y = ds;
			yield return null;
		}

		StopDive();
	}

	IEnumerator ChangeTrailColor(Color color, float time) {
		float initialTime = Time.time;
		Material mat = GetComponentInChildren<TrailRenderer>().material;

		while (Time.time - initialTime < time) {
			mat.SetColor("_Color", Color.Lerp(mat.color, color, (Time.time - initialTime) / time));
			yield return null;
		}
	}

	void StopDive() {
		StopCoroutine("Dive");
		isDiving = false;
		StartCoroutine(ChangeTrailColor(new Color(1, 1, 1, 0.5f), 0.2f));
	}
	
	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "FloorTile") {
			if (!isAlive) return;

			if (Time.time - timeOfLastFloorHit < 0.05f) return;

			timeOfLastFloorHit = Time.time;

			coll.GetComponent<FloorTile>().Break();
		}

		else if (coll.tag == "Mine") {
			if (!shouldHitMines) return;
			Mine mine = coll.GetComponent<Mine>();
			mine.Explode();
			Die(mine);
		}

		// change it so mine collisions are just by radius?
	}

	void OnTriggerExit(Collider coll) {
		if (coll.tag == "NearMissZone") {
			Mine m = coll.transform.parent.GetComponent<Mine>();
			if (!m.hasBeenNearMissed) m.NearMiss();
		}
	}

	void Die(Mine mine) {
		isAlive = false;
		charController.enabled = false;
		GetComponent<CapsuleCollider>().enabled = true;
		rigidbody.isKinematic = false;
		rigidbody.useGravity = true;
		rigidbody.velocity = Vector3.zero;
		rigidbody.AddForce(mine.explosionForce.x, mine.explosionForce.y, 0);
		rigidbody.AddRelativeTorque(0, 0, Random.Range(-maxDeathTorque, 0));
		gameManager.GameOver();
	}
}
