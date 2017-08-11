using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpwanerController : MonoBehaviour
{


	public GameObject entity;

	// Use this for initialization
	void Start ()
	{
		var entityObject = Instantiate(entity, transform.parent.gameObject.transform);
		entityObject.transform.position = transform.position;
		DestroyImmediate(gameObject);
	}
}
