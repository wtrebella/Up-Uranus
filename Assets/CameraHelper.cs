using UnityEngine;
using System.Collections;

public class CameraHelper : MonoBehaviour {
	public static CameraHelper instance;

	public tk2dCamera cam;
	public tk2dCameraAnchor anchorLowerRight;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
