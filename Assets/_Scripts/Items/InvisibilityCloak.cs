using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityCloak : Item
{
    public override string GetName()
    {
        return "Invisibilty Cloak";
    }

    public override Sprite GetSprite()
    {
        return Resources.Load("Items/ItemCloak", typeof(Sprite)) as Sprite;
    }
    
    public override string GetDescription()
    {
        return "This is a cloak that hides you from Camera Vision";
    }
    public override void UseItem(GameObject player)
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("Camera");
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        if (agents.Length == 0 && agents.Length == 0)
        {
            player.GetComponent<CharacterControlScript>().hasAnActiveItem = false;
            var uicontroller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            //Directly consume the item
            uicontroller.Item.sprite = null;
            uicontroller.Item.enabled = false;
        }
        else
        {
            foreach (GameObject camera in cameras)
            {
                //The Component is not added to the camera but rather to its child "view Radius"
                camera.GetComponentInChildren<ViewFieldController>().gameObject
                    .AddComponent<InvisibilityCloakComponent>();
            }
            foreach (var agent in agents)
            {
                agent.GetComponentInChildren<ViewFieldController>().gameObject
                    .AddComponent<InvisibilityCloakComponent>();
            }
        }

    }
}
