using UnityEngine;
using System.Collections;
using InControl;

public class GameManager : MonoBehaviour {
	public BoxCollider floorCollider;

	private bool isGameRunning = true;

	void Awake() {
		InControlManagerHelper.Init();
	}

	void Start () {

	}
	
	void Update () {
		if (!isGameRunning && InputManager.ActiveDevice.Action1.WasPressed) Application.LoadLevel(0);
	}

	public void GameOver() {
		isGameRunning = false;
		Vector3 p = floorCollider.transform.position;
		p.y = 0.2f;
		floorCollider.transform.position = p;
	}
}
