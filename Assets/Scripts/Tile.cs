using UnityEngine;
using System.Collections;

public class Tile {
    
    private Resources.Tiles type;
    private string idParent;

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

    public string IdParent
    {
        get
        {
            return idParent;
        }

        set
        {
            idParent = value;
        }
    }

    #endregion Accessors;

    public Tile (Resources.Tiles type, string idParent)
    {
        this.type = type;
        this.idParent = idParent;
    }
}
