using UnityEngine;
using System.Collections;

public class Tile {
    
    private Resources.Tiles type;
    private Node parent;

    #region Accessors;
    
    public Resources.Tiles Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public Node Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }

    #endregion Accessors;

    public Tile (Resources.Tiles type, Node parent)
    {
        this.type = type;
        this.parent = parent;
    }
}
