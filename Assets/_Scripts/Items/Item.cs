using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item
{
   public abstract string GetName();
   public abstract string GetDescription();
   public abstract void UseItem(GameObject player);
   public abstract Sprite GetSprite();
}
