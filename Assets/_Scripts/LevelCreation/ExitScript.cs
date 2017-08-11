using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UnityEngine;
using UnityStandardAssets._2D;

public class ExitScript : MonoBehaviour {
    public GameObject connectedRoom;

    public float widhtRoom;
    public float heightRoom;

    private void OnTriggerStay2D(Collider2D other) {
        //If the Player touchs and leaves the current room the Exit the Camera will change the position to the room connected to the exit
        if (other.tag.Equals("Player")) {
            var neighbourExitScripts = transform.parent.gameObject.GetComponentsInChildren<ExitScript>();
            var unusedRooms = new HashSet<GameObject>();
            foreach (var exitScript in neighbourExitScripts) {
                unusedRooms.Add(exitScript.connectedRoom);
            }
            EventController.Instance.GetComponent<AwarenessController>().DisableDetectorsNotIn(connectedRoom);
            var newRoom = connectedRoom;
            var newNeighbourExitScripts = newRoom.GetComponentsInChildren<ExitScript>();
            foreach (var neighbourExitScript in newNeighbourExitScripts) {
                if(unusedRooms.Contains(neighbourExitScript.connectedRoom))
                    unusedRooms.Remove(neighbourExitScript.connectedRoom);
                neighbourExitScript.connectedRoom.SetActive(true);
            }
            //disable non affected ones
            unusedRooms.Remove(newRoom);
            foreach(var room in unusedRooms) {
                room.SetActive(false);
            }
            newRoom.SetActive(true);
            var bounds = Camera.main.GetComponent<Camera2DFollow>().bounds;
            Camera.main.GetComponent<Camera2DFollow>().bounds = new Bounds(
                new Vector3(
                    connectedRoom.transform.position.x + 15.5f,
                    connectedRoom.transform.position.y + 8.5f,
                    Camera.main.transform.position.z
                ),
                bounds.size
            );
        }
    }
}
