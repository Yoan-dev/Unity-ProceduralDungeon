using UnityEngine;
using System.Collections;

public class Tile {

    private int x;
    private int y;

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

    #endregion Accessors;
}
