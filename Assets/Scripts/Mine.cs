using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
	public bool hasBeenNearMissed {get; private set;}
	public Vector2 explosionForce;

	[HideInInspector] public MineManager mineManager;
	
	void Start () {
		hasBeenNearMissed = false;
	}
	
	void Update () {
	
	}

	public void Setup() {
		hasBeenNearMissed = false;
	}

	public void Explode() {
		mineManager.ExplodeMine(this);
	}

	public void NearMiss() {
		hasBeenNearMissed = true;
	}
}
