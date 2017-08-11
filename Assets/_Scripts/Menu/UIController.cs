using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	[HideInInspector]public Text TextComponent;
	[HideInInspector] public Image CoolDownTimer;
	[HideInInspector] public Image Item;

	[HideInInspector] public Image DashLevelOne;
	[HideInInspector] public Image DashLevelTwo;
	[HideInInspector] public Image DashLevelThree;

	public float ItemCoolDownOpacity = 0.5f;
	public float theTime;
	public float CoolDownTime;
	public bool CoolDownActivated;

	private TimeController timeController;
	// Use this for initialization
	void Start ()
	{
		CoolDownTimer = GameObject.Find("CoolDownTimer").GetComponent<Image>();
		timeController = EventController.Instance.GetComponent<TimeController>();
		TextComponent = GameObject.Find("TimeText").GetComponent<Text>();
		Item = GameObject.Find("Items").GetComponent<Image>();

		//Finds the Dases
		DashLevelOne = GameObject.Find("DashesLevelOne").GetComponent<Image>();
		DashLevelTwo = GameObject.Find("DashesLevelTwo").GetComponent<Image>();
		DashLevelThree = GameObject.Find("DashesLevelThree").GetComponent<Image>();

		//This Code Sets the Origin of the Circular Animation to the top and fills it to zero
		CoolDownTimer.type = Image.Type.Filled;
		CoolDownTimer.fillMethod = Image.FillMethod.Radial360;
		CoolDownTimer.fillClockwise = false;
		CoolDownTimer.fillOrigin = 10;
		CoolDownTimer.fillAmount = 0;
		//Sets the Opacity to fithy percent
		Color tempColour = CoolDownTimer.color;
		tempColour.a = ItemCoolDownOpacity;
		CoolDownTimer.color = tempColour;

		Item.enabled = false;

//		DashLevelOne.enabled = false;
//		DashLevelTwo.enabled = false;
//		DashLevelThree.enabled = false;
		//Item.enabled = false;

		DashLevelOne.fillAmount = 0f;
		DashLevelTwo.fillAmount = 0f;
		DashLevelThree.fillAmount = 0f;
	}

	// Update is called once per frame
	void Update ()
	{
		//Displays the remaining time in the top right corner of the screen
		theTime = timeController.OverallTime;
		TextComponent.text = theTime.ToString("0.00");

		//DIsplays an CoolDownTimer on the current Ability
		if (CoolDownActivated)
		{
			CoolDownTimer.fillAmount -= 1.0f / CoolDownTime * Time.deltaTime;
			if (CoolDownTimer.fillAmount <= 0)
			{
				CoolDownTime = 0;
				CoolDownActivated = false;
			}
		}

	}

	public void ActivateCooldown()
	{
		CoolDownTimer.fillAmount = 1;
	}

	public void DeactivateCooldown()
	{
		CoolDownTimer.fillAmount = 0;
	}
}
