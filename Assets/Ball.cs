using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public float forwardSpeed;
	public float maxDiveSpeed;
	public float diveAcceleration;
	public float reboundSpeed;
	public FloorTileManager floorTileManager;

	private float ballHeight;
	private float floorHeight;
	private float minY;
	private bool isDiving = false;

	// Use this for initialization
	void Start () {
		floorHeight = floorTileManager.tileHeight;

		SphereCollider ballCollider = (SphereCollider)collider;
		ballHeight = ballCollider.radius * 2 * transform.localScale.y;
		minY = floorHeight / 2f + ballHeight / 2f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		MoveForward();

		if (Input.GetMouseButtonDown(0)) {
			if (!isDiving) StartCoroutine(Dive());
		}
	}

	void MoveForward() {
		Vector3 v = rigidbody.velocity;

		rigidbody.velocity = new Vector3(forwardSpeed, v.y, 0);
	}

	IEnumerator Dive() {
		StopCoroutine(Dive());

		isDiving = true;
		Vector3 v = rigidbody.velocity;
		float ds = v.y;

		while (Mathf.Abs(ds) < Mathf.Abs(maxDiveSpeed)) {
			ds = Mathf.Max(v.y + diveAcceleration, maxDiveSpeed);
			v = new Vector3(v.x, ds, 0);
			rigidbody.velocity = v;

			yield return null;
		}

		isDiving = false;
	}

	void OnTriggerEnter(Collider coll) {
		Vector3 p = transform.position;
		Vector3 v = rigidbody.velocity;

		if (coll.gameObject.tag == "Floor") {
			isDiving = false;
			transform.position = new Vector3(p.x, minY, p.z);
			rigidbody.velocity = new Vector3(forwardSpeed, reboundSpeed, 0);
		}
	}
}
