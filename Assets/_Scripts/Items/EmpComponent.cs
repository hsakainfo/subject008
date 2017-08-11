using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpComponent : ModifyDetectionComponent {
	public AudioSource AudioSource;
	public AudioClip empStart;
	
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		AudioSource = EventController.Instance.GetComponent<AudioSource>();
		empStart = Resources.Load("Sounds/EMP+BUMMMM") as AudioClip;
		AudioSource.PlayOneShot(empStart, 1.0f);
	}


}
