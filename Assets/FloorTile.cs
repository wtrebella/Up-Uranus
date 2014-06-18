using UnityEngine;
using System.Collections;

public class FloorTile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll) {
		Debug.Log(gameObject.name);
	}
}
