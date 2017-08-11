using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/*
    This class contains the algorithem that puts the different room to one level together
*/
public class ScreenCreatorScript : MonoBehaviour {
    public GameObject[] roomPrefabs;

    public int levelSize = 10;

    public int roomWidht = 32;
    public int roomHeight = 18;

    public string[] Themes = {"ice", "volcano", "forest", "volcano", "forest"};
    public string[] BorderThemes = {"beach", "ice"};
    private string borderTheme;
    private string innerTheme;

    private bool exitSet = false;
    private Dictionary<RoomPosition, GameObject> doneRooms = new Dictionary<RoomPosition, GameObject>();
    private List<GameObject> todoRooms = new List<GameObject>();

    private struct RoomPosition {
        public int x, y;

        public RoomPosition(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    void Start() {
        // while there is no ExitRoom redesign the Level
        while (exitSet == false) {
            foreach (var entry in doneRooms) {
                Destroy(entry.Value);
            }
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                Destroy(player);
            doneRooms = new Dictionary<RoomPosition, GameObject>();
            todoRooms = new List<GameObject>();
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            UnityEngine.Random.InitState(t.Seconds);
            borderTheme = BorderThemes[UnityEngine.Random.Range(0, BorderThemes.Length)];
            innerTheme = Themes[UnityEngine.Random.Range(0, Themes.Length)];

            //Select the Start Room
            var startRoom = FindRandomEntrance();
            startRoom.GetComponent<RoomBehaviour>().x = 0;
            startRoom.GetComponent<RoomBehaviour>().y = 0;

            todoRooms.Add(startRoom);
            doneRooms.Add(new RoomPosition(0, 0), startRoom);

            while (todoRooms.Count > 0) {
                var todoRoom = todoRooms.First();
                todoRoom.SetActive(false);
                var roomBehaviour = todoRoom.GetComponent<RoomBehaviour>();

                doRoom(todoRoom, roomBehaviour.x, roomBehaviour.y);
            }
            startRoom.SetActive(true);
            //if there is no Exit Room try to replace a existing room by a start room
            if (exitSet == false) {
                GameObject[] roomsWithExit = Array.FindAll(roomPrefabs, x => x.GetComponent<RoomProperties>().isExit);
                GameObject[] roomsWithoutEntrance = Array.FindAll(doneRooms.Values.ToArray(),
                    x => x.GetComponent<RoomProperties>().isEntrance == false);
                foreach (var exitRoom in roomsWithExit) {
                    foreach (var changeroom in roomsWithoutEntrance) {
                        if (exitRoom.GetComponent<RoomProperties>().GetExitDirections()
                            .Equals(changeroom.GetComponent<RoomProperties>().GetExitDirections())) {
                            var roomBehaviour = changeroom.GetComponent<RoomBehaviour>();
                            doneRooms[new RoomPosition(roomBehaviour.x, roomBehaviour.y)] = changeroom;
                            exitSet = true;
                        }
                    }
                }
            }
        }
        InstantiateRooms();
    }

    private GameObject doRoom(GameObject room, int x, int y) {
        RoomProperties roomProperties = room.GetComponent<RoomProperties>();
        RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();

        foreach (var exit in roomProperties.GetExitDirections()) {
            if (exit.Value) {
                Dictionary<Direction, bool> directions = new Dictionary<Direction, bool>(); //change
                var currentX = x;
                var currentY = y;
                if (exit.Key == Direction.NORTH)
                    currentY++;
                else if (exit.Key == Direction.SOUTH)
                    currentY--;
                else if (exit.Key == Direction.EAST)
                    currentX++;
                else if (exit.Key == Direction.WEST)
                    currentX--;

                if (IsRoom(currentX, currentY))
                    continue;

                if (IsRoom(currentX - 1, currentY)) {
                    if (HasExit(currentX - 1, currentY, Direction.EAST))
                        directions.Add(Direction.WEST, true);
                    else
                        directions.Add(Direction.WEST, false);
                }
                if (IsRoom(currentX + 1, currentY)) {
                    if (HasExit(currentX + 1, currentY, Direction.WEST))
                        directions.Add(Direction.EAST, true);
                    else directions.Add(Direction.EAST, false);
                }
                if (IsRoom(currentX, currentY - 1)) {
                    if (HasExit(currentX, currentY - 1, Direction.NORTH))
                        directions.Add(Direction.SOUTH, true);
                    else
                        directions.Add(Direction.SOUTH, false);
                }
                if (IsRoom(currentX, currentY + 1)) {
                    if (HasExit(currentX, currentY + 1, Direction.SOUTH))
                        directions.Add(Direction.NORTH, true);
                    else {
                        directions.Add(Direction.NORTH, false);
                    }
                }
                var connectingRoom = FindRandomRoomWithExitAt(directions, currentX, currentY);
                todoRooms.Add(connectingRoom);
                AddRoom(connectingRoom, currentX, currentY);
            }
        }

        todoRooms.Remove(room);
        return room;
    }

    private GameObject FindRandomEntrance() {
        //Get a List of all possible entrie rooms
        GameObject[] entrances = Array.FindAll(roomPrefabs, x => x.GetComponent<RoomProperties>().isEntrance);
        //Select a random room out of the entrance list
        return Instantiate<GameObject>(entrances[(int) UnityEngine.Random.Range(0, entrances.Length)]);
    }

    private GameObject FindRandomRoomWithExitAt(Dictionary<Direction, bool> directions, int x, int y) {
        //if there is no exit the new room can be an exit
        bool canBeExit = !exitSet;

        // return all possible Rooms at this point
        GameObject[] rooms = Array.FindAll(roomPrefabs, it => {
            RoomProperties roomProperties = it.GetComponent<RoomProperties>();
            bool isEntrance = roomProperties.isEntrance;
            //bool hasCorrectExit = directions.All(direction => roomProperties.GetExitDirections ()[direction]);
            bool hasCorrectExit = true;
            foreach (var entry in directions) {
                if (roomProperties.GetExitDirections().Contains(entry) == false) {
                    hasCorrectExit = false;
                }
            }
            bool isExit = roomProperties.isExit;


            return !isEntrance && hasCorrectExit && (canBeExit || !isExit);
        });

        //the rooms are sorted by number of exits
        GameObject[] sortedRooms = sortRoomsByNumeberofExits(rooms);

        int numberOfRolls = Math.Max(1, (Mathf.Abs(x) + Mathf.Abs(y)) / levelSize);
        int lowestRoomIndex = int.MaxValue;

        for (int i = 0; i < numberOfRolls; i++) {
            lowestRoomIndex = Math.Min(lowestRoomIndex, UnityEngine.Random.Range(0, rooms.Length));
        }

        if (doneRooms.Count > levelSize * 2)
            lowestRoomIndex = 0;

        if (sortedRooms[lowestRoomIndex].GetComponent<RoomProperties>().isExit) {
            exitSet = true;
        }

        var chosenRoomPrefab = sortedRooms[lowestRoomIndex];
        var chosenRoom = Instantiate(chosenRoomPrefab);
        return chosenRoom;
    }

    private GameObject[] sortRoomsByNumeberofExits(GameObject[] rooms) {
        //this method returns an array of all rooms sorted by the number of rooms
        Dictionary<GameObject, int> exitsPerRoom = new Dictionary<GameObject, int>();
        foreach (var room in rooms) {
            int exits = 0;
            foreach (var pair in room.GetComponent<RoomProperties>().GetExitDirections()) {
                if (pair.Value == true) {
                    exits++;
                }
            }
            exitsPerRoom.Add(room, exits);
        }
        return exitsPerRoom.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
    }

    private bool IsRoom(int x, int y) {
        //return if at this position is an existing room
        return GetRoom(x, y) != null;
    }

    private GameObject GetRoom(int x, int y) {
        GameObject ret;
        return doneRooms.TryGetValue(new RoomPosition(x, y), out ret) ? ret : null;
    }

    private bool HasExit(int x, int y, Direction direction) {
        if (!IsRoom(x, y))
            return false;

        var room = GetRoom(x, y);
        var roomProperties = room.GetComponent<RoomProperties>();

        return roomProperties.GetExitDirections()[direction];
    }

    private void AddRoom(GameObject room, int x, int y) {
        var roomBehaviour = room.GetComponent<RoomBehaviour>();
        roomBehaviour.x = x;
        roomBehaviour.y = y;
        doneRooms.Add(new RoomPosition(x, y), room);
    }

    private IEnumerable<GameObject> Row(int y) {
        return doneRooms.Where(it => it.Key.y == y).Select(it => it.Value);
    }

    private IEnumerable<GameObject> Column(int x) {
        return doneRooms.Where(it => it.Key.x == x).Select(it => it.Value);
    }

    private GameObject[] FindBorderRooms() {
        var minX = doneRooms.Keys.Min(it => it.x);
        var maxX = doneRooms.Keys.Max(it => it.x);

        var minY = doneRooms.Keys.Min(it => it.y);
        var maxY = doneRooms.Keys.Max(it => it.y);

        var borderRooms = new List<GameObject>();

        for (var x = minX; x <= maxX; x++) {
            var orderedColumn = Column(x).OrderBy(it => it.GetComponent<RoomBehaviour>().y);
            borderRooms.Add(orderedColumn.First());
            borderRooms.Add(orderedColumn.Last());
        }
        for (var y = minY; y <= maxY; y++) {
            var orderedColumn = Row(y).OrderBy(it => it.GetComponent<RoomBehaviour>().x);
            borderRooms.Add(orderedColumn.First());
            borderRooms.Add(orderedColumn.Last());
        }

        return borderRooms.ToArray();
    }

    private void InstantiateRooms() {
        //Add the Rooms to the Scene
        var borderRooms = FindBorderRooms();

        foreach (var pair in doneRooms) {
            var room = pair.Value;
            var position = pair.Key;
            var behaviour = room.GetComponent<RoomBehaviour>();
            var properties = room.GetComponent<RoomProperties>();

            behaviour.theme = innerTheme;
            if (properties.isExit)
                behaviour.theme = "beach";
            if (borderRooms.Contains(room))
                behaviour.theme = borderTheme;
            var tileThemers = room.GetComponentsInChildren<TileThemer>();
            foreach (var tileThemer in tileThemers) {
                tileThemer.SetThemedSprite(behaviour.theme);
            }

            int x = position.x;
            int y = position.y;
            room.transform.position = new Vector3(x * roomWidht, y * roomHeight, 0f);

            GameObject[] neigborRooms = new GameObject[4];
            if (IsRoom(x - 1, y))
                neigborRooms[0] = GetRoom(x - 1, y);
            if (IsRoom(x, y + 1))
                neigborRooms[1] = GetRoom(x, y + 1);
            if (IsRoom(x + 1, y))
                neigborRooms[2] = GetRoom(x + 1, y);
            if (IsRoom(x, y - 1))
                neigborRooms[3] = GetRoom(x, y - 1);
            properties.linkExitsWithRooms(neigborRooms);
        }
    }
}
