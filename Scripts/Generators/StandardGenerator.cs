using UnityEngine;
using System.Collections;

public class StandardGenerator : ChunkGenerator
{

    public float threshhold = 0.5f;

    public override Block GenerateBlock(float x, float y)
    {
        if (Mathf.PerlinNoise(x, y) > threshhold)
        {
            return new StandardBlock();
        }
        else
            return null;
    }
}
