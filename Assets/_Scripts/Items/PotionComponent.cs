using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PotionComponent : ItemBaseComponent {
	public uint dashes = 0;
	public int uses = 3;
	public AudioSource AudioSource;
	public AudioClip soundDrink;
	private CharacterControlScript _ccs;
	// Use this for initialization
	public override void Start () {
		base.Start();
		//Disable cooldown for potion
		uiController.CoolDownActivated = false;
		//Load sounds
		AudioSource = EventController.Instance.GetComponent<AudioSource>();
	    soundDrink = Resources.Load("schlucken") as AudioClip;
		AudioSource.PlayOneShot(soundDrink, 1.0f);
		//We have a player here
		_ccs = gameObject.GetComponent<CharacterControlScript>();
		_ccs.dashLengthInSeconds *= 0.5f;
		dashes = _ccs.DashCounter;
	}
	
	// Update is called once per frame
	public override void Update() {
		if (_ccs.DashCounter >= dashes + uses)
			Destroy (this);
	}

	// Called when component gets destroyed
	public override void OnDestroy()
	{
		uiController.CoolDownTimer.fillAmount = 0;
		_ccs.dashLengthInSeconds *= 2;
		base.OnDestroy();

	}
}