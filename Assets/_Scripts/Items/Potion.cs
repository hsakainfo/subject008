using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public override string GetName()
    {
        return "Potion";
    }
    
    public override Sprite GetSprite()
    {
        return Resources.Load("Items/ItemPotion", typeof(Sprite)) as Sprite;
    }
    
    public override string GetDescription()
    {
        return "This is a potion to let you move faster";
    }

    public override void UseItem(GameObject player)
    {
		player.AddComponent<PotionComponent>();
    }

}
