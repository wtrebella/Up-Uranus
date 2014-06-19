using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MineManager : MonoBehaviour {
	public Mine minePrefab;
	public Vector2 verticalBounds;
	public float minDistFromOtherMine;
	public float maxDistFromOtherMine;

	private List<Mine> mines;

	void Start () {
		mines = new List<Mine>();

		for (int i = 0; i < 15; i++) {
			Mine m = (Mine)Instantiate(minePrefab, new Vector3(-100, -100, 0), Quaternion.identity);
			m.transform.parent = transform;
			m.mineManager = this;
			mines.Add(m);

			if (i == 0) {
				Vector3 newPos = Vector3.zero;
				newPos.x = CameraHelper.instance.anchorLowerRight.transform.position.x + 1;
				newPos.y = Random.Range(verticalBounds.x, verticalBounds.y);

				m.transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
			}
			else {
				PlaceMineAtEnd(m);
			}
		}
	}
	
	void Update () {
		Mine m = mines[0];
		if (m.transform.position.x < CameraHelper.instance.anchorLowerLeft.transform.position.x - 1) PlaceMineAtEnd(m);
	}

	void PlaceMineAtEnd(Mine m) {
		mines.Remove(m);
		Vector3 prevPos = mines[mines.Count - 1].transform.position;
		mines.Add(m);
		Vector3 newPos = prevPos;
		float dist = (newPos - prevPos).magnitude;
		
		while (dist < minDistFromOtherMine || dist > maxDistFromOtherMine) {
			newPos.x = prevPos.x + Random.Range(minDistFromOtherMine, maxDistFromOtherMine);
			newPos.y = verticalBounds.x + Random.Range(0, verticalBounds.y);
			
			dist = (newPos - prevPos).magnitude;
		}
		m.transform.position = newPos;

		m.Setup();
	}

	public void ExplodeMine(Mine m) {
		mines.Remove(m);
		Destroy(m.gameObject);
	}
}
