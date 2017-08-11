using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
    public int scene;

    private Scene currentScene;

    // Use this for initialization
    public void Start() {
        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadScene());
    }

    private AsyncOperation operation;
    
    IEnumerator LoadScene() {
        yield return null;
        
        operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.89f)
        {
            // Debug.Log(operation.progress);
            yield return null;
        }

        GameObject.FindWithTag("Splash Text").GetComponent<Text>().enabled = true;
    }

    private void FixedUpdate()
    {
        if (Input.anyKeyDown)
        {

            if (operation.progress > 0.8f)
            {
                operation.allowSceneActivation = true;
                GameObject.FindWithTag("Splash Text").GetComponent<Text>().text = "Starting ...";
            }
        }
    }

}