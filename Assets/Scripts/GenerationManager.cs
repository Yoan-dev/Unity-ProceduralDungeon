using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationManager : MonoBehaviour {
    
    // Unity editor (temporary)
    public GameObject floor;
    public GameObject wallTop;
    public GameObject wallBottom;
    public GameObject node;

    // Generation attributes
    private Map map;
    private int currentX;
    private int currentY;
    private int nodesLimit;
    private int ramificationChance;
    
    private bool generated;
	
	void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (generated) return;
            generated = true;
            Initialize();
            while (NextStep()) { }
        }
        if (Input.GetMouseButtonUp(1))
            Clean();
    }

    #region Generation;

    private void Initialize ()
    {
        // Initializations
        Resources.Initialize();
        map = new Map();
        map.Initialize();

        // Starting room position regarding dungeon bounds
        currentX = map.GetStartingX();
        currentY = map.GetStartingY();

        // Random nodes number
        nodesLimit = Resources.Rand.Next(Resources.MinNodes, Resources.MaxNodes);
    }

    private void Clean ()
    {
        generated = false;
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }

    // Next generation step
    private bool NextStep ()
    {
        // if NextNode generation is a failure
        // we chose another origin
        if (!NextNode()) ramificationChance = 100;

        // otherwise we have only a chance
        // to change ramificate
        // (tree shape instand of giant corridor)
        if (Resources.Rand.Next(1, 101) < ramificationChance)
        {
            ramificationChance = Resources.RamificationChance;
            Node temp = map.RandomNode();
            currentX = temp.X;
            currentY = temp.Y;
        }
        else ramificationChance += Resources.RamificationChanceInc;

        // if the nodes generation is over
        if (map.NodesCount() >= nodesLimit)
        {
            GenerateMainPath();
            GenerateAddtitionalPaths();
            GenerateTiles();
            return false;
        }
        return true;
    }

    #region Nodes;

    private bool NextNode ()
    {
        // first node generation
        if (map.NodesCount() == 0) map.GenerateNode(currentX, currentY, true);
        else
        {
            // we pick a random direction
            Resources.Direction dir = Resources.RandomDirection();
            int addX = 0, addY = 0;
            switch (dir)
            {
                case Resources.Direction.North: addY++; break;
                case Resources.Direction.South: addY--; break;
                case Resources.Direction.East: addX++; break;
                case Resources.Direction.West: addX--; break;
            }
            // than we try to find a elligble location
            // for the node, if possible
            if (!FindElligible(addX, addY)) return false;
            else map.GenerateNode(currentX, currentY, false);
        }
        return true;
    }

    private bool FindElligible (int addX, int addY)
    {
        // incremently search for a possible location
        while (!map.IsElligible(currentX, currentY))
        {
            currentX += addX;
            currentY += addY;
            // if this is impossible, return false
            if (map.IsForbidden(currentX, currentY)) return false;
        }
        return true;
    }

    #endregion Nodes;

    #region Pathing;

    // Linking of all nodes using a stack
    private void GenerateMainPath ()
    {
        Stack<Node> stack = new Stack<Node>();
        IList<Node> visited = new List<Node>();
        stack.Push(map.FirstNode());
        visited.Add(map.FirstNode());
        while (visited.Count != map.NodesCount())
        {
            Node next = NextMainPath(stack.Peek());
            if (next == null) stack.Pop();
            else
            {
                stack.Peek().SetNeighboor(next);
                stack.Push(next);
                visited.Add(next);
            }
        }
    }

    // Select a random neighboorless adjacent node
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

    // Set random adjacent nodes as neighboors
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

    // Try to set a neighboor to a node
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

    #region Tiles;

    // Isometric tiles generation
    // super raw stuff, about to change
    private void GenerateTiles()
    {
        Tile[,] tiles = map.GenerateTiles();
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j] != null)
                {
                    GameObject toInstantiate = null;
                    switch (tiles[i, j].Type)
                    {
                        case Resources.Tiles.Floor: toInstantiate = floor; break;
                        case Resources.Tiles.WallTop: toInstantiate = wallTop; break;
                        case Resources.Tiles.WallBottom: toInstantiate = wallBottom; break;
                    }
                    GameObject instance = Instantiate(toInstantiate, new Vector3((j - i) * Resources.WidthUnit, (i + j) * Resources.HeightUnit), Quaternion.identity) as GameObject;
                    if (transform.FindChild(tiles[i, j].IdParent) == null)
                    {
                        GameObject container = Instantiate(node, transform) as GameObject;
                        container.name = tiles[i, j].IdParent;
                    }
                    instance.transform.SetParent(transform.FindChild(tiles[i, j].IdParent));
                }
            }
        }
    }
    //

    #endregion Tiles;

    #endregion Generation;

    #region Gizmos;

    private void OnDrawGizmos()
    {
        /*/ Draw nodes
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
        /*/

        // Draw tiles
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
                        else if (matrice[i, j].Type == Resources.Tiles.WallTop) Gizmos.color = Color.red;
                        else if (matrice[i, j].Type == Resources.Tiles.WallBottom) Gizmos.color = Color.yellow;
                        else Gizmos.color = Color.blue;
                        Gizmos.DrawCube(new Vector2(i, j), Vector2.one);
                    }
                }
            }
        }
        //
    }

    #endregion Gizmos;
}
