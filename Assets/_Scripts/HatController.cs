using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : MonoBehaviour
{
	private SpriteRenderer[] renderer;
	private List<SpriteRenderer> HatDash = new List<SpriteRenderer>();
	private List<SpriteRenderer> HatEmp = new List<SpriteRenderer>();
	private List<SpriteRenderer> HatBubble = new List<SpriteRenderer>();
	private List<SpriteRenderer> HatCloak = new List<SpriteRenderer>();
	private List<SpriteRenderer> oldItem = new List<SpriteRenderer>();
	
	private bool initialised = false;
	
	
	
	// Update is called once per frame
	void Update () {
		if (!initialised)
		{
			LateStart();
			initialised = true;
		}
	}

	void ChangeItem(String item)
	{
		foreach (var rend in oldItem)
		{
			rend.enabled = false;
		}
		switch (item)
		{
			case "Potion":
				foreach (var rend in HatDash)
				{
					rend.enabled = true;
					
				}
				oldItem = HatDash;
				break;
			case "EMP":
				foreach (var rend in HatEmp)
				{
					rend.enabled = true;
				}
				oldItem = HatEmp;
				break;
			case "Sound Bubble":
				foreach (var rend in HatBubble)
				{
					rend.enabled = true;
				}
				oldItem = HatBubble;
				break;
			case "Invisibilty Cloak":
				foreach (var rend in HatCloak)
				{
					rend.enabled = true;
				}
				oldItem = HatCloak;
				break;
			
		}
		
				
	}

	void LateStart()
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(true);
		}
		renderer = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer rend in renderer)
		{

			switch (rend.tag)
			{
					case "emp":
						HatEmp.Add(rend);
						rend.enabled = false;
						break;
					case "dash":
						HatDash.Add(rend);
						rend.enabled = false;
						break;
					case "bubble":
						HatBubble.Add(rend);
						rend.enabled = false;
						break;
					case "cloak":
						HatCloak.Add(rend);
						rend.enabled = false;
						break;
					case "none":
						break;
					default:
						// Debug.Log("tag not known");
						break;
			}
		}
	}
}
