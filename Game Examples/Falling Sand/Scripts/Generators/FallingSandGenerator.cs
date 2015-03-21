using UnityEngine;
using System.Collections;

public class FallingSandGenerator : ChunkGenerator
{

    public override Block GenerateBlock(float x, float y)
    {

        if (y < 0.5f || x < 0.5f || x >= 255f)
            return new FallingSteel();
        else
            return null;
    }
}
