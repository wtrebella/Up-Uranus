using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearMissManager : MonoBehaviour {
	public GameObject nearMissPrefab;
	public GameObject uiRoot;

	private Stack<NearMiss> nearMissStack;

	void Start () {
		nearMissStack = new Stack<NearMiss>();
		for (int i = 0; i < 3; i++) {
			nearMissStack.Push(InitNearMissObject());
		}
	}

	private NearMiss InitNearMissObject() {
		NearMiss nearMiss = NGUITools.AddChild(uiRoot, nearMissPrefab).GetComponent<NearMiss>();
		nearMiss.Reset();
		return nearMiss;
	}

	private NearMiss GetNearMissObject() {
		NearMiss nearMiss = null;

		if (nearMissStack.Count > 0) nearMiss = nearMissStack.Pop();
		else nearMiss = InitNearMissObject();

		return nearMiss;
	}

	public void PlayNearMiss(Vector3 worldPosition) {
		NearMiss nearMiss = GetNearMissObject();
		nearMiss.PlayAtPosition(worldPosition);
	}

	public void DoneWithTween(NearMiss nearMiss) {
		nearMiss.Reset();
		nearMissStack.Push(nearMiss);
	}
}
