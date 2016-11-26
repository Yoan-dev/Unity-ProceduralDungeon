using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

    private Node[,] matrice;
    private IList<Node> nodes = new List<Node>();

    private Tile[,] tiles;

    #region Initialization;

    public void Initialize ()
    {
        matrice = new Node[
            Resources.Rand.Next(Resources.MinWidth, Resources.MaxWidth + 1),
            Resources.Rand.Next(Resources.MinHeight, Resources.MaxHeight + 1)];

        tiles = new Tile[
            matrice.GetLength(0) * Resources.NodeSize, 
            matrice.GetLength(1) * Resources.NodeSize];
    }

    public int GetStartingX()
    {
        if (!Resources.EastAccess) return matrice.GetLength(0) - 1;
        if (!Resources.WestAccess) return 0;
        return matrice.GetLength(0) / 2;
    }

    public int GetStartingY()
    {
        if (!Resources.NorthAccess) return matrice.GetLength(1) - 1;
        if (!Resources.SouthAccess) return 0;
        return matrice.GetLength(1) / 2;
    }

    #endregion Initialization;

    #region Nodes;

    public void GenerateNode (int x, int y, bool first)
    {
        Node node = new Node(first, "node" + nodes.Count, x, y);
        matrice[x, y] = node;
        nodes.Add(node);
    }

    public int NodesCount ()
    {
        return nodes.Count;
    }

    public Node RandomNode ()
    {
        return nodes[Resources.Rand.Next(0, nodes.Count)];
    }

    public bool IsElligible (int x, int y)
    {
        return !IsForbidden(x, y) && matrice[x, y] == null;
    }

    public bool IsForbidden (int x, int y)
    {
        return !(x >= 0 && x < matrice.GetLength(0) && y >= 0 && y < matrice.GetLength(1));
    }

    public Node[,] MatriceClone ()
    {
        return matrice.Clone() as Node[,];
    }

    public ArrayList NodesClone ()
    {
        ArrayList res = new ArrayList();
        foreach (Node node in nodes)
            res.Add(node);
        return res;
    }

    public Node FirstNode ()
    {
        return nodes[0];
    }

    public Node GetNode (int x, int y)
    {
        if (!IsForbidden(x, y) && !IsElligible(x, y)) return matrice[x, y];
        else return null;
    }

    #endregion Nodes;

    #region Tiles;

    public Tile[,] GenerateTiles ()
    {
        foreach (Node node in nodes)
            tiles = node.Generate(tiles);
        return tiles;
    }

    public Tile[,] TilesClone()
    {
        return tiles.Clone() as Tile[,];
    }

    #endregion Tiles;
}
