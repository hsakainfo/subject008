using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessController : Singleton<AwarenessController>
{
    public float deltaTime = 0.0f;
    private float timeOfAlarm = 0.2f;
    public float previousDetectionLevel = 0f;
    public float detectionLevel = 0f;
    public float maxDetectionUntilAlarm = 100;
    public bool increasingAwareness;
    public int lifecount = 1;
    public float scalePerLevel = 0.5f;
    public float DetectionDecay = 0.35f;
    public HashSet<DetectionController> ActiveDetectors = new HashSet<DetectionController>();
    private EventController eventController;

    private AudioSource source;
    private AudioClip detected1;
    private AudioClip detected2;
    private AudioClip alarm;
    private AudioClip gameOver;
    
    protected AwarenessController() {}
    void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            return;
        }
        if (!source)
        {
            //use player here instead of eventcontroller because the eventcontroller is busy playing the soundtrack on loop;
            source = player.GetComponent<AudioSource>();
            detected1 = Resources.Load("Sounds/agent1.6") as AudioClip;
            detected2 = Resources.Load("Sounds/agent2_2.3") as AudioClip;
            gameOver = Resources.Load("Sounds/game over") as AudioClip;
            alarm = Resources.Load("Sounds/alarm") as AudioClip;
        }
        if (!eventController)
        {
            eventController = EventController.Instance;
        }
        if (deltaTime >= timeOfAlarm)
        {
            if (previousDetectionLevel < detectionLevel)
            {
                if (!source.isPlaying)
                {
                    source.clip = alarm;
                    source.Play();
                }
            }
            previousDetectionLevel = detectionLevel;
            deltaTime = 0;
        }
        deltaTime += Time.deltaTime;
        UpdateDetectionLevel();
    }



    private void FixedUpdate()
    {

        if (maxDetectionUntilAlarm <= detectionLevel)
        {
            int randomClip = Random.Range(0, 1);
            switch (randomClip)
            {
                case 0:
                    source.PlayOneShot(detected1, 1.0f);
                    break;
                case 1:
                    source.PlayOneShot(detected2, 1.0f);
                    break;
            }

            this.lifecount -= 1;
            detectionLevel = 0;

            if (lifecount == 0)
            {
                source.PlayOneShot(gameOver, 1.0f);
                eventController.Death();
            }

        }

        if (detectionLevel > 0)
        {
            detectionLevel -= DetectionDecay;

            if (detectionLevel < 0)
            {
                detectionLevel = 0;
            }
        }
    }

    private void UpdateDetectionLevel() {
        var levelScaling = 1f + (scalePerLevel * Mathf.Max(0,(EventController.Instance.island-1)));
        foreach(var dc in ActiveDetectors) {
            if(dc.isAbleToRecord) {
                detectionLevel += dc.increaseAmount * Time.deltaTime * levelScaling;
            }
        }
    }

    public void DisableDetectorsNotIn(GameObject room) {
        //ActiveDetectors.Clear();
        //TODO: Insert code if ExitScript Trigger bug is fixed (ask Mischa or Daniel)
        ActiveDetectors.RemoveWhere(x => !x.gameObject.transform.IsChildOf(room.transform));
    }

    public void ResetController() {
        previousDetectionLevel = 0;
        detectionLevel = 0;
        increasingAwareness = false;
        lifecount = 1;
        ActiveDetectors.Clear();
    }
}
