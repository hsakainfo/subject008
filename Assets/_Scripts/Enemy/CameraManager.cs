using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour
{

	public float spawnChance = 0.5f;

	private Animator animator;
	public float viewAngle;//the maximun amount to rotate away from its spawn looking direction
	public float rotationSpeed;
	public float startAngle;
	private float spawnAngleVector;
	private float currentDegreeAwayFromSpawnLookingDirection;//relatively to spawn angle
	public bool keepRotating;

	private Transform coneTransform; 
	// Use this for initialization
	void Start () {
		float random = Random.Range(0f, 1f);
		
		if (random > spawnChance)
			Destroy(this.gameObject);
		
		animator = GetComponent<Animator>();

		animator.SetFloat("offset", Random.Range(0f, 1f));
		animator.SetBool("bush", false);
		
		if (Random.Range(0f, 1f) > 0.5)
			animator.SetBool("bush", true);

		spawnAngleVector = startAngle;
		currentDegreeAwayFromSpawnLookingDirection = 0;
		
		coneTransform = transform.FindChild("ViewField").transform;
		coneTransform.Rotate(0f, 0f, spawnAngleVector);
		
	}

	private void FixedUpdate() {
		//rotate at certain speed
		if (
			!keepRotating &&
			Mathf.Abs(currentDegreeAwayFromSpawnLookingDirection) >= viewAngle &&
			Math.Sign(currentDegreeAwayFromSpawnLookingDirection) == Math.Sign(rotationSpeed)
		) {
			//rotating away from spawn
			rotationSpeed *= -1;
		}
		
		currentDegreeAwayFromSpawnLookingDirection += rotationSpeed * Time.deltaTime;
		
		coneTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
	}
}
