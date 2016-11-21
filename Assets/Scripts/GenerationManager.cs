using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationManager : MonoBehaviour {
    
    private Map map;
    private int currentX;
    private int currentY;
    private int nodesLimit;
    private int ramificationChance;
	
	void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Initialize();
            while (NextStep()) { }
            GenerateMainPath();
            GenerateAddtitionalPaths();
            map.GenerateTiles();
        }
	}

    #region Generation;

    private void Initialize ()
    {
        Resources.Initialize();
        map = new Map();
        map.Initialize();
        currentX = map.GetStartingX();
        currentY = map.GetStartingY();
        nodesLimit = Resources.Rand.Next(Resources.MinNodes, Resources.MaxNodes);
    }

    private bool NextStep ()
    {
        if (map.NodesCount() >= nodesLimit) return false;
        if (!NextNode()) ramificationChance = 100;
        if (Resources.Rand.Next(1, 101) < ramificationChance)
        {
            ramificationChance = Resources.RamificationChance;
            Node temp = map.RandomNode();
            currentX = temp.X;
            currentY = temp.Y;
        }
        else ramificationChance += Resources.RamificationChanceInc;
        return true;
    }

    #region Nodes;

    private bool NextNode ()
    {
        if (map.NodesCount() == 0) map.GenerateNode(currentX, currentY, true);
        else
        {
            Resources.Direction dir = Resources.RandomDirection();
            int addX = 0, addY = 0;
            switch (dir)
            {
                case Resources.Direction.North: addY++; break;
                case Resources.Direction.South: addY--; break;
                case Resources.Direction.East: addX++; break;
                case Resources.Direction.West: addX--; break;
            }
            if (!FindElligible(addX, addY)) return false;
            else map.GenerateNode(currentX, currentY, false);
        }
        return true;
    }

    private bool FindElligible (int addX, int addY)
    {
        while (!map.IsElligible(currentX, currentY))
        {
            currentX += addX;
            currentY += addY;
            if (map.IsForbidden(currentX, currentY)) return false;
        }
        return true;
    }

    #endregion Nodes;

    #region Pathing;

    private void GenerateMainPath ()
    {
        Stack<Node> stack = new Stack<Node>();
        IList<Node> done = new List<Node>();
        stack.Push(map.FirstNode());
        done.Add(map.FirstNode());
        while (done.Count != map.NodesCount())
        {
            Node next = NextMainPath(stack.Peek());
            if (next == null) stack.Pop();
            else
            {
                stack.Peek().SetNeighboor(next);
                stack.Push(next);
                done.Add(next);
            }
        }
    }

    private Node NextMainPath (Node node)
    {
        ArrayList choicePool = new ArrayList();
        Node temp;
        int[] values = new int[] { 1, -1, 0, 0 };
        for (int i = 0, j = values.Length - 1; i < values.Length; i++, j--)
        {
            temp = map.GetNode(node.X + values[i], node.Y + values[j]);
            if (temp != null && !temp.HaveANeighboor()) choicePool.Add(temp);
        }
        if (choicePool.Count == 0) return null;
        else return choicePool[Resources.Rand.Next(0, choicePool.Count)] as Node;
    }

    private void GenerateAddtitionalPaths ()
    {
        int additionalPathLimit = Resources.Rand.Next(Resources.MinAdditionalPath, Resources.MaxAdditionalPath + 1);
        int additionalPathCount = 0;
        ArrayList pool = map.NodesClone();
        Node choice;
        while (additionalPathCount < additionalPathLimit)
        {
            if (pool.Count == 0) return;
            choice = pool[Resources.Rand.Next(0, pool.Count)] as Node;
            bool success = AddPath(choice);
            if (!success) pool.Remove(choice);
            else additionalPathCount++;
        }
    }

    private bool AddPath (Node node)
    {
        ArrayList choicePool = new ArrayList();
        Node temp = null;
        int[] values = new int[] { 1, -1, 0, 0 };
        for (int i = 0, j = values.Length - 1; i < values.Length; i++, j--)
        {
            temp = map.GetNode(node.X + values[i], node.Y + values[j]);
            if (temp != null && !node.IsNeighboor(temp)) choicePool.Add(temp);
        }
        if (choicePool.Count == 0) return false;
        node.SetNeighboor(choicePool[Resources.Rand.Next(0, choicePool.Count)] as Node);
        return true;
    }

    #endregion Pathing;

    #endregion Generation;
    
    private void OnDrawGizmos()
    {
        /*
        if (map != null)
        {
            Node[,] matrice = map.MatriceClone();
            if (matrice != null)
            {
                int i2 = 0, j2 = 0;
                for (int i = 0; i < matrice.GetLength(0); i++)
                {
                    i2++;
                    j2 = 0;
                    for (int j = 0; j < matrice.GetLength(1); j++)
                    {
                        j2++;
                        if (matrice[i, j] == null) Gizmos.color = Color.white;
                        else if (matrice[i, j].First) Gizmos.color = Color.red;
                        else Gizmos.color = Color.blue;
                        Gizmos.DrawCube(new Vector2(i + i2, j + j2), Vector2.one);
                        if (matrice[i, j] != null)
                        {
                            Gizmos.color = Color.yellow;
                            if (matrice[i, j].North != null) Gizmos.DrawCube(new Vector2(i + i2, j + j2 + 1), Vector2.one);
                            if (matrice[i, j].South != null) Gizmos.DrawCube(new Vector2(i + i2, j + j2 - 1), Vector2.one);
                            if (matrice[i, j].West != null) Gizmos.DrawCube(new Vector2(i + i2 - 1, j + j2), Vector2.one);
                            if (matrice[i, j].East != null) Gizmos.DrawCube(new Vector2(i + i2 + 1, j + j2), Vector2.one);
                        }
                    }
                }
            }
        }
        */
        if (map != null)
        {
            Tile[,] matrice = map.TilesClone();
            if (matrice != null)
            {
                for (int i = 0; i < matrice.GetLength(0); i++)
                {
                    for (int j = 0; j < matrice.GetLength(1); j++)
                    {
                        if (matrice[i, j] == null) Gizmos.color = Color.white;
                        else if (matrice[i, j].Type == Resources.Tiles.Wall) Gizmos.color = Color.red;
                        else Gizmos.color = Color.blue;
                        Gizmos.DrawCube(new Vector2(i, j), Vector2.one);
                    }
                }
            }
        }
    }
}
