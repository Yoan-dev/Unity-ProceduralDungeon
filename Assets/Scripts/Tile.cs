using UnityEngine;
using System.Collections;

public class Tile {

    private int x;
    private int y;
    private Resources.Tiles type;

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

    #endregion Accessors;

    public Tile (Resources.Tiles type)
    {
        this.type = type;
    }
}
