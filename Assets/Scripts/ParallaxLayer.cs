using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallaxLayer : MonoBehaviour {
	public float linkedObjRelativeSpeed;
	public float spriteWidth;
	public float spriteOverlap = 0.04f;
	public float margin = 0;
	public float verticalVariation = 0;
	public int numSprites = 3;
	public GameObject linkedObj;
	public tk2dSprite sprite;
	public bool useFixedUpdate = false;

	private List<tk2dSprite> sprites;
	private float distanceTraveled = 0;
	private float adjustedSpriteWidthInWorldUnits;
	private float pixelsPerMeter;
	private float totalWidth;
	private Vector3 prevPosLinkedObj;
	private Vector3 baseSpritePos;

	void Awake() {
		prevPosLinkedObj = linkedObj.transform.position;
	}

	void Start () {
		pixelsPerMeter = CameraHelper.instance.cam.SettingsRoot.CameraSettings.orthographicPixelsPerMeter;
		adjustedSpriteWidthInWorldUnits = spriteWidth / pixelsPerMeter - spriteOverlap + margin;

		sprites = new List<tk2dSprite>();
		sprites.Add(sprite);

		baseSpritePos = sprite.transform.position;

		sprite.transform.position = new Vector3(baseSpritePos.x, baseSpritePos.y + Random.Range(0, verticalVariation), baseSpritePos.z);

		for (int i = 1; i < numSprites; i++) {
			tk2dSprite s = (tk2dSprite)Instantiate(sprite);
			s.transform.parent = sprite.transform.parent;
			Vector3 newPos = baseSpritePos;
			newPos.x += adjustedSpriteWidthInWorldUnits * i;
			newPos.y = baseSpritePos.y + Random.Range(0, verticalVariation);
			s.transform.position = newPos;
			sprites.Add(s);
		}

		totalWidth = adjustedSpriteWidthInWorldUnits * sprites.Count;
	}
	
	void Update () {
		if (!linkedObj.rigidbody.isKinematic) {
			useFixedUpdate = true;
			return;
		}

		if (!useFixedUpdate) UpdatePositions();
	}

	void FixedUpdate() {
		if (linkedObj.rigidbody.isKinematic) {
			useFixedUpdate = false;
			return;
		}

		if (useFixedUpdate) UpdatePositions();
	}

	void UpdatePositions() {
		Vector3 curPosLinkedObj = linkedObj.transform.position;
		Vector3 p = transform.localPosition;
		float delta;
		if (useFixedUpdate)	delta = linkedObjRelativeSpeed * linkedObj.rigidbody.velocity.x * Time.fixedDeltaTime;
		else delta = linkedObjRelativeSpeed * (curPosLinkedObj.x - prevPosLinkedObj.x);

		tk2dSprite s;
		Vector3 sp;
		
		p.x += delta;
		transform.localPosition = p;
		
		distanceTraveled += delta;
		
		if (distanceTraveled > adjustedSpriteWidthInWorldUnits) {
			distanceTraveled -= adjustedSpriteWidthInWorldUnits;
			
			s = sprites[sprites.Count - 1];
			sp = s.transform.position;
			
			sprites.Remove(s);
			sprites.Insert(0, s);
			
			sp.x -= totalWidth;
			sp.y = baseSpritePos.y + Random.Range(0, verticalVariation);
			s.transform.position = sp;
		}
		else if (distanceTraveled < -adjustedSpriteWidthInWorldUnits) {
			distanceTraveled += adjustedSpriteWidthInWorldUnits;
			
			s = sprites[0];
			sp = s.transform.position;
			
			sprites.Remove(s);
			sprites.Add(s);
			
			sp.x += totalWidth;
			sp.y = baseSpritePos.y + Random.Range(0, verticalVariation);
			s.transform.position = sp;
		}
		
		prevPosLinkedObj = linkedObj.transform.position;
	}
}
