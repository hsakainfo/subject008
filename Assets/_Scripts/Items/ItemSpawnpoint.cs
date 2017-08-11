using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnpoint : MonoBehaviour
{
    public float chanceItem = 50f;
    private GameObject DecoyItem;
    List<GameObject> Items = new List<GameObject>();
    void Start()
    {
        DecoyItem = (Resources.Load<GameObject>("Items/DecoyItem"));
        Items.Add(Resources.Load<GameObject>("Items/ItemSoundBubblePrefab"));
        Items.Add(Resources.Load<GameObject>("Items/ItemCloakPrefab"));
        Items.Add(Resources.Load<GameObject>("Items/ItemEMPPrefab"));
        Items.Add(Resources.Load<GameObject>("Items/ItemPotionPrefab"));
        createItems();
    }

    private void createItems()
    {
        if ((Random.value * 100) <= chanceItem)
        {
            //Instantiate Item at this position
            Instantiate(Items[Random.Range(0, Items.Count)], this.transform);
        }
        else
        {
            Instantiate(DecoyItem, this.transform);
        }
    }

    public GameObject GenerateItem(int index) {
        return Instantiate(Items[index]);
    }
}
