using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dCamera))]

public class CameraFollowX : ObjectFollowX {
	override protected void Awake() {
		base.Awake();
	}

	override protected void Start() {
		base.Start();
		offset += -GetComponent<tk2dCamera>().nativeResolutionWidth / 2f;
	}

	override protected void LateUpdate () {
		base.LateUpdate();
	}
}
