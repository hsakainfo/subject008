using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawner : MonoBehaviour {

	public float viewAngle;//the maximun amount to rotate away from its spawn looking direction
	public float rotationSpeed;
	public float startAngle;
	public bool keepRotating;
	public GameObject entity;

	// Use this for initialization
	void Start () {

		var entityObject = Instantiate(entity, transform.parent.gameObject.transform);
		entityObject.transform.position = transform.position;
		var cameraManager = entityObject.GetComponent<CameraManager>();

		cameraManager.viewAngle = viewAngle;
		cameraManager.rotationSpeed = rotationSpeed;
		cameraManager.startAngle = startAngle;
		cameraManager.keepRotating = keepRotating;
		DestroyImmediate(gameObject);

	}

	// Update is called once per frame
	void Update () {
	}
}
