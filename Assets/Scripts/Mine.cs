using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
	public bool hasBeenNearMissed {get; private set;}
	public Vector2 explosionForce;
	public AudioClip explosionSound;
	public AudioClip nearMissSound;
	
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
		AudioSource.PlayClipAtPoint(explosionSound, Vector3.zero);
		mineManager.ExplodeMine(this);
	}

	public void NearMiss() {
		AudioSource.PlayClipAtPoint(nearMissSound, Vector3.zero);
		hasBeenNearMissed = true;
	}
}
