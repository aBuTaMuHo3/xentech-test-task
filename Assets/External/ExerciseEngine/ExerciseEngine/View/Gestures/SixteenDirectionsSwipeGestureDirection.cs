using System;
using SynaptikonFramework.Util.Math;

namespace ExerciseEngine.View.Gestures
{
    public class SwipeGestureVO : ISwipeGesture
    {
        public Vector2D GesturePoint { get; }

        public SwipeGestureVO(GestureDirection gestureDirection, Vector2D gesturePoint)
        {
            GestureDirection = gestureDirection;
            GesturePoint = gesturePoint;
        }

        private GestureDirection GestureDirection { get; }

        public bool CheckDirection(GestureDirection gestureDirectionToCheck)
        {

            if(GestureDirection == GestureDirection.TAP){
                return gestureDirectionToCheck == GestureDirection;
            }else{
                return gestureDirectionToCheck.HasFlag(GestureDirection);
            }
        }
    }

    [Flags]
    public enum GestureDirection
    {
        TAP = 0,

        NORTH_BY_EAST = 1 << 0,
        NORTHEAST_BY_NORTH = 1 << 1,
        NORTHEAST_BY_EAST = 1 << 2,
        EAST_BY_NORTH = 1 << 3,

        EAST_BY_SOUTH = 1 << 4,
        SOUTHEAST_BY_EAST = 1 << 5,
        SOUTHEAST_BY_SOUTH = 1 << 6,
        SOUTH_BY_EAST = 1 << 7,

        SOUTH_BY_WEST = 1 << 8,
        SOUTHWEST_BY_SOUTH = 1 << 9,
        SOUTHWEST_BY_WEST = 1 << 10,
        WEST_BY_SOUTH = 1 << 11,

        WEST_BY_NORTH = 1 << 12,
        NORTHWEST_BY_WEST = 1 << 13,
        NORTHWEST_BY_NORTH = 1 << 14,
        NORTH_BY_WEST = 1 << 15,

        NORTH = NORTH_BY_EAST | NORTH_BY_WEST,
        SOUTH = SOUTH_BY_EAST | SOUTH_BY_WEST,
        EAST = EAST_BY_NORTH | EAST_BY_SOUTH,
        WEST = WEST_BY_NORTH | WEST_BY_SOUTH,

        NORTHEAST = NORTHEAST_BY_NORTH | NORTHEAST_BY_EAST,
        SOUTHEAST = SOUTHEAST_BY_EAST | SOUTHEAST_BY_SOUTH,
        NORTHWEST = NORTHWEST_BY_WEST | NORTHWEST_BY_NORTH,
        SOUTHWEST = SOUTHWEST_BY_WEST | SOUTHWEST_BY_SOUTH,

        DIAGONAL_ONLY = NORTHEAST | SOUTHEAST | NORTHWEST | SOUTHWEST,
        DIAGONAL_ORTHOGONAL = ORTHOGONAL | DIAGONAL_ONLY,

        HORIZONTAL = EAST | WEST,
        VERTICAL = NORTH | SOUTH,
        ORTHOGONAL = EAST | WEST | NORTH | SOUTH,

        FOUR_DIRECTION_NORTH = NORTHWEST_BY_NORTH | NORTH_BY_EAST | NORTH_BY_WEST | NORTHEAST_BY_NORTH,
        FOUR_DIRECTION_SOUTH = SOUTHEAST_BY_SOUTH | SOUTH_BY_EAST | SOUTH_BY_WEST | SOUTHWEST_BY_SOUTH,
        FOUR_DIRECTION_EAST = NORTHEAST_BY_EAST | EAST_BY_NORTH | EAST_BY_SOUTH | SOUTHEAST_BY_EAST,
        FOUR_DIRECTION_WEST = SOUTHWEST_BY_WEST | WEST_BY_NORTH | WEST_BY_SOUTH | NORTHWEST_BY_WEST
    }
}