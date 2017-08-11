using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
public enum ItemType
{
	Potion,
	EMP,
	InvisibilityCloak,
	SoundBubble
}


public class ItemController : MonoBehaviour
{
	private Collider2D col2d;
	public ItemType ItemType;
	private UIController _uiController;
	public SpriteRenderer SpriteRenderer;
	void Start ()
	{
		SpriteRenderer = GetComponent<SpriteRenderer>();
		col2d = GetComponent<Collider2D>();
		_uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other.GetComponent<CharacterControlScript>().ItemInInventory == null)
		{
			var item = AddItemToInventory(other.GetComponent<CharacterControlScript>());
			SpriteRenderer.sprite = item.GetSprite();	
			Destroy(gameObject);
		}
		else if (other.CompareTag("Player") && other.GetComponent<CharacterControlScript>().ItemInInventory != null)
		{
			SpriteRenderer.sprite = getNewItem().GetSprite();
		}
	}

	private Item getNewItem()
	{
		switch (ItemType)
		{
				case ItemType.Potion:
					return new Potion();
				case ItemType.EMP:
					return new EMP();
				case ItemType.InvisibilityCloak:
					return new InvisibilityCloak();
				case ItemType.SoundBubble:
					return  new SoundBubble();
				default:
					return null;
		}
	}

	public Item AddItemToInventory(CharacterControlScript ccs)
	{
		if(!_uiController) {
			_uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
		}
		Item newItem = getNewItem();
		ccs.ItemInInventory = newItem;
		ccs.SendMessageToHat();
		_uiController.Item.enabled = true;
		_uiController.Item.sprite = newItem.GetSprite();
		return newItem;
	}
}
