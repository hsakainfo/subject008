using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : Singleton<EventController> {
    public int island = 0;

    protected EventController() {}
    void Awake() {
        gameObject.AddComponent<AwarenessController>();
        gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<TimeController>();
        gameObject.AddComponent<BackgroundMusicController>();
        gameObject.AddComponent<CheatItemSpawnpoint>();
    }
    public void ResetController() {
        GetComponent<AwarenessController>().ResetController();
        GetComponent<TimeController>().ResetController();
        island = 0;
    }

    public void Death() {
        GetComponent<TimeController>().StopTime();
        // Load menu on death
        SceneManager.LoadScene(3);
    }
}
