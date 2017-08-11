using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Lifecount : MonoBehaviour
{
    public EventController eventController;
    public AwarenessController awarenessController;
    public Image heart2;
    public Image heart1;
    public Image heart3;


    // Use this for initialization
    void Start()
    {
        eventController = EventController.Instance.GetComponent<EventController>();
        awarenessController = EventController.Instance.GetComponent<AwarenessController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (awarenessController.lifecount < 3)
            heart3.enabled = false;
        if (awarenessController.lifecount < 2)
            heart2.enabled = false;
        if (awarenessController.lifecount < 1)
            heart1.enabled = false;
    }
}