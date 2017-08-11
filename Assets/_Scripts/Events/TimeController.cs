using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
	public float OverallTime;
	public float TimeAddedPerScreen = 15;
	public int LevelsCompleted;

	private bool running = false;

	private EventController eventController;

	// Use this for initialization
	void Start()
	{
		OverallTime = 0f;
		DontDestroyOnLoad(transform.gameObject);
	}

	public void StartTime()
	{
		running = true;
	}

    public void StopTime()
	{
		running = false;
	}

	private void FixedUpdate()
	{
		if (running)
		{
			OverallTime = OverallTime + Time.deltaTime;
		}
	}

	public void AddTime()
	{
		OverallTime = OverallTime + 15;
	}

	public void ResetController() {
		StopTime();
		OverallTime = 0;
		LevelsCompleted = 0;
	}
}
