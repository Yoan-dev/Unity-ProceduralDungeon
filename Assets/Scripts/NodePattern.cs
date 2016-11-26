using UnityEngine;
using System.Collections;

public abstract class NodePattern {

    protected Node node;

    public NodePattern (Node node)
    {
        this.node = node;
    }

    // super raw stuff, about to change
    public Tile[,] Generate(Tile[,] tiles, int x, int y)
    {
        int iStart = x * Resources.NodeSize + Resources.NodeGap;
        int iEnd = x * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap;
        int jStart = y * Resources.NodeSize + Resources.NodeGap;
        int jEnd = y * Resources.NodeSize + Resources.NodeSize - Resources.NodeGap;
        for (int i = iStart; i < iEnd; i++)
        {
            for (int j = jStart; j < jEnd; j++)
            {
                if (i == iStart || j == jStart)
                    tiles[i, j] = new Tile(Resources.Tiles.WallBottom, node.Id);
                else if (i == iEnd - 1 || j == jEnd - 1)
                    tiles[i, j] = new Tile(Resources.Tiles.WallTop, node.Id);
                else tiles[i, j] = new Tile(Resources.Tiles.Floor, node.Id);
            }
        }
        if (node.North != null || node.First && !Resources.NorthAccess) tiles = WallNorth(tiles, x, y);
        if (node.South != null || node.First && !Resources.SouthAccess) tiles = WallSouth(tiles, x, y);
        if (node.East != null || node.First && !Resources.EastAccess) tiles = WallEast(tiles, x, y);
        if (node.West != null || node.First && !Resources.WestAccess) tiles = WallWest(tiles, x, y);

        return tiles;
    }

    private Tile[,] WallNorth(Tile[,] tiles, int x, int y)
    {
        int iStart = (y + 1) * Resources.NodeSize - Resources.NodeGap - 1;
        int iEnd = (y + 1) * Resources.NodeSize;
        int jStart = x * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth;
        int jEnd = x * Resources.NodeSize + 2 + Resources.NodeSize / 2;
        for (int i = iStart; i < iEnd; i++)
        {
            for (int j = jStart; j < jEnd; j++)
            {
                if (j == jEnd - 1 || i == iStart && j == jStart) tiles[j, i] = new Tile(Resources.Tiles.WallTop, GenerateCorridorId(node, node.North));
                else if (j == jStart) tiles[j, i] = new Tile(Resources.Tiles.WallBottom, GenerateCorridorId(node, node.North));
                else tiles[j, i] = new Tile(Resources.Tiles.Floor, GenerateCorridorId(node, node.North));
            }
        }
        return tiles;
    }

    private Tile[,] WallSouth(Tile[,] tiles, int x, int y)
    {
        int iStart = y * Resources.NodeSize;
        int iEnd = y * Resources.NodeSize + Resources.NodeGap + 1;
        int jStart = x * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth;
        int jEnd = x * Resources.NodeSize + 2 + Resources.NodeSize / 2;
        for (int i = iStart; i < iEnd; i++)
        {
            for (int j = jStart; j < jEnd; j++)
            {
                if (j == jStart) tiles[j, i] = new Tile(Resources.Tiles.WallBottom, GenerateCorridorId(node, node.South));
                else if (j == jEnd - 1) tiles[j, i] = new Tile(Resources.Tiles.WallTop, GenerateCorridorId(node, node.South));
                else tiles[j, i] = new Tile(Resources.Tiles.Floor, GenerateCorridorId(node, node.South));
            }
        }
        return tiles;
    }

    private Tile[,] WallEast(Tile[,] tiles, int x, int y)
    {
        int iStart = (x + 1) * Resources.NodeSize - Resources.NodeGap - 1;
        int iEnd = (x + 1) * Resources.NodeSize;
        int jStart = y * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth;
        int jEnd = y * Resources.NodeSize + 2 + Resources.NodeSize / 2;
        for (int i = iStart; i < iEnd; i++)
        {
            for (int j = jStart; j < jEnd; j++)
            {
                if (j == jEnd - 1 || i == iStart && j == jStart) tiles[i, j] = new Tile(Resources.Tiles.WallTop, GenerateCorridorId(node, node.East));
                else if (j == jStart) tiles[i, j] = new Tile(Resources.Tiles.WallBottom, GenerateCorridorId(node, node.East));
                else tiles[i, j] = new Tile(Resources.Tiles.Floor, GenerateCorridorId(node, node.East));
            }
        }
        return tiles;
    }

    private Tile[,] WallWest(Tile[,] tiles, int x, int y)
    {
        int iStart = x * Resources.NodeSize;
        int iEnd = x * Resources.NodeSize + Resources.NodeGap + 1;
        int jStart = y * Resources.NodeSize + Resources.NodeSize / 2 - Resources.CorridorWidth;
        int jEnd = y * Resources.NodeSize + 2 + Resources.NodeSize / 2;
        for (int i = iStart; i < iEnd; i++)
        {
            for (int j = jStart; j < jEnd; j++)
            {
                if (j == jStart) tiles[i, j] = new Tile(Resources.Tiles.WallBottom, GenerateCorridorId(node, node.West));
                else if (j == jEnd - 1) tiles[i, j] = new Tile(Resources.Tiles.WallTop, GenerateCorridorId(node, node.West));
                else tiles[i, j] = new Tile(Resources.Tiles.Floor, GenerateCorridorId(node, node.West));
            }
        }
        return tiles;
    }

    private string GenerateCorridorId(Node n1, Node n2)
    {
        if (n2 == null) return "corridor-entrance" + n1.Id;
        if (n1.Id.GetHashCode() > n2.Id.GetHashCode())
            return "corridor-" + n1.Id + "-" + n2.Id;
        else return "corridor-" + n2.Id + "-" + n1.Id;
    }
    //

}
