using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour {
	public bool northExit;
	public bool eastExit;
	public bool southExit;
	public bool westExit;

	public bool isEntrance;
	public bool isExit;

	public GameObject westExitTrigger;
	public GameObject northExitTrigger;
	public GameObject eastExitTrigger;
	public GameObject southExitTrigger;

	public Dictionary<Direction, bool> GetExitDirections() {
		Dictionary<Direction, bool> ret = new Dictionary<Direction, bool> ();
		ret.Add (Direction.NORTH, northExit);
		ret.Add (Direction.EAST, eastExit);
		ret.Add (Direction.SOUTH, southExit);
		ret.Add (Direction.WEST, westExit);
		return ret;
	}

	public void linkExitsWithRooms(GameObject[] neighborRooms) {

		//0 = west 1 = north 2 = east 3 = south
		if(westExit) {
			westExitTrigger.GetComponent<ExitScript>().connectedRoom = neighborRooms[0];
		}
		if(eastExit) {
			eastExitTrigger.GetComponent<ExitScript>().connectedRoom = neighborRooms[2];
		}
		if(northExit) {
			northExitTrigger.GetComponent<ExitScript>().connectedRoom = neighborRooms[1];
		}
		if(southExit) {
			southExitTrigger.GetComponent<ExitScript>().connectedRoom = neighborRooms[3];
		}
	}


	}

