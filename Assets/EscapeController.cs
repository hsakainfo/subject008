using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void FixedUpdate()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene(0);
		
	}
}
