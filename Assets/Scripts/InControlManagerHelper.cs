using UnityEngine;
using System.Collections;
using InControl;

public class InControlManagerHelper : MonoBehaviour {
	private static InControlManager inControlManager;

	public static void Init() {
		if (inControlManager == null) {
			GameObject go = new GameObject("InControl Manager");
			inControlManager = go.AddComponent<InControlManager>();
			InputManager.AttachDevice(new UnityInputDevice(new KeyboardAndMouseProfile()));
			go.AddComponent<InControlManagerHelper>();
		}
	}

	void Awake () {
		DontDestroyOnLoad(transform.gameObject);
	}
}
