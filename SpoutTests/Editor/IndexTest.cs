using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class IndexTest : MonoBehaviour {

    int blockSize = 500;
    int chunkSize = 5;

    [Test]
    public void Something()
    {
        for (int x = 0; x < 100; x+=5)
        {
            for (int y = 0; y < 100; y+=5)
            {
                Debug.Log(ToIndex(new Vector2(x, y)) + " : " + new Vector2(x,y));
               // ToIndex(new Vector2(x,y));
            }
        }
    }

    Vector2 ToIndex(Vector2 position)
    {
        Vector2 points = new Vector2(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        Vector2 mod = new Vector2(points.x % (blockSize / 100f), points.y % (blockSize / 100f));
        Vector2 index = points - mod;
        index /= chunkSize;
        return index;
    }

}
