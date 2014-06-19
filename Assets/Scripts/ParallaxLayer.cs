using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallaxLayer : MonoBehaviour {
	public float linkedObjRelativeSpeed;
	public float spriteWidth;
	public float spriteOverlap = 0.04f;
	public int numSprites = 3;
	public GameObject linkedObj;
	public List<tk2dSprite> sprites;

	private float distanceTraveled = 0;
	private float pixelsPerMeter;
	private Vector3 prevPosLinkedObj;

	void Start () {
		prevPosLinkedObj = linkedObj.transform.position;
		pixelsPerMeter = CameraHelper.instance.cam.SettingsRoot.CameraSettings.orthographicPixelsPerMeter;
	}
	
	void Update () {
		Vector3 curPosLinkedObj = linkedObj.transform.position;
		Vector3 p = transform.localPosition;
		float spriteWidthInWorldUnits = spriteWidth / pixelsPerMeter - spriteOverlap;
		float totalWidth = spriteWidthInWorldUnits * sprites.Count;
		float delta = linkedObjRelativeSpeed * (curPosLinkedObj.x - prevPosLinkedObj.x);
		tk2dSprite s;
		Vector3 sp;

		p.x += delta;
		transform.localPosition = p;

		distanceTraveled += delta;

		if (distanceTraveled > spriteWidthInWorldUnits) {
			distanceTraveled -= spriteWidthInWorldUnits;

			s = sprites[sprites.Count - 1];
			sp = s.transform.position;

			sprites.Remove(s);
			sprites.Insert(0, s);

			sp.x -= totalWidth;
			s.transform.position = sp;
		}
		else if (distanceTraveled < -spriteWidthInWorldUnits) {
			distanceTraveled += spriteWidthInWorldUnits;

			s = sprites[0];
			sp = s.transform.position;

			sprites.Remove(s);
			sprites.Add(s);

			sp.x += totalWidth;
			s.transform.position = sp;
		}

		prevPosLinkedObj = linkedObj.transform.position;
	}
}
