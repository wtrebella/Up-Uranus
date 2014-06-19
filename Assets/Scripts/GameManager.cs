using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private bool isGameRunning = true;

	void Start () {
	
	}
	
	void Update () {
		if (!isGameRunning && Input.GetMouseButtonDown(0)) Application.LoadLevel(0);
	}

	public void GameOver() {
		isGameRunning = false;
	}
}
