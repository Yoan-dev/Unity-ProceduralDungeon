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

    // super raw stuff
    public Tile[,] Generate (Tile[,]  tiles)
    {
        for (int i = x * Resources.NodeSize + Resources.NodeGap; i < x * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap; i++)
        {
            for (int j = y * Resources.NodeSize + Resources.NodeGap; j < y * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap; j++)
            {
                if (
                    i == x * Resources.NodeSize + Resources.NodeGap ||
                    j == y * Resources.NodeSize + Resources.NodeGap)
                    tiles[i, j] = new Tile(Resources.Tiles.WallBottom, this);
                else if (
                    i == x * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap - 1 || 
                    j == y * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap - 1)
                    tiles[i, j] = new Tile(Resources.Tiles.WallTop, this);
                else tiles[i, j] = new Tile(Resources.Tiles.Floor, this);
            }
        }
        if (North != null) tiles = WallNorth(tiles);
        if (South != null) tiles = WallSouth(tiles);
        if (East != null) tiles = WallEast(tiles);
        if (West != null) tiles = WallWest(tiles);

        return tiles;
    }

    private Tile[,] WallNorth(Tile[,] tiles)
    {
        for (int i = (y + 1) * Resources.NodeSize - Resources.NodeGap - 1; i < (y + 1) * Resources.NodeSize; i++)
        {
            for (int j = x * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth; j < x * Resources.NodeSize + 2 + Resources.NodeSize / 2; j++)
            {
                if (j == x * Resources.NodeSize + 2 + Resources.NodeSize / 2 - 1 ||
                    i == (y + 1) * Resources.NodeSize - Resources.NodeGap - 1 &&
                    j == x * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth)
                    tiles[j, i] = new Tile(Resources.Tiles.WallTop, this);
                else if (j == x * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth)
                    tiles[j, i] = new Tile(Resources.Tiles.WallBottom, this);
                else tiles[j, i] = new Tile(Resources.Tiles.Floor, this);
            }
        }
        return tiles;
    }

    private Tile[,] WallSouth(Tile[,] tiles)
    {
        for (int i = y * Resources.NodeSize; i < y * Resources.NodeSize + Resources.NodeGap + 1; i++)
        {
            for (int j = x * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth; j < x * Resources.NodeSize + 2 + Resources.NodeSize / 2; j++)
            {
                if (j == x * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth)
                    tiles[j, i] = new Tile(Resources.Tiles.WallBottom, this);
                else if (j == x * Resources.NodeSize + 2 + Resources.NodeSize / 2 - 1)
                    tiles[j, i] = new Tile(Resources.Tiles.WallTop, this);
                else tiles[j, i] = new Tile(Resources.Tiles.Floor, this);
            }
        }
        return tiles;
    }

    private Tile[,] WallEast(Tile[,] tiles)
    {
        for (int i = (x + 1) * Resources.NodeSize - Resources.NodeGap - 1; i < (x + 1) * Resources.NodeSize; i++)
        {
            for (int j = y * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth; j < y * Resources.NodeSize + 2 + Resources.NodeSize / 2; j++)
            {
                if (j == y * Resources.NodeSize + 2 + Resources.NodeSize / 2 - 1 ||
                    i == (x + 1) * Resources.NodeSize - Resources.NodeGap - 1 &&
                    j == y * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth)
                    tiles[i, j] = new Tile(Resources.Tiles.WallTop, this);
                else if (j == y * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth)
                    tiles[i, j] = new Tile(Resources.Tiles.WallBottom, this);
                else tiles[i, j] = new Tile(Resources.Tiles.Floor, this);
            }
        }
        return tiles;
    }

    private Tile[,] WallWest(Tile[,] tiles)
    {
        for (int i = x * Resources.NodeSize; i < x * Resources.NodeSize + Resources.NodeGap + 1; i++)
        {
            for (int j = y * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth; j < y * Resources.NodeSize + 2 + Resources.NodeSize / 2; j++)
            {
                if (j == y * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth)
                    tiles[i, j] = new Tile(Resources.Tiles.WallBottom, this);
                else if (j == y * Resources.NodeSize + 2 + Resources.NodeSize / 2 - 1)
                    tiles[i, j] = new Tile(Resources.Tiles.WallTop, this);
                else tiles[i, j] = new Tile(Resources.Tiles.Floor, this);
            }
        }
        return tiles;
    }
    //

    #endregion Tiles;
}
