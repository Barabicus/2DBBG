using UnityEngine;
using System.Collections;

public class FallingSandPerlinGen : ChunkGenerator
{
    public float threshhold = 0.5f;
    public float zoom = 1f;

    public override Block GenerateBlock(float x, float y)
    {
        if (Mathf.PerlinNoise(x / (zoom), y / (zoom)) > threshhold)
        {
            return new WallBlock();
        }
        else
            return null;
    }
}
