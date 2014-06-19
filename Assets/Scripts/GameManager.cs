using UnityEngine;
using System.Collections;
using InControl;

public class GameManager : MonoBehaviour {
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
	}
}
