using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    public GameObject generation;

    private System.Random rand;
    private String seed;

    #region Accessors;

    public System.Random Rand
    {
        get
        {
            return rand;
        }

        set
        {
            rand = value;
        }
    }

    public string Seed
    {
        get
        {
            return seed;
        }

        set
        {
            seed = value;
        }
    }

    #endregion Accessors;

    void Start ()
    {
        InitializeSeed(null);
        (generation.GetComponent("Generation") as Generation).GameManager = this;
	}

    // if seed is null, it means we use a random seed
    public void InitializeSeed (String seed)
    {
        if (seed == null) this.seed = Time.time.ToString();
        else this.seed = seed;
        rand = new System.Random(this.seed.GetHashCode());
    }
}
