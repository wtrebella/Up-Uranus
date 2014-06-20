using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorTileManager : MonoBehaviour {
	public FloorTile floorTilePrefab;
	public float tileMargin = 0;

	[HideInInspector] public float tileWidth;
	[HideInInspector] public float tileHeight;

	private List<FloorTile> floorTiles;

	private int maxNumTiles;

	void Awake() {
		floorTiles = new List<FloorTile>();
	}

	void Start () {
		tk2dCameraAnchor anchorLowerRight = CameraHelper.instance.anchorLowerRight;

		tileWidth = floorTilePrefab.transform.localScale.x * ((BoxCollider)floorTilePrefab.collider).size.x;
		tileHeight = floorTilePrefab.transform.localScale.y * ((BoxCollider)floorTilePrefab.collider).size.y;

		float maxX = anchorLowerRight.transform.position.x - tileWidth / 2f;
		maxNumTiles = (int)(maxX / (tileWidth + tileMargin) + 8);

		for (int i = 0; i < maxNumTiles; i++) {
			FloorTile t = (FloorTile)Instantiate(floorTilePrefab);
			t.transform.position = new Vector3((i + 0.5f) * (tileWidth + tileMargin), tileHeight / 2f);
			t.transform.parent = transform;
			floorTiles.Add(t);
		}
	}
	
	void Update () {
		Vector3 camOrigin = CameraHelper.instance.anchorLowerLeft.transform.position;

		FloorTile t = floorTiles[0];
		if (t.transform.position.x < camOrigin.x - tileWidth) PlaceTileAtEnd(t);
	}

	void PlaceTileAtEnd(FloorTile t) {
		floorTiles.Remove(t);
		FloorTile lastTile = floorTiles[floorTiles.Count - 1];
		floorTiles.Add(t);
		Vector3 lastTilePos = lastTile.transform.position;
		t.transform.position = lastTilePos + new Vector3(tileWidth + tileMargin, 0, 0);
		t.GetComponent<FloorTile>().Setup();
	}
}
