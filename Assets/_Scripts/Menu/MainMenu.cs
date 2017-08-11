using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {
    public EventController eventController;
    public AwarenessController awarenessController;

    public void SettingButton() {
        SceneManager.LoadScene(5);
    }

    public void PlayButton() {
        if (EventController.Instance != null)
            EventController.Instance.ResetController();
        SceneManager.LoadScene(1);
    }

    public void HelpButton() {
        SceneManager.LoadScene(4);
    }

    public void ExitButton() {
#if UNITY_EDITOR
        if (Application.isEditor)
            EditorApplication.isPlaying = false;
        else
#endif
        Application.Quit();
    }

    public void BackButton() {
	SceneManager.LoadScene(0);
    }

    void Start() {
        eventController = EventController.Instance;
        awarenessController = EventController.Instance.GetComponent<AwarenessController>();
    }

    void Update() {
    }
}
