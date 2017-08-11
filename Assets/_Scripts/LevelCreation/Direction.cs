using System;

public enum Direction
{
	NORTH,
	EAST,
	SOUTH,
	WEST

}

public static class DirectionFunctions {

	public static Direction Opposite(this Direction thiz) {
		if (thiz == Direction.NORTH)
			return Direction.SOUTH;
		else if (thiz == Direction.SOUTH)
			return Direction.NORTH;
		else if (thiz == Direction.EAST)
			return Direction.WEST;
		else
			return Direction.EAST;
	}
}