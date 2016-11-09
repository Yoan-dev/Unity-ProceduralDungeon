using UnityEngine;
using System.Collections;

public class GenericRoom : Room {

    public override void Initialize (System.Random rand)
    {
        base.Initialize(rand);
        Width = rand.Next(Values.MinimumRoomSize, Values.MaximumRoomSize);
        Height = rand.Next(Values.MinimumRoomSize, Values.MaximumRoomSize);

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                GameObject instance = Instantiate(tile, new Vector2(i, j), Quaternion.identity, transform) as GameObject;
                tiles.Add(instance);
            }
        }
    }
}
