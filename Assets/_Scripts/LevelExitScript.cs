using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitScript : MonoBehaviour
{

    public GameObject LevelCounter;

    private AudioSource source;
    private AudioClip exitSound;



    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag.Equals("Player"))
        {
            source = EventController.Instance.GetComponent<AudioSource>();
            exitSound = Resources.Load("Sounds/Floß2") as AudioClip;
            source.PlayOneShot(exitSound, 1.0f);
            LevelCounter.GetComponent<LevelCounterScript>().NewLevel();

        }
    }
}
