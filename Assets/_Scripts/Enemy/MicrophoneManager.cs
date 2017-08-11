using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{

	public float spawnChance = 0.5f;

	private Animator animator;
	
	// Use this for initialization
	void Start () {
		float random = Random.Range(0f, 1f);
		if (random > spawnChance)
		{
			Destroy(this.gameObject);
		}

		animator = GetComponent<Animator>();

		animator.SetFloat("offset", Random.Range(0f, 1f));
		animator.SetBool("bush", false);
		if (Random.Range(0f, 1f) > 0.5)
		{
			animator.SetBool("bush", true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
