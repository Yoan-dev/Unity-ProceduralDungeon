using UnityEngine;
using System.Collections;

public class Node {

    // Coordinates
    private int x;
    private int y;

    // Is is the first dungeon node
    private bool first;

    // GameObject name
    private string id;

    // NodePattern
    private NodePattern nodePattern;

    // Neighboors
    private Node north;
    private Node south;
    private Node east;
    private Node west;

    #region Accessors;

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public bool First
    {
        get
        {
            return first;
        }

        set
        {
            first = value;
        }
    }

    public Node North
    {
        get
        {
            return north;
        }

        set
        {
            north = value;
        }
    }

    public Node South
    {
        get
        {
            return south;
        }

        set
        {
            south = value;
        }
    }

    public Node East
    {
        get
        {
            return east;
        }

        set
        {
            east = value;
        }
    }

    public Node West
    {
        get
        {
            return west;
        }

        set
        {
            west = value;
        }
    }

    public string Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string Id1
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    #endregion Accessors;

    public Node (bool first, string id, int x, int y)
    {
        this.first = first;
        this.id = id;
        this.x = x;
        this.y = y;
        nodePattern = new GenericNode(this);
    }

    #region Neighbooring;

    public void SetNeighboor (Node neighboor)
    {
        if (y < neighboor.Y)
        {
            north = neighboor;
            neighboor.South = this;
        }
        if (y > neighboor.Y)
        {
            south = neighboor;
            neighboor.North = this;
        }
        if (x < neighboor.X)
        {
            east = neighboor;
            neighboor.West = this;
        }
        if (x > neighboor.X)
        {
            west = neighboor;
            neighboor.East = this;
        }
    }

    public bool IsNeighboor(Node node)
    {
        return north == node || south == node || east == node || west == node;
    }

    public bool HaveANeighboor()
    {
        return !(north == null && south == null && east == null && west == null);
    }

    #endregion Neighbooring;

    #region Tiles;
    
    public Tile[,] Generate (Tile[,]  tiles)
    {
        return nodePattern.Generate(tiles, x, y);
    }

    #endregion Tiles;
}
