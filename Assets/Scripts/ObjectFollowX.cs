using UnityEngine;
using System.Collections;

public class ObjectFollowX : MonoBehaviour {
	public GameObject objectToFollow;

	protected float previousX = 0;

	protected virtual void Awake() {

	}

	protected virtual void Start() {
		previousX = objectToFollow.transform.position.x;
	}

	protected virtual void LateUpdate () {
		Vector3 p = transform.position;
		float x = p.x + (objectToFollow.transform.position.x - previousX);

		transform.position = new Vector3(x, p.y, p.z);

		previousX = objectToFollow.transform.position.x;
	}
}
