using UnityEngine;
using System.Collections;

public class PlanetGenerator : ChunkGenerator {

    public Vector2 centerPoint;
    public float radius;
    public float perlinAdd = 2f;

    public override Block GenerateBlock(float x, float y)
    {
        Vector2 point = new Vector2(x, y);
        if (Vector2.Distance(point, centerPoint) < radius + (Mathf.PerlinNoise(x, y) * perlinAdd))
            return new StandardBlock();
        else
            return null;
    }
}
