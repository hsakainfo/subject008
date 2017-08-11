using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP : Item
{
	
	
	public override string GetName()
	{
		return "EMP";
	}

	public override string GetDescription()
	{
		return "This is an item to disable all Cameras and Microphones";
	}

	public override Sprite GetSprite()
	{
		return Resources.Load("Items/ItemEMP", typeof(Sprite)) as Sprite;
	}

	public override void UseItem (GameObject player)
	{
		GameObject[] Cameras;
		GameObject[] Microphones;
		Cameras = GameObject.FindGameObjectsWithTag ("Camera");
		Microphones = GameObject.FindGameObjectsWithTag ("Microphone");
		if (Cameras.Length == 0 && Microphones.Length == 0)
		{
			player.GetComponent<CharacterControlScript>().hasAnActiveItem = false;
			//Load the ui controller
			var uicontroller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
			//Directly consume the item
			uicontroller.Item.sprite = null;
			uicontroller.Item.enabled = false;
		}
		else
		{
			foreach (GameObject Camera in Cameras)
			{
				Camera.GetComponentInChildren<ViewFieldController>().gameObject.AddComponent<EmpComponent>();
			}
			foreach (GameObject Microphone in Microphones)
			{
				Microphone.GetComponentInChildren<MicrophoneController>().gameObject.AddComponent<EmpComponent>();
			}
		}
	}
}

