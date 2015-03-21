using UnityEngine;
using System.Collections;

public class MiningAsteroidGeneratorSmall : ChunkGenerator
{

    public Vector2 centerPoint;
    public float radius;
    public float perlinAdd = 2f;

    public override void PreLoadGenerator(Chunk chunk)
    {
        base.PreLoadGenerator(chunk);
        radius = Random.Range(1f, 3f);
        perlinAdd = Random.Range(1f, 3f);
    }

    public override Block GenerateBlock(float x, float y)
    {
        Vector2 point = new Vector2(x, y);

            if (Vector2.Distance(point, centerPoint) < radius + (Mathf.PerlinNoise(x, y) * perlinAdd))
                return new MiningAsteroidBlock();
        return null;
    }
}
