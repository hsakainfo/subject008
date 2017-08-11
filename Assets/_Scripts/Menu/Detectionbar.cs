using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detectionbar : MonoBehaviour
{

	public float detectionLevel;
	public Image detectionBar;
	private AwarenessController awarenessController;
	
	
	

	// Use this for initialization
	void Start ()
	{
		detectionBar.fillAmount = 0.0f;
		awarenessController = EventController.Instance.GetComponent<AwarenessController>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		detectionBar.fillAmount = awarenessController.detectionLevel / awarenessController.maxDetectionUntilAlarm;
		

	}
}
