using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour {
	public string theme;
	public Dictionary<Direction,GameObject> connectedRooms = new Dictionary<Direction,GameObject>();
	public int x;
	public int y;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
