using UnityEngine;
using System.Collections;

public class MiningAsteroidGenerator : ChunkGenerator
{

    public Vector2[] centerPoint;
    public float[] radius;
    public float perlinAdd = 2f;

    public int asteroidAmount = 10;
    public float pointRangeMin = 5;
    public float pointRangeMax = 50;

    [ContextMenu("Generate Points")]
    public void GeneratePoints()
    {
        centerPoint = new Vector2[asteroidAmount];
        radius = new float[asteroidAmount];

        for (int i = 0; i < asteroidAmount; i++)
        {
            centerPoint[i] = new Vector2(Random.Range(0, pointRangeMax), Random.Range(0, pointRangeMax));
            radius[i] = Random.Range(1, 5);
        }
    }

    public override Block GenerateBlock(float x, float y)
    {
        Vector2 point = new Vector2(x, y);

        for (int i = 0; i < centerPoint.Length; i++)
                if (Vector2.Distance(point, centerPoint[i]) < radius[i] + (Mathf.PerlinNoise(x, y) * perlinAdd))
                    return new MiningAsteroidBlock();
        return null;
    }
}
