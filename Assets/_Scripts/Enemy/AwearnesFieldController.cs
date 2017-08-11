using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwearnesFieldController : MonoBehaviour
{

	private Transform ViewFieldTransform;

	private AwarenessController _awarenessController;
	// Use this for initialization
	void Start ()
	{
		ViewFieldTransform = this.gameObject.transform;
		_awarenessController = EventController.Instance.GetComponent<AwarenessController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float temp = _awarenessController.detectionLevel /_awarenessController.maxDetectionUntilAlarm;
		ViewFieldTransform.localScale = new Vector3(temp,temp,0); 
	}
}
