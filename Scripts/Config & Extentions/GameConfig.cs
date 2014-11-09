using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        // Bullets ignore bullets
        Physics2D.IgnoreLayerCollision(9, 9);
        // Bullets ignore player
        Physics2D.IgnoreLayerCollision(9, 10);
        // Chunk Ignores Chunk
        Physics2D.IgnoreLayerCollision(11, 11);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}

public static class Vector3Extention
{

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    public static string ToStringProper(this Vector2 vector)
    {
        return "(" + vector.x + "," + vector.y + ")";
    }

    public static string ToStringProper(this Vector3 vector)
    {
        return "(" + vector.x + "," + vector.y + ")";
    }
}

public static class MathfExtention
{
    public static float FloorFloat2Decimal(this Mathf math)
    {
        return 0.0f;
    }
}
