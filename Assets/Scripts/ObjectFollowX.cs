using UnityEngine;
using System.Collections;

public class ObjectFollowX : MonoBehaviour {
	public GameObject objectThatFollows;

	protected float previousX = 0;

	protected virtual void Awake() {

	}

	protected virtual void Start() {
		previousX = transform.position.x;
	}

	protected virtual void LateUpdate () {
		Vector3 p = objectThatFollows.transform.position;
		float x = p.x + (transform.position.x - previousX);

		objectThatFollows.transform.position = new Vector3(x, p.y, p.z);

		previousX = transform.position.x;
	}
}
