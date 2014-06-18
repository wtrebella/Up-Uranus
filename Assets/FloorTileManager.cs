using UnityEngine;
using System.Collections;

public class FloorTileManager : MonoBehaviour {
	public FloorTile floorTilePrefab;
	public float tileMargin = 0;

	[HideInInspector] public float tileWidth;
	[HideInInspector] public float tileHeight;

	private int maxNumTiles;

	void Awake() {
		tileWidth = floorTilePrefab.transform.localScale.x * ((BoxCollider)floorTilePrefab.collider).size.x;
		tileHeight = tileWidth;
	}

	// Use this for initialization
	void Start () {
		tk2dCameraAnchor anchorLowerRight = CameraHelper.instance.anchorLowerRight;

		float maxX = anchorLowerRight.transform.position.x - tileWidth / 2f;
		maxNumTiles = (int)(maxX / (tileWidth + tileMargin) + 1);

		for (int i = 0; i < maxNumTiles; i++) {
			FloorTile t = (FloorTile)Instantiate(floorTilePrefab);
			t.transform.position = new Vector3((i + 0.5f) * (tileWidth + tileMargin), tileHeight / 2f);
			t.transform.parent = transform;
			t.name = "Tile " + i.ToString();
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
