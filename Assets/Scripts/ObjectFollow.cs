using UnityEngine;
using System.Collections;

public class ObjectFollow : MonoBehaviour {
	public bool followX = true;
	public bool followY = true;

	public GameObject[] objectsThatFollow;

	protected Vector2 previousPos = Vector2.zero;

	protected virtual void Awake() {

	}

	protected virtual void Start() {
		if (followX) previousPos.x = transform.position.x;
		if (followY) previousPos.y = transform.position.y;
	}

	protected virtual void LateUpdate () {
		foreach (GameObject o in objectsThatFollow) {
			Vector3 p = o.transform.position;

			if (followX) {
				float x = p.x + (transform.position.x - previousPos.x);
				o.transform.position = new Vector3(x, p.y, p.z);
			}

			if (followY) {
				float y = p.y + (transform.position.y - previousPos.y);
				o.transform.position = new Vector3(p.x, y, p.z);
			}
		}

		previousPos.x = transform.position.x;
		previousPos.y = transform.position.y;
	}
}
