using UnityEngine;
using System.Collections;

public class StandardGenerator : ChunkGenerator
{

    public float threshhold = 0.5f;
    public float zoom = 1f;

    public override Block GenerateBlock(float x, float y)
    {
        if (Mathf.PerlinNoise(x / (zoom), y / (zoom)) > threshhold)
        {
            return new StandardBlock();
        }
        else
            return null;
    }
}
