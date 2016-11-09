using UnityEngine;
using System.Collections;

public abstract class Room : MonoBehaviour {

    // Prefabs
    public GameObject tile;
    public GameObject wall;

    // GameObjects
    protected ArrayList tiles = new ArrayList();
    protected ArrayList walls = new ArrayList();
    private ArrayList corridorsTo = new ArrayList();

    protected System.Random rand;
    private int width;
    private int height;

    #region Accessors;

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public ArrayList CorridorsTo
    {
        get
        {
            return corridorsTo;
        }

        set
        {
            corridorsTo = value;
        }
    }

    #endregion Accessors;

    public virtual void Initialize (System.Random rand)
    {
        this.rand = rand;
    }

    public void Wall ()
    {
        //foreach (GameObject tile in tiles) TODO;
    }

    // move each tile to an isometric position
    public void Iso()
    {
        foreach (GameObject tile in tiles)
            tile.transform.position = new Vector2((tile.transform.position.y - tile.transform.position.x) * Values.WidthUnit, (tile.transform.position.x + tile.transform.position.y) * Values.HeightUnit);
    }
}
