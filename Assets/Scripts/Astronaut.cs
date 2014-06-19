using UnityEngine;
using System.Collections;
using GifRecorder;

public class Astronaut : MonoBehaviour {
	public float forwardSpeed = 7;
	public float diveAcceleration = -500;
	public float maxDiveSpeed = -100;
	public float reboundSpeed = 14;
	public GameManager gameManager;

	private float timeOfLastFloorHit = 0;
	private bool isDiving = false;
	private Vector3 velocity;
	private CharacterController charController;

	void Start () {
		//Recorder.Record(1024, 768, 4, 30, "upUranusGif.gif");
		velocity = new Vector3(forwardSpeed, 0, 0);
		charController = GetComponent<CharacterController>();
	}

	void Update() {
		if (!isDiving && Input.GetMouseButtonDown(0)) StartCoroutine("Dive");
	}

	void FixedUpdate() {
		if (charController.isGrounded) StopDive();

		UpdateVelocityX();
		UpdateVelocityY();

		charController.Move(velocity * Time.deltaTime);
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

		while (Mathf.Abs(ds) < Mathf.Abs(maxDiveSpeed)) {
			ds = Mathf.Max(ds + diveAcceleration * Time.deltaTime, maxDiveSpeed);

			velocity.y = ds;
			yield return null;
		}

		StopDive();
	}

	void StopDive() {
		StopCoroutine("Dive");
		isDiving = false;
	}
	
	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "FloorTile") {
			if (Time.time - timeOfLastFloorHit < 0.05f) return;

			timeOfLastFloorHit = Time.time;

			coll.GetComponent<FloorTile>().Break();
		}

		else if (coll.tag == "Mine") {
			coll.GetComponent<Mine>().Explode();
			Die();
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.tag == "NearMissZone") {
			Mine m = coll.transform.parent.GetComponent<Mine>();
			if (!m.hasBeenNearMissed) m.NearMiss();
		}
	}

	void Die() {
		Destroy(gameObject);
		gameManager.GameOver();
	}
}
