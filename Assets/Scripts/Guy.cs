using UnityEngine;
using System.Collections;
using GifRecorder;

public class Guy : MonoBehaviour {
	public float forwardSpeed;
	public float diveAcceleration;
	public float maxDiveSpeed;
	public float reboundSpeed;

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
		if (Time.time - timeOfLastFloorHit < 0.05f) return;

		timeOfLastFloorHit = Time.time;

		coll.GetComponent<FloorTile>().Break();
	}
}
