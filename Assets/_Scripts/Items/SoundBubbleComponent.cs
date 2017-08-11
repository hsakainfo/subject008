using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBubbleComponent : ModifyDetectionComponent {
	public AudioSource AudioSource;
	public AudioClip bubbleStart;
	public AudioClip bubbleEnd;
	
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		//Load audio
		AudioSource = EventController.Instance.GetComponent<AudioSource>();
		bubbleStart = Resources.Load("Sounds/bubble.3") as AudioClip;
		bubbleEnd = Resources.Load("Sounds/bubble ende") as AudioClip;
		//Play bubble start
		AudioSource.PlayOneShot(bubbleStart, 1.0f);
	}
	// Called when component gets destroyed
	public override void OnDestroy()
	{
		AudioSource.PlayOneShot(bubbleEnd, 1.0f);
		base.OnDestroy();
	}
}
