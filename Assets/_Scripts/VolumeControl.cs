using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VolumeControl : MonoBehaviour
{

	public Slider volumeSlider;
	public AudioSource volumeAudio;

	// Use this for initialization
	void Start()
	{
		//volumeSlider.onValueChanged.AddListener(delegate { VolumeSliderChanged(); });
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void VolumeSliderChanged()
	{
		AudioListener.volume = volumeSlider.value;
	}

}