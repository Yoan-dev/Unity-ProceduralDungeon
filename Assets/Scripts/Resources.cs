using UnityEngine;
using System.Collections;
using System;

public class Resources {

    // North, South, East, West
    public enum Direction { North, South, East, West };

    // Tiles
    public enum Tiles { Floor, WallTop, WallBottom };

    // Isometric
    private static float widthUnit = 1.275f;
    private static float heightUnit = 0.64f;

    // Random
    private static bool useSeed = false;
    private static string seed;
    private static System.Random rand;

    // Map size
    private static int minWidth = 10;
    private static int maxWidth = 10;
    private static int minHeight = 10;
    private static int maxHeight = 10;

    // Map limits
    private static bool northAccess = true;
    private static bool southAccess = false;
    private static bool eastAccess = true;
    private static bool westAccess = true;

    // Nodes size
    private static int nodeSize = 20;
    private static int nodeGap = 3;
    private static int corridorWidth = 2;

    // Nodes count
    private static int minNodes = 15;
    private static int maxNodes = 20;

    // Additional path count
    private static int minAdditionalPath = 2;
    private static int maxAdditionalPath = 3;

    // Generation metrics
    private static int northFactor = 40;
    private static int southFactor = 20;
    private static int eastFactor = 25;
    private static int westFactor = 25;
    private static int ramificationChance = 50;
    private static int ramificationChanceInc = 20;

    #region Accessors;

    public static System.Random Rand
    {
        get
        {
            return rand;
        }

        set
        {
            rand = value;
        }
    }

    public static bool NorthAccess
    {
        get
        {
            return northAccess;
        }

        set
        {
            northAccess = value;
        }
    }

    public static bool SouthAccess
    {
        get
        {
            return southAccess;
        }

        set
        {
            southAccess = value;
        }
    }

    public static bool EastAccess
    {
        get
        {
            return eastAccess;
        }

        set
        {
            eastAccess = value;
        }
    }

    public static bool WestAccess
    {
        get
        {
            return westAccess;
        }

        set
        {
            westAccess = value;
        }
    }

    public static int MinWidth
    {
        get
        {
            return minWidth;
        }

        set
        {
            minWidth = value;
        }
    }

    public static int MaxWidth
    {
        get
        {
            return maxWidth;
        }

        set
        {
            maxWidth = value;
        }
    }

    public static int MinHeight
    {
        get
        {
            return minHeight;
        }

        set
        {
            minHeight = value;
        }
    }

    public static int MaxHeight
    {
        get
        {
            return maxHeight;
        }

        set
        {
            maxHeight = value;
        }
    }

    public static int MinNodes
    {
        get
        {
            return minNodes;
        }

        set
        {
            minNodes = value;
        }
    }

    public static int MaxNodes
    {
        get
        {
            return maxNodes;
        }

        set
        {
            maxNodes = value;
        }
    }

    public static int RamificationChance
    {
        get
        {
            return ramificationChance;
        }

        set
        {
            ramificationChance = value;
        }
    }

    public static int RamificationChanceInc
    {
        get
        {
            return ramificationChanceInc;
        }

        set
        {
            ramificationChanceInc = value;
        }
    }

    public static int MinAdditionalPath
    {
        get
        {
            return minAdditionalPath;
        }

        set
        {
            minAdditionalPath = value;
        }
    }

    public static int MaxAdditionalPath
    {
        get
        {
            return maxAdditionalPath;
        }

        set
        {
            maxAdditionalPath = value;
        }
    }

    public static int NodeSize
    {
        get
        {
            return nodeSize;
        }

        set
        {
            nodeSize = value;
        }
    }

    public static int NodeGap
    {
        get
        {
            return nodeGap;
        }

        set
        {
            nodeGap = value;
        }
    }

    public static int CorridorWidth
    {
        get
        {
            return corridorWidth;
        }

        set
        {
            corridorWidth = value;
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

    #endregion Accessors;

    public static void Initialize ()
    {
        if (useSeed) rand = new System.Random(seed.GetHashCode());
        else rand = new System.Random(Time.time.GetHashCode());
    }

    // North, South, East, West
    public static Direction RandomDirection ()
    {
        int totalFactor = northFactor + southFactor + eastFactor + westFactor;
        int choice = rand.Next(1, totalFactor + 1);
        if (choice <= northFactor) return Direction.North;
        if (choice <= northFactor + southFactor) return Direction.South;
        if (choice <= northFactor + southFactor + eastFactor) return Direction.East;
        else return Direction.West;
    }
}
