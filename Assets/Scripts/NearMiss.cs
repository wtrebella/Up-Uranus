using UnityEngine;
using System.Collections;

public class NearMiss : MonoBehaviour {
	public AudioClip nearMissSound;

	private TweenScale tweenScale;
	private UILabel uiLabel;

	void Awake () {
		tweenScale = GetComponent<TweenScale>();
		uiLabel = GetComponent<UILabel>();

		NearMissManager nmm = GameObject.Find("Near Miss Manager").GetComponent<NearMissManager>();
		EventDelegate del = new EventDelegate(nmm, "DoneWithTween");
		EventDelegate.Parameter p = new EventDelegate.Parameter(this, "NearMiss");
		del.parameters.SetValue(p, 0);
		EventDelegate.Add(tweenScale.onFinished, del);
	}

	public void PlayAtPosition(Vector3 worldPosition) {
		uiLabel.enabled = true;
		transform.position = CameraHelper.instance.GetUIPosition(worldPosition);
		tweenScale.PlayForward();
	}

	public void Reset() {
		uiLabel.enabled = false;
		tweenScale.ResetToBeginning();
	}
}
