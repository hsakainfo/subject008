using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelCounterScript : MonoBehaviour {

    private int levelNumber;
    public int changePerLevel = 2;
    public GameObject SceneLoaderPrefab;
    private TimeController _timeController;

    public static LevelCounterScript instance;

    void Start()
    {
        _timeController = EventController.Instance.GetComponent<TimeController>();
    }

    public void Awake() {
        //Make sure that there is only one instance at a time
        if (!instance) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    private bool initialized = false;

    private void FixedUpdate()
    {
        if (!initialized)
        {
            initialized = true;
            levelNumber = EventController.Instance.island;
        }
    }

    public void NewLevel() {
        //if the goes to the next level a new level is loaded
        levelNumber++;
        EventController.Instance.island = levelNumber + 1;
        SceneManager.LoadScene(1);
    }

    public int GetLevelSize() {
        // important for difficulty of the new Level
        return levelNumber * changePerLevel + 10;
    }
}
