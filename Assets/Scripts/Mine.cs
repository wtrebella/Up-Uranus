using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
	public bool hasBeenNearMissed {get; private set;}
	public Vector2 explosionForce;
	public AudioClip explosionSound;
	public AudioClip nearMissSound;
	public GameObject nearMissPrefab;

	[HideInInspector] public MineManager mineManager;

	private tk2dSpriteAnimator animator;

	void Awake() {
		animator = GetComponent<tk2dSpriteAnimator>();
	}

	void Start () {
		hasBeenNearMissed = false;
	}
	
	void Update () {
	
	}

	public void Setup() {
		hasBeenNearMissed = false;
		animator.Play("Idle");
	}

	public void Explode() {
		AudioSource.PlayClipAtPoint(explosionSound, Vector3.zero);
		mineManager.ExplodeMine(this);
	}

	public void NearMiss() {
		GameObject nm = NGUITools.AddChild(GameObject.Find("UI Root"), nearMissPrefab);
		nm.transform.position = CameraHelper.instance.GetUIPosition(transform.position);

		animator.Play("Disarm");
		AudioSource.PlayClipAtPoint(nearMissSound, Vector3.zero);
		hasBeenNearMissed = true;
	}
}
