using UnityEngine;
using System.Collections;

public class FloorTile : MonoBehaviour {
	public AudioClip breakSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {

	}

	public void Setup() {
		GetComponent<MeshRenderer>().enabled = true;
	}

	public void Break() {
		GetComponent<MeshRenderer>().enabled = false;
		AudioSource.PlayClipAtPoint(breakSound, Vector3.zero, 0.05f);
	}
}
