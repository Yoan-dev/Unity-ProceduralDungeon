using UnityEngine;
using System.Collections;

public class Values {

    // Tiles units for isometric transformation
    private static float widthUnit = 1.275f;
    private static float heightUnit = 0.64f;

    // RoomGeneration
    private static int minimumRoomSize = 10;
    private static int maximumRoomSize = 15;

    // Corridors
    private static int minimumCorridorWidth = 2;
    private static int maximumCorridorWidth = 3;

    // Distances between rooms/corridors
    private static int distanceBetweenRooms = 5;
    private static int distanceAdditionalCorridors = 15;

    #region Values;

    public static int MinimumRoomSize
    {
        get
        {
            return minimumRoomSize;
        }

        set
        {
            minimumRoomSize = value;
        }
    }

    public static int MaximumRoomSize
    {
        get
        {
            return maximumRoomSize;
        }

        set
        {
            maximumRoomSize = value;
        }
    }

    public static float WidthUnit
    {
        get
        {
            return widthUnit;
        }

        set
        {
            widthUnit = value;
        }
    }

    public static float HeightUnit
    {
        get
        {
            return heightUnit;
        }

        set
        {
            heightUnit = value;
        }
    }

    public static int MinimumCorridorWidth
    {
        get
        {
            return minimumCorridorWidth;
        }

        set
        {
            minimumCorridorWidth = value;
        }
    }

    public static int MaximumCorridorWidth
    {
        get
        {
            return maximumCorridorWidth;
        }

        set
        {
            maximumCorridorWidth = value;
        }
    }

    public static int DistanceBetweenRooms
    {
        get
        {
            return distanceBetweenRooms;
        }

        set
        {
            distanceBetweenRooms = value;
        }
    }

    public static int DistanceAdditionalCorridors
    {
        get
        {
            return distanceAdditionalCorridors;
        }

        set
        {
            distanceAdditionalCorridors = value;
        }
    }

    #endregion Values;
}
