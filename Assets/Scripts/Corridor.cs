using UnityEngine;
using System.Collections;
using System;

public class Corridor : Room {

    public bool Initialize (GameObject from, GameObject to, System.Random rand)
    {
        base.Initialize(rand);
        float x1 = from.transform.position.x,
            y1 = from.transform.position.y,
            x2 = to.transform.position.x,
            y2 = to.transform.position.y,
            w1 = (from.GetComponent("Room") as Room).Width,
            h1 = (from.GetComponent("Room") as Room).Height,
            w2 = (to.GetComponent("Room") as Room).Width,
            h2 = (to.GetComponent("Room") as Room).Height;

        // we generate a first corridor in order to have
        // each room accessible in our dungeon
        if (VerticalCorridor(x1, y1, w1, h1, x2, y2, w2, h2, rand)) return true;
        if (HorizontalCorridor(x1, y1, w1, h1, x2, y2, w2, h2, rand)) return true;

        // if it is not possible, the two rooms cannot have
        // a corridor between them
        return false;
    }

    /* to generate a vertical / horizontal corridor
     * we need a minimum number of facing tiles
     * between the two rooms (minimum corridor width) */

    private bool VerticalCorridor(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2, System.Random rand)
    {
        int gap = (int)Mathf.Min(x1 + w1, x2 + w2) - (int)Mathf.Max(x1, x2);
        if (gap < Values.MinimumCorridorWidth) return false;
        int width = rand.Next(2, Mathf.Min(gap + 1, Values.MaximumCorridorWidth + 1));
        int startX = rand.Next((int)Mathf.Max(x1, x2), (int)Mathf.Min(x1 + w1 - width, x2 + w2 - width) + 1);
        for (int i = (int)((y1 < y2) ? y1 + h1 : y2 + h2); i < (int)((y1 > y2) ? y1 : y2); i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject instance = Instantiate(tile, new Vector2(startX + j, i), Quaternion.identity, transform) as GameObject;
                tiles.Add(instance);
            }
        }
        return true;
    }

    private bool HorizontalCorridor(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2, System.Random rand)
    {
        int gap = (int)Mathf.Min(y1 + h1, y2 + h2) - (int)Mathf.Max(y1, y2);
        if (gap < Values.MinimumCorridorWidth) return false;
        int width = rand.Next(2, Mathf.Min(gap + 1, Values.MaximumCorridorWidth + 1));
        int startY = rand.Next((int)Mathf.Max(y1, y2), (int)Mathf.Min(y1 + h1 - width, y2 + h2 - width) + 1);
        for (int i = (int)((x1 < x2) ? x1 + w1 : x2 + w2); i < (int)((x1 > x2) ? x1 : x2); i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject instance = Instantiate(tile, new Vector2(i, startY + j), Quaternion.identity, transform) as GameObject;
                tiles.Add(instance);
            }
        }
        return true;
    }
}
