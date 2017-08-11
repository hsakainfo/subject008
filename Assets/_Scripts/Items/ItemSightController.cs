using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSightAController : MonoBehaviour {
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			//gameObject.GetComponentInParent<SpriteRenderer>().sprite = this.gameObject.GetComponentInParent<ItemController>().getNewItem().GetSprite();
		}
	}
}
