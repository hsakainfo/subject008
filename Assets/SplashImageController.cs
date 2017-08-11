using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashImageController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		var island = EventController.Instance.island;
		var images = GameObject.FindGameObjectsWithTag("Splash Image");
		foreach (var image in images)
		{
			var sprite = image.GetComponent<Image>();
			
			sprite.enabled = image.name == "before " + island;
		}
		
	}
}
