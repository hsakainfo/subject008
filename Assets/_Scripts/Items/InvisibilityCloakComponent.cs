using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class InvisibilityCloakComponent : ModifyDetectionComponent {
	public AudioSource AudioSource;
	public AudioClip cloakStart;
	public AudioClip cloakEnd;
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		AudioSource = EventController.Instance.GetComponent<AudioSource>();
		cloakStart = Resources.Load("Sounds/invisible_1") as AudioClip;
		cloakEnd = Resources.Load("Sounds/invisible Ende") as AudioClip;
		
		AudioSource.PlayOneShot(cloakStart, 1.0f);
	}
	

	// Called when component gets destroyed
	public override void OnDestroy()
	{
		AudioSource.PlayOneShot(cloakStart, 1.0f);
		base.OnDestroy();
	}
}
