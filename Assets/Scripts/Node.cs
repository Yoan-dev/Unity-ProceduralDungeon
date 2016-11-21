using UnityEngine;
using System.Collections;

public class Node {

    private int x;
    private int y;
    private bool first;

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

    #endregion Accessors;

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
        for (int i = x * Resources.NodeSize + Resources.NodeGap; i < x * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap; i++)
        {
            for (int j = y * Resources.NodeSize + Resources.NodeGap; j < y * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap; j++)
            {
                if (i == x * Resources.NodeSize + Resources.NodeGap ||
                    i == x * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap - 1 ||
                    j == y * Resources.NodeSize + Resources.NodeGap ||
                    j == y * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap - 1)
                    tiles[i, j] = new Tile(Resources.Tiles.Wall);
                else tiles[i, j] = new Tile(Resources.Tiles.Floor);
            }
        }
        return tiles;
    }

    #endregion Tiles;
}
