using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour {

	private AudioSource source;

	private AudioClip backgroundMusic;

	private AudioClip startingMusic;
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!source)
		{
			source = EventController.Instance.GetComponent<AudioSource>();
			backgroundMusic = Resources.Load("Sounds/soundtrack") as AudioClip;
			startingMusic = Resources.Load("Sounds/soundtrackstart") as AudioClip;
			source.loop = true;
			source.clip = backgroundMusic;
			source.volume = 0.1f;
			source.PlayDelayed(1.47f);
			source.PlayOneShot(startingMusic, 0.05f);
		}
	}
}
