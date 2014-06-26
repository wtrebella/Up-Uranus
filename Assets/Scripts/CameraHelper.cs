using UnityEngine;
using System.Collections;

public class CameraHelper : MonoBehaviour {
	public static CameraHelper instance;

	public Camera gameCam;
	public Camera uiCam;

	public tk2dCamera cam;
	public tk2dCameraAnchor anchorLowerRight;
	public tk2dCameraAnchor anchorLowerLeft;
	
	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 GetUIPosition(Vector3 worldPosition) {
		Vector3 pos = gameCam.WorldToViewportPoint(worldPosition);
		return uiCam.ViewportToWorldPoint(pos);
	}
}
