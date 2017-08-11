using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{

    public bool potenialEnemy = false;
    public float chanceEnemy = 0f;
    public bool potenialItem = false;
    public float chanceItem = 0f;

    public GameObject[] Items;
    public GameObject[] Enemies;

    private void createItemEnemy()
    {
        if (potenialEnemy)
        {
            if (Random.value < chanceEnemy)
            {
                //Instantiate Enemy as a child of the GameObject
                Instantiate(Enemies[Random.Range(0, Enemies.Length)], this.transform);
            }

        }
        else if (potenialItem)
        {
            if (Random.value < chanceItem)
            {
                //Instantiate Item at this position
                Instantiate(Items[Random.Range(0, Items.Length)], this.transform);
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        createItemEnemy();
    }
}
