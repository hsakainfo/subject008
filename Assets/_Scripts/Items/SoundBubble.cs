using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SoundBubble : Item
{
    public override string GetName()
    {
        return "Sound Bubble";
    }

    public override string GetDescription()
    {
        return "This Sound Bubble conceals your Movement from audio Detection";
    }
    
    public override Sprite GetSprite()
    {
        return Resources.Load("Items/ItemBubble", typeof(Sprite)) as Sprite;
    }
    
    public override void UseItem(GameObject player)
    {
        GameObject[] microphones = GameObject.FindGameObjectsWithTag("Microphone");
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        if (agents.Length == 0 && microphones.Length == 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControlScript>().hasAnActiveItem = false;
            var item = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>().Item;
            item.sprite = null;
            item.enabled = false;
        }
        else
        {
            foreach (var microphone in microphones)
            {
                microphone.GetComponentInChildren<MicrophoneController>().gameObject
                    .AddComponent<SoundBubbleComponent>();
            }
            foreach (var agent in agents)
            {
                agent.GetComponentInChildren<MicrophoneController>().gameObject.AddComponent<SoundBubbleComponent>();
            }
        }
    }

}
