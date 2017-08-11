using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatItemSpawnpoint : MonoBehaviour
{
    List<GameObject> Items = new List<GameObject>();
    void Start()
    {
        Items.Add(Resources.Load<GameObject>("Items/ItemSoundBubblePrefab"));
        Items.Add(Resources.Load<GameObject>("Items/ItemCloakPrefab"));
        Items.Add(Resources.Load<GameObject>("Items/ItemEMPPrefab"));
        Items.Add(Resources.Load<GameObject>("Items/ItemPotionPrefab"));
    }

    public GameObject GenerateItem(int index) {
        return Instantiate(Items[index]);
    }
}
