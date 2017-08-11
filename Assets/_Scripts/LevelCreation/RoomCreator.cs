using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

static class StringClone {
    public static string RemoveClone(this string thiz) {
        return thiz.Replace("(Clone)", "");
    }
}

[ExecuteInEditMode]
public class RoomCreator : MonoBehaviour {
    public GameObject BottomLeftCorner;
    public GameObject BottomRightCorner;
    public GameObject TopRightCorner;
    public GameObject TopLeftCorner;

    public GameObject BottomLeftCornerInner;
    public GameObject BottomRightCornerInner;
    public GameObject TopRightCornerInner;
    public GameObject TopLeftCornerInner;


    public GameObject Floor;

    public GameObject TopWall;
    public GameObject BottomWall;
    public GameObject RightWall;
    public GameObject LeftWall;

    public GameObject Tiles;

    public GameObject RoomPrefab;

    public GameObject ExitTrigger;

    public bool ShouldGenerate = false;

    // Use this for initialization
    void Start() {
        if (!ShouldGenerate)
            return;

        try {
            var directions = AllDirections();
            var counter = 0;

            foreach (var exits in directions) {
                var room = Instantiate(RoomPrefab, new Vector3(counter * 32, 0, 0), Quaternion.identity);
                room.name = room.name.RemoveClone() + " " +
                            String.Join(", ", exits.Select(x => x.ToString()).ToArray());
                var roomProperties = room.GetComponent<RoomProperties>();
                foreach (var exit in exits) {
                    switch (exit) {
                        case Direction.NORTH:
                            roomProperties.northExit = true;
                            roomProperties.northExitTrigger = Instantiate(ExitTrigger, room.transform);
                            roomProperties.northExitTrigger.transform.localPosition = new Vector3(15.5f, 20f, 0);
                            roomProperties.northExitTrigger.name = roomProperties.northExitTrigger.name.RemoveClone() +
                                                                   " NORTH";
                            roomProperties.northExitTrigger.GetComponent<BoxCollider2D>().size = new Vector2(5, 5);
                            break;
                        case Direction.WEST:
                            roomProperties.westExit = true;
                            roomProperties.westExitTrigger = Instantiate(ExitTrigger, room.transform);
                            roomProperties.westExitTrigger.transform.localPosition = new Vector3(-3f, 8.5f, 0);
                            roomProperties.westExitTrigger.name = roomProperties.westExitTrigger.name.RemoveClone() +
                                                                  " WEST";
                            roomProperties.westExitTrigger.GetComponent<BoxCollider2D>().size = new Vector2(5, 5);
                            break;
                        case Direction.SOUTH:
                            roomProperties.southExit = true;
                            roomProperties.southExitTrigger = Instantiate(ExitTrigger, room.transform);
                            roomProperties.southExitTrigger.transform.localPosition = new Vector3(15.5f, -3f, 0);
                            roomProperties.southExitTrigger.name = roomProperties.southExitTrigger.name.RemoveClone() +
                                                                   " SOUTH";
                            roomProperties.southExitTrigger.GetComponent<BoxCollider2D>().size = new Vector2(5, 5);
                            break;
                        case Direction.EAST:
                            roomProperties.eastExit = true;
                            roomProperties.eastExitTrigger = Instantiate(ExitTrigger, room.transform);
                            roomProperties.eastExitTrigger.transform.localPosition = new Vector3(34f, 8.5f, 0);
                            roomProperties.eastExitTrigger.name = roomProperties.eastExitTrigger.name.RemoveClone() +
                                                                  " EAST";
                            roomProperties.eastExitTrigger.GetComponent<BoxCollider2D>().size = new Vector2(5, 5);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                var tiles = Instantiate(Tiles, room.transform);
                tiles.transform.localPosition = new Vector3(0, 0, 0);
                tiles.name = tiles.name.RemoveClone();

                for (var x = 1; x < 31; x++) {
                    for (var y = 1; y < 17; y++) {
                        var floor = Instantiate(Floor, tiles.transform);
                        floor.transform.localPosition = new Vector3(x, y, 0);
                        floor.name = floor.name.RemoveClone() + " (" + x + "/" + y + "}";
                    }
                }

                var topLeft = Instantiate(TopLeftCorner, tiles.transform);
                topLeft.transform.localPosition = new Vector3(0, 17, 0);
                topLeft.name = topLeft.name.RemoveClone() + " (0/17)";
                var topRight = Instantiate(TopRightCorner, tiles.transform);
                topRight.transform.localPosition = new Vector3(31, 17, 0);
                topRight.name = topRight.name.RemoveClone() + " (31/17)";
                var bottomRight = Instantiate(BottomRightCorner, tiles.transform);
                bottomRight.transform.localPosition = new Vector3(31, 0, 0);
                bottomRight.name = bottomRight.name.RemoveClone() + " (31/0)";
                var bottomLeft = Instantiate(BottomLeftCorner, tiles.transform);
                bottomLeft.transform.localPosition = new Vector3(0, 0, 0);
                bottomLeft.name = bottomLeft.name.RemoveClone() + " (0/0)";

                for (var x = 1; x < 31; x++) {
                    GameObject tileNorth = null;
                    GameObject tileSouth = null;

                    if (exits.Contains(Direction.NORTH)) {
                        if (x == 15 || x == 16)
                            tileNorth = Instantiate(Floor, tiles.transform);

                        if (x == 14)
                            tileNorth = Instantiate(BottomRightCornerInner, tiles.transform);

                        if (x == 17)
                            tileNorth = Instantiate(BottomLeftCornerInner, tiles.transform);
                    }

                    if (exits.Contains(Direction.SOUTH)) {
                        if (x == 15 || x == 16)
                            tileSouth = Instantiate(Floor, tiles.transform);

                        if (x == 14)
                            tileSouth = Instantiate(TopRightCornerInner, tiles.transform);

                        if (x == 17)
                            tileSouth = Instantiate(TopLeftCornerInner, tiles.transform);
                    }

                    if (tileNorth == null)
                        tileNorth = Instantiate(TopWall, tiles.transform);

                    if (tileSouth == null)
                        tileSouth = Instantiate(BottomWall, tiles.transform);

                    tileNorth.transform.localPosition = new Vector3(x, 17, 0);
                    tileNorth.name = tileNorth.name.RemoveClone() + " (" + x + "/17)";

                    tileSouth.transform.localPosition = new Vector3(x, 0, 0);
                    tileSouth.name = tileSouth.name.RemoveClone() + " (" + x + "/0)";
                }

                for (var y = 1; y < 17; y++) {
                    GameObject tileEast = null;
                    GameObject tileWest = null;

                    if (exits.Contains(Direction.EAST)) {
                        if (y == 8 || y == 9)
                            tileEast = Instantiate(Floor, tiles.transform);

                        if (y == 7)
                            tileEast = Instantiate(TopLeftCornerInner, tiles.transform);

                        if (y == 10)
                            tileEast = Instantiate(BottomLeftCornerInner, tiles.transform);
                    }

                    if (exits.Contains(Direction.WEST)) {
                        if (y == 8 || y == 9)
                            tileWest = Instantiate(Floor, tiles.transform);

                        if (y == 7)
                            tileWest = Instantiate(TopRightCornerInner, tiles.transform);

                        if (y == 10)
                            tileWest = Instantiate(BottomRightCornerInner, tiles.transform);
                    }

                    if (tileEast == null)
                        tileEast = Instantiate(RightWall, tiles.transform);
                    if (tileWest == null)
                        tileWest = Instantiate(LeftWall, tiles.transform);

                    tileEast.transform.localPosition = new Vector3(31, y, 0);
                    tileEast.name = tileEast.name.RemoveClone() + " (31/" + y + ")";

                    tileWest.transform.localPosition = new Vector3(0, y, 0);
                    tileWest.name = tileWest.name.RemoveClone() + " (0/" + y + ")";
                }

                counter++;
            }
        }
        finally {
            ShouldGenerate = false;
        }
    }

    void Update() {
        Start();
    }

    private List<List<Direction>> AllDirections() {
        var directions = new[] {
            Direction.NORTH, Direction.EAST, Direction.WEST, Direction.SOUTH
        };

        int n = directions.Length;
        // Power set contains 2^N subsets.
        int powerSetCount = 1 << n;
        var ans = new List<List<Direction>>();

        for (int setMask = 0; setMask < powerSetCount; setMask++) {
            var currentDirections = new List<Direction>();
            for (int i = 0; i < n; i++) {
                // Checking whether i'th element of input collection should go to the current subset.
                if ((setMask & (1 << i)) > 0) {
                    currentDirections.Add(directions[i]);
                }
            }
            ans.Add(currentDirections);
        }

        return ans;
    }
}
