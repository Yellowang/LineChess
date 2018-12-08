using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipsisJump : MonoBehaviour {

	private float countTime;
	private Transform[] points;

	void Start() {
		
		countTime = 0;
		points = new Transform[transform.childCount];

		for (int i = 0; i < transform.childCount; i++) {
			points [i] = transform.GetChild (i).transform;
		}

	}

	void Update () {

		countTime += Time.deltaTime;

		if (Mathf.FloorToInt (countTime) % 2 == 0) {
			for (int i = 1; i < transform.childCount; i += 2) {
				points [i].localPosition = new Vector3(points [i].localPosition.x, 0.5f, points [i].localPosition.z);
			}
			for (int i = 0; i < transform.childCount; i += 2) {
				points [i].localPosition = new Vector3(points [i].localPosition.x, 0f, points [i].localPosition.z);
			}
		} else {
			for (int i = 1; i < transform.childCount; i += 2) {
				points [i].localPosition = new Vector3(points [i].localPosition.x, 0f, points [i].localPosition.z);
			}
			for (int i = 0; i < transform.childCount; i += 2) {
				points [i].localPosition = new Vector3(points [i].localPosition.x, 0.5f, points [i].localPosition.z);
			}
		}

	}
}
