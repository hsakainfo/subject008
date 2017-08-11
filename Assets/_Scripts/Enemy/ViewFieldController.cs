using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using _scripts;

public class ViewFieldController : DetectionController
{
	public float rotationSpeed;
	public float rotationTracker;

	private AgentController _agentController;
	private bool isAgentChild;
	private Vector2 viewFieldDirection;
	
	// Use this for initialization
	public override void Start ()
	{
		//Call base to load awareness controller
		base.Start();
		_agentController = GetComponentInParent<AgentController>();
		if (transform.parent.CompareTag("Agent"))
		{
			isAgentChild = true;
		}
		else
		{
			isAgentChild = false;
		}

		if (isAgentChild)
		{
			rotationTracker = 0;
		}
	}

	//angle between agent direction and viewfield direction
	private float CalculateAngleWithDirection()
	{
		return Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(viewFieldDirection, _agentController.direction) /
		                                  (viewFieldDirection.magnitude * _agentController.direction.magnitude));
	}
	
	//fixed update for collision detection
	public void FixedUpdate()
	{
		//updating viewfield rotation so the direction of the field fits the direction of the agent
		if (isAgentChild)
		{	
			if (_agentController.direction == new Vector2(1f, 0f))
			{
				transform.RotateAround(transform.parent.position, Vector3.forward, 0 - rotationTracker);
				rotationTracker = 0;
			}
			else if (_agentController.direction == new Vector2(0f, -1f))
			{
				transform.RotateAround(transform.parent.position, Vector3.forward, 270 - rotationTracker);
				rotationTracker = 270;
			}
			else if (_agentController.direction == new Vector2(-1f, 0))
			{
				transform.RotateAround(transform.parent.position, Vector3.forward, 180 - rotationTracker);
				rotationTracker = 180;
			}
			else if (_agentController.direction == new Vector2(0f, 1f))
			{
				transform.RotateAround(transform.parent.position, Vector3.forward, 90 - rotationTracker);
				rotationTracker = 90;
			}
		}
	}
}
